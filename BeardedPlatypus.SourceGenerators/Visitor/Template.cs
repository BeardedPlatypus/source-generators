using System;
using System.Collections.Generic;
using System.Linq;

namespace BeardedPlatypus.SourceGenerators.Visitor
{
    /// <summary>
    /// <see cref="Visitor.Template"/> defines the string template functions to
    /// generate the appropriate source code in the visitor source generator.
    /// </summary>
    internal static class Template
    {
        /// <summary>
        /// Generate the source code for the extension to the visitable interface.
        /// </summary>
        /// <param name="accessModifier">
        /// The access modifier of the visitable interface.
        /// </param>
        /// <param name="interfaceName">
        /// The name of the interface which is being extended.
        /// </param>
        /// <param name="namespaceName">
        /// The name of the namespace of which the interface is part.
        /// </param>
        /// <param name="visitorName">
        /// The name of the visitor interface.
        /// </param>
        /// <returns>
        /// The source code as a string specifying the extension to visitable
        /// interface.
        /// </returns>
        /// <remarks>
        /// All parameters are assumed to be valid non-null strings.
        /// </remarks>
        internal static string VisitableInterfaceExtension(string accessModifier,
                                                           string interfaceName,
                                                           string namespaceName,
                                                           string visitorName) =>
        $@"// Auto-generated code
namespace {namespaceName}
{{
    {accessModifier} partial interface {interfaceName}
    {{
        /// <summary>
        /// Accept the specified <paramref name=""visitor"" />.
        /// </summary>
        /// <param name=""visitor"">The visitor which visits this <see cref=""{interfaceName}""/>.</param>
        void Accept({visitorName} visitor);
    }}
}}";

        /// <summary>
        /// Generate the name of the extension file.
        /// </summary>
        /// <param name="name">The name of the construct being extended.</param>
        /// <returns>
        /// A string with the name of the extension file.
        /// </returns>
        /// <remarks>
        /// All parameters are assumed to be valid non-null strings.
        /// </remarks>
        internal static string VisitableExtensionFileName(string name) =>
            $"{name}.Visitable.cs";

        /// <summary>
        /// Generate the source code for the visitor interface.
        /// </summary>
        /// <param name="accessModifier">
        /// The access modifier of the interface this visitor visits.
        /// </param>
        /// <param name="visitorName">
        /// The name of the visitor interface.
        /// </param>
        /// <param name="interfaceName">
        /// The name of the interface for which this visitor is defined.
        /// </param>
        /// <param name="namespaceName">
        /// The name of the namespace of which the interface is part.
        /// </param>
        /// <param name="classes">
        /// The set of classes which implement the visitable interface.
        /// </param>
        /// <returns>
        /// The source code as a string of the visitor interface.
        /// </returns>
        internal static string VisitorInterface(string accessModifier,
                                                string visitorName,
                                                string interfaceName,
                                                string namespaceName,
                                                IEnumerable<string> classes)
        {
            string ToClassString(string className) =>
                $@"
        /// <summary>
        /// Receive the specified <paramref name=""element"" />.
        /// </summary>
        /// <param name=""element"">The element to act upon.</param>
        void Receive({className} element);";



            return $@"// Auto-generated code
namespace {namespaceName}
{{
    /// <summary>
    /// <see cref=""{visitorName}""/> defines the visitor interface to visit the 
    /// implementations of the <see cref=""{interfaceName}""/>.
    /// </summary>
    {accessModifier} interface {visitorName}
    {{{string.Join("\r\n", classes.Select(ToClassString))}
    }}
}}";
        }

        /// <summary>
        /// Generate the visitor interface file name.
        /// </summary>
        /// <param name="visitorName">The name of the interface.</param>
        /// <returns>The file name of the visitor interface.</returns>
        internal static string VisitorInterfaceFileName(string visitorName) =>
            $"{visitorName}.cs";

        /// <summary>
        /// Generate the extension source code for classes that implement a visitable
        /// interface.
        /// </summary>
        /// <param name="accessModifierClass">
        /// The access modifier of the class.
        /// </param>
        /// <param name="className">
        /// The name of the class.
        /// </param>
        /// <param name="visitors">
        /// The visitors that should be implemented, structured as:
        /// access modifier | visitor name | is abstract.
        /// </param>
        /// <param name="namespaceName">
        /// The namespace the class belongs to.
        /// </param>
        /// <returns>
        /// The source code as a string of the extension of a class that implements a
        /// visitable interface..
        /// </returns>
        /// <remarks>
        /// The visitor names are expected to be distinct.
        /// </remarks>
        internal static string VisitableClassExtension(string accessModifierClass,
                                                       string className,
                                                       string namespaceName,
                                                       IEnumerable<Tuple<string, string, bool>> visitors)
        {
            string ToAcceptMethod(Tuple<string, string, bool> visitor) =>
                visitor.Item3 
                    ? $"        {visitor.Item1} abstract void Accept({visitor.Item2} visitor);"
                    : $"        {visitor.Item1} void Accept({visitor.Item2} visitor) => visitor.Receive(this);";

            return $@"// Auto-generated code
namespace {namespaceName}
{{
    {accessModifierClass} partial class {className}
    {{
{string.Join("\r\n\r\n", visitors.Select(ToAcceptMethod))}
    }}
}}";
        }
    }
}