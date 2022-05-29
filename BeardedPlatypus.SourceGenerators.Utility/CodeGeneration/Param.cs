using System;
using BeardedPlatypus.SourceGenerators.Utility.Internal;
using Microsoft.CodeAnalysis;

namespace BeardedPlatypus.SourceGenerators.Utility.CodeGeneration
{
    /// <summary>
    /// <see cref="Param"/> describes a single parameter used
    /// in the generation of code.
    /// </summary>
    public sealed class Param
    {
        // TODO: Add additional validation here?
        /// <summary>
        /// Creates a new <see cref="Param"/> with the given parameters.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="typeName">The type of the parameter.</param>
        /// <param name="doc">The doc-string of the parameter</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="name"/> or <paramref name="typeName"/> are <c>null</c>.
        /// </exception>
        public Param(string name, string typeName, string doc = null)
        {
            Name = name.EnsureNotNull(nameof(name));
            TypeName = typeName.EnsureNotNull(nameof(typeName));
            Doc = ExtractDocString(doc);
        }

        /// <summary>
        /// Creates a new <see cref="Param"/> with the given parameters.
        /// </summary>
        /// <param name="name">The name of this parameter.</param>
        /// <param name="typeSymbol">The type symbol specifying the type of this parameter.</param>
        /// <param name="isFullyQualified">Whether the type name should be fully qualified.</param>
        /// <param name="doc">The doc-string of the parameter.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="name"/> or <paramref name="typeSymbol"/> are <c>null</c>.
        /// </exception>
        public Param(string name, INamedTypeSymbol typeSymbol, bool isFullyQualified, string doc) :
            this(name, ToTypeName(typeSymbol, isFullyQualified), doc) { }

        /// <summary>
        /// Creates a new <see cref="Param"/> from the given <paramref name="parameterSymbol"/>.
        /// </summary>
        /// <param name="parameterSymbol">The parameter symbol.</param>
        /// <param name="isFullyQualified">Whether the parameter type should be fully qualified.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="parameterSymbol"/> is <c>null</c>.
        /// </exception>
        public Param(IParameterSymbol parameterSymbol, bool isFullyQualified = true) :
            this(parameterSymbol?.Name, 
                 ToTypeName(parameterSymbol?.Type, isFullyQualified),
                 parameterSymbol?.GetDocumentationCommentXml()) { }

        private static string ToTypeName(ISymbol symbol, bool isFullyQualified) =>
            isFullyQualified ? symbol?.ToDisplayString() : symbol?.Name;

        private static string ExtractDocString(string doc)
        {
            if (doc == null) return null;
            if (!doc.StartsWith("<param name=\"")) return doc;

            int docStrStart = doc.IndexOf(">", StringComparison.Ordinal) + 1;
            int docStrEnd = doc.LastIndexOf("</", StringComparison.Ordinal - 1);

            return doc.Substring(docStrStart, docStrEnd - docStrStart);
        }

        /// <summary>
        /// Gets the parameter name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the name of the type of the parameter.
        /// </summary>
        public string TypeName { get; }

        /// <summary>
        /// Gets the documentation of this parameter.
        /// </summary>
        public string Doc { get; }

        // TODO: Add options for wrapping the doc-string?
        /// <summary>
        /// Generate the doc-string of this <see cref="Param"/>.
        /// </summary>
        /// <returns>
        /// The doc-string of this <see cref="Param"/>.
        /// </returns>
        public string GenerateDocString() =>
            !(Doc is null) ? $"<param name=\"{Name}\">{Doc}</param>" : null;

        /// <summary>
        /// Generate the parameter string of this <see cref="Param"/>.
        /// </summary>
        /// <returns>
        /// The parameter string: "{TypeName} {Name}".
        /// </returns>
        public string GenerateParamString() =>
            $"{TypeName} {Name}";
    }
}