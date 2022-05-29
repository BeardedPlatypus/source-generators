using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using BeardedPlatypus.SourceGenerators.Utility.Internal;
using Microsoft.CodeAnalysis;

namespace BeardedPlatypus.SourceGenerators.Utility.CodeGeneration
{
    public sealed class SourceBuilder : ISourceBuilder
    {
        private readonly IImmutableList<string> _usings;
        private readonly IImmutableList<ISourceElementBuilder> _sourceElements;

        private readonly string _namespace;

        public SourceBuilder() : this(ImmutableList.Create<string>(),
                                      ImmutableList.Create<ISourceElementBuilder>(),
                                      null) 
        {}

        private SourceBuilder(SourceBuilder srcSourceBuilder,
                              IImmutableList<string> updatedUsings = null,
                              IImmutableList<ISourceElementBuilder> updatedSourceElements = null,
                              string updatedNamespace = null) :
            this(updatedUsings ?? srcSourceBuilder._usings,
                 updatedSourceElements ?? srcSourceBuilder._sourceElements,
                 updatedNamespace ?? srcSourceBuilder._namespace) 
        { }


        private SourceBuilder(IImmutableList<string> usings, 
                              IImmutableList<ISourceElementBuilder> sourceElements,
                              string @namespace)
        {
            _usings = usings;
            _sourceElements = sourceElements;
            _namespace = @namespace;
        }

        public ISourceBuilder WithUsing(string @using) =>
            WithUsings(@using);

        public ISourceBuilder WithUsing(INamespaceSymbol @using) =>
            WithUsings(@using);

        public ISourceBuilder WithUsings(params string[] usings)
        {
            Ensure.ContainsNoNull(usings, nameof(usings));
            return new SourceBuilder(this, updatedUsings: _usings.AddRange(usings));
        }

        public ISourceBuilder WithUsings(params INamespaceSymbol[] usings) =>
            WithUsings(usings?.Select(u => u?.ToDisplayString()).ToArray());

        public ISourceBuilder WithNamespace(string @namespace)
        {
            Ensure.NotNull(@namespace, nameof(@namespace));
            return new SourceBuilder(this, updatedNamespace: @namespace);
        }

        public ISourceBuilder WithNamespace(INamespaceSymbol @namespace) =>
            WithNamespace(@namespace?.ToDisplayString());

        public ISourceBuilder WithSourceElement(ISourceElementBuilder sourceElementBuilder)
        {
            Ensure.NotNull(sourceElementBuilder, nameof(sourceElementBuilder));
            return new SourceBuilder(this, updatedSourceElements: _sourceElements.Add(sourceElementBuilder));
        }

        // TODO: Add correct spacing.
        public IEnumerable<string> Compile() => 
            _namespace is null ? CompileGlobalNamespace() 
                               : CompileWithNamespace();

        public IEnumerable<string> CompileGlobalNamespace()
        {
            IEnumerable<string>[] items =
            {
                CompileCommon(),
                CompileUsings(),
                CompileSourceElements(),
            };

            return items.SelectMany(identity => identity);

        }

        private IEnumerable<string> CompileWithNamespace()
        {
            IEnumerable<string>[] items =
            {
                CompileCommon(),
                CompileUsings(),
                CompileNamespace(CompileSourceElements()),
            };

            return items.SelectMany(identity => identity);
        }

        private static IEnumerable<string> CompileCommon()
        {
            yield return "// Auto-generated code";
        }

        private IEnumerable<string> CompileUsings() =>
            _usings.Select(ToUsingStr);

        private static string ToUsingStr(string @using) =>
            $"using {@using};";

        private IEnumerable<string> CompileNamespace(params IEnumerable<string>[] innerBlocks)
        {
            yield return $"namespace {_namespace}"; 
            foreach (string line in SourceBuilderHelper.WithScope(innerBlocks)) 
                yield return line;
        }

        private IEnumerable<string> CompileSourceElements() =>
            _sourceElements.SelectMany(element => element.Compile());
    }
}