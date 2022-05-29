using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace BeardedPlatypus.SourceGenerators.Utility.CodeGeneration
{
    public interface ISourceBuilder
    {
        ISourceBuilder WithUsing(string @using);
        ISourceBuilder WithUsing(INamespaceSymbol @using);

        ISourceBuilder WithUsings(params string[] usings);
        ISourceBuilder WithUsings(params INamespaceSymbol[] usings);

        ISourceBuilder WithNamespace(string @namespace);
        ISourceBuilder WithNamespace(INamespaceSymbol @namespace);

        // TODO: Add alias support
        // void AddAlias(string aliasName, string originalName);
        // ISourceBuilder WithAlias(string aliasName, string originalName);
        // void AddAlias(string aliasName, INamedTypeSymbol symbol);
        // ISourceBuilder WithAlias(string aliasName, INamedTypeSymbol originalName);

        // TODO: Add a EnumBuilder
        ISourceBuilder WithSourceElement(ISourceElementBuilder sourceElementBuilder);

        IEnumerable<string> Compile();
    }
}