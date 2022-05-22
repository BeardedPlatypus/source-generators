using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using BeardedPlatypus.SourceGenerators.Annotations.Visitor;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using VisitableDescription = System.Tuple<Microsoft.CodeAnalysis.INamedTypeSymbol,
                                          System.Collections.Generic.ICollection<Microsoft.CodeAnalysis.INamedTypeSymbol>>;

namespace BeardedPlatypus.SourceGenerators.Visitor
{
    /// <summary>
    /// <see cref="VisitableGenerator"/> is responsible for generating the visitor
    /// pattern of any interfaces annotated with the <see cref="VisitableAttribute"/>.
    /// </summary>
    [Generator]
    public class VisitableGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxContextReceiver is SyntaxReceiver syntaxReceiver)) return;

            GenerateVisitorInterfaces(context, syntaxReceiver);
            GenerateInterfaceExtensions(context, syntaxReceiver);
            GenerateClassExtensions(context, syntaxReceiver);
        }

        private static void GenerateVisitorInterfaces(GeneratorExecutionContext context, 
                                                      SyntaxReceiver syntaxReceiver)
        {
            foreach ((INamedTypeSymbol interfaceSymbol, IEnumerable<INamedTypeSymbol> classSymbols) in syntaxReceiver.InterfacesToExtend)
            {
                // TODO: Add logic which validates this and gives feedback to the consumer.
                string visitorName = GetVisitorName(interfaceSymbol);
                SourceText sourceText = SourceText.From(
                    Template.VisitorInterface(
                        GetAccessibilityString(interfaceSymbol), 
                        visitorName, 
                        interfaceSymbol.ToDisplayString(),
                        interfaceSymbol.ContainingNamespace.ToDisplayString(), 
                        classSymbols.Select(cs => cs.ToDisplayString())),
                    Encoding.UTF8);
                // TODO: verify if this is correct.
                context.AddSource(Template.VisitorInterfaceFileName(visitorName), sourceText);
            }
        }

        // TODO: Extend for other declarations, or just give an error?
        private static string GetAccessibilityString(INamedTypeSymbol symbol) =>
            symbol.DeclaredAccessibility == Accessibility.Public
                ? "public" 
                : "internal";

        // TODO: Extend this with attribute retrieval to allow for customization of the visitor name.
        private static string GetVisitorName(INamedTypeSymbol interfaceSymbol, bool fullyQualified = false)
        {
            string name = fullyQualified ? interfaceSymbol.ToDisplayString() : interfaceSymbol.Name;
            return $"{name}Visitor";
        }

        private void GenerateInterfaceExtensions(GeneratorExecutionContext context, 
                                                 SyntaxReceiver syntaxReceiver)
        {
            foreach ((INamedTypeSymbol interfaceSymbol, IEnumerable<INamedTypeSymbol> _) in syntaxReceiver.InterfacesToExtend)
            {
                SourceText sourceText = SourceText.From(
                    Template.VisitableInterfaceExtension(
                        GetAccessibilityString(interfaceSymbol), 
                        interfaceSymbol.Name, 
                        interfaceSymbol.ContainingNamespace.ToDisplayString(), 
                        GetVisitorName(interfaceSymbol, fullyQualified:true)), 
                    Encoding.UTF8);
                context.AddSource(Template.VisitableExtensionFileName(interfaceSymbol.Name), sourceText);
            }
        }

        private void GenerateClassExtensions(GeneratorExecutionContext context, SyntaxReceiver syntaxReceiver)
        {
            foreach ((INamedTypeSymbol classSymbol, IEnumerable<INamedTypeSymbol> interfaceSymbols) in syntaxReceiver.ClassesToExtend)
            {
                // TODO: Extend this to incorporate the correct abstract behaviour
                SourceText sourceText = SourceText.From(
                    Template.VisitableClassExtension(
                        GetAccessibilityString(classSymbol), 
                        classSymbol.Name, 
                        classSymbol.ContainingNamespace.ToDisplayString(), 
                        interfaceSymbols.Select(s => new Tuple<string, string, bool>(GetAccessibilityString(s), GetVisitorName(s, true), false))),
                    Encoding.UTF8); 
                context.AddSource(Template.VisitableExtensionFileName(classSymbol.Name), sourceText);
            }
        }

        /// <summary>
        /// <see cref="VisitableGenerator.SyntaxReceiver"/> is responsible for collecting the
        /// relevant syntax information.
        /// </summary>
        internal class SyntaxReceiver : ISyntaxContextReceiver
        {
            private static readonly string AttributeName = nameof(VisitableAttribute);

            // Tuple containing the class which needs to be extended, and the set of interfaces which it should extend.
            private readonly IList<VisitableDescription> _classesToExtend =
                new List<VisitableDescription>();

            // Tuple containing the interface which needs to be implemented and the set of classes that implement it.
            private readonly IDictionary<INamedTypeSymbol, IList<INamedTypeSymbol>> _interfacesToExtend =
                new SortedDictionary<INamedTypeSymbol, IList<INamedTypeSymbol>>();

            /// <summary>
            /// Gets the classes to extend, and the interfaces that should be implemented.
            /// </summary>
            public IEnumerable<VisitableDescription> ClassesToExtend =>
                _classesToExtend;

            /// <summary>
            /// Gets the interfaces to extend, and the classes that implement the interfaces.
            /// </summary>
            public IEnumerable<VisitableDescription> InterfacesToExtend =>
                _interfacesToExtend.Select(kvp => new VisitableDescription(kvp.Key, kvp.Value));

            public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
            {
                switch (context.Node)
                {
                    case ClassDeclarationSyntax classDeclarationSyntax
                        when TryGetVisitableClassDescription(classDeclarationSyntax,
                                                             context.SemanticModel,
                                                             out VisitableDescription visitableClass):
                        _classesToExtend.Add(visitableClass);
                        AddClassToInterfaces(visitableClass);
                        break;
                }
            }

            private void AddClassToInterfaces(VisitableDescription visitableClass)
            {
                foreach (INamedTypeSymbol interfaceSymbol in visitableClass.Item2)
                {
                    if (!_interfacesToExtend.TryGetValue(interfaceSymbol, out IList<INamedTypeSymbol> classes))
                    {
                        classes = new List<INamedTypeSymbol>();
                        _interfacesToExtend[interfaceSymbol] = classes;
                    }
                    
                    classes.Add(visitableClass.Item1);
                }
            }

            private static bool IsVisitableInterfaceSymbol(INamedTypeSymbol interfaceSymbol) =>
                interfaceSymbol?.GetAttributes().Any(IsVisitableAttribute) ?? false;

            private static bool IsVisitableAttribute(AttributeData attribute) =>
                attribute.AttributeClass?.Name == AttributeName ||
                attribute.AttributeClass?.Name == AttributeName.Replace("Attribute", "");


            private static bool TryGetVisitableClassDescription(ClassDeclarationSyntax classDeclarationSyntax,
                                                                SemanticModel semanticModel,
                                                                out VisitableDescription visitableClassDescription)
            {
                visitableClassDescription = null;

                var symbol = semanticModel.GetDeclaredSymbol(classDeclarationSyntax);
                ISet<INamedTypeSymbol> visitableInterfaces =
                    symbol.AllInterfaces.Where(IsVisitableInterfaceSymbol).ToImmutableHashSet();

                if (!visitableInterfaces.Any()) return false;

                ImmutableHashSet<INamedTypeSymbol> parentVisitableInterfaces =
                    symbol.BaseType?.AllInterfaces.Where(IsVisitableInterfaceSymbol).ToImmutableHashSet();

                if (parentVisitableInterfaces != null && !parentVisitableInterfaces.IsEmpty)
                    visitableInterfaces.ExceptWith(parentVisitableInterfaces);

                if (!visitableInterfaces.Any()) return false;

                visitableClassDescription = new VisitableDescription(symbol, visitableInterfaces);
                return true;
            }
        }
    }
}
