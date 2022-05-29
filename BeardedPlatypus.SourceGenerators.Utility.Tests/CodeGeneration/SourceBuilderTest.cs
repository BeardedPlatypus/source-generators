using BeardedPlatypus.SourceGenerators.Utility.CodeGeneration;
using Microsoft.CodeAnalysis;
using NSubstitute;
using NUnit.Framework;
using Substitute = NSubstitute.Substitute;

namespace BeardedPlatypus.SourceGenerators.Utility.Tests.CodeGeneration;

[TestFixture]
public class SourceBuilderTest
{
    private ISourceBuilder SourceBuilder { get; } = new SourceBuilder();

    [Test]
    public void EmptySourceBuilder_ExpectedResults()
    {
        var result = SourceBuilder.Compile();

        string[] expectedResult =
        {
            "// Auto-generated code",
        };

        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [Test]
    public void WithUsing_StringNull_ArgumentNullException()
    {
        void Call() => SourceBuilder.WithUsing(null as string);
        Assert.That(Call, Throws.ArgumentNullException);
    }

    [Test]
    public void WithUsing_String_ExpectedResults()
    {
        const string @namespace = "Microsoft.CodeAnalysis";
        var sourceBuilder = SourceBuilder.WithUsing(@namespace);
        var result = sourceBuilder.Compile();

        string[] expectedResult =
        {
            "// Auto-generated code",
            $"using {@namespace};"
        };

        Assert.That(result, Is.EqualTo(expectedResult));
        Assert.That(sourceBuilder, Is.Not.SameAs(SourceBuilder));
    }

    [Test]
    public void WithUsing_SymbolNull_ArgumentNullException()
    {
        void Call() => SourceBuilder.WithUsing(null as INamespaceSymbol);
        Assert.That(Call, Throws.ArgumentNullException);
    }

    [Test]
    public void WithUsing_Symbol_ExpectedResults()
    {
        const string @namespace = "Microsoft.CodeAnalysis";
        var symbol = Substitute.For<INamespaceSymbol>();
        symbol.ToDisplayString().Returns(@namespace);

        var sourceBuilder = SourceBuilder.WithUsing(symbol);
        var result = sourceBuilder.Compile();

        string[] expectedResult =
        {
            "// Auto-generated code",
            $"using {@namespace};"
        };

        Assert.That(result, Is.EqualTo(expectedResult));
        Assert.That(sourceBuilder, Is.Not.SameAs(SourceBuilder));
    }


    private static IEnumerable<TestCaseData> WithUsingsStringNullData()
    {
        yield return new TestCaseData(null);
        yield return new TestCaseData(new[] {new string?[] { "a", null }});
    }

    [Test]
    [TestCaseSource(nameof(WithUsingsStringNullData))]
    public void WithUsings_StringNull_ThrowsArgumentNullException(string?[] usings)
    {
        void Call() => SourceBuilder.WithUsings(usings);
        Assert.That(Call, Throws.ArgumentNullException);
    }

    [Test]
    public void WithUsings_String_ExpectedResults()
    {
        const string namespace1 = "Microsoft.CodeAnalysis";
        const string namespace2 = "NUnit.Framework";
        var sourceBuilder = SourceBuilder.WithUsings(namespace1, namespace2);
        var result = sourceBuilder.Compile();

        string[] expectedResult =
        {
            "// Auto-generated code",
            $"using {namespace1};",
            $"using {namespace2};",
        };

        Assert.That(result, Is.EqualTo(expectedResult));
        Assert.That(sourceBuilder, Is.Not.SameAs(SourceBuilder));
    }

    private static IEnumerable<TestCaseData> WithUsingsSymbolNullData()
    {
        yield return new TestCaseData(null);
        yield return new TestCaseData(new[] { new INamespaceSymbol?[] { Substitute.For<INamespaceSymbol>(), null }});
    }

    [Test]
    [TestCaseSource(nameof(WithUsingsSymbolNullData))]
    public void WithUsings_SymbolNull_throwsArgumentNullException(INamespaceSymbol?[] usings) 
    {
        void Call() => SourceBuilder.WithUsings(usings);
        Assert.That(Call, Throws.ArgumentNullException);
    }

    [Test]
    public void WithUsings_Symbol_ExpectedResults()
    {
        const string namespace1 = "Microsoft.CodeAnalysis";
        var symbol1 = Substitute.For<INamespaceSymbol>();
        symbol1.ToDisplayString().Returns(namespace1);

        const string namespace2 = "NUnit.Framework";
        var symbol2 = Substitute.For<INamespaceSymbol>();
        symbol2.ToDisplayString().Returns(namespace2);

        var sourceBuilder = SourceBuilder.WithUsings(symbol1, symbol2);
        var result = sourceBuilder.Compile();

        string[] expectedResult =
        {
            "// Auto-generated code",
            $"using {namespace1};",
            $"using {namespace2};",
        };

        Assert.That(result, Is.EqualTo(expectedResult));
        Assert.That(sourceBuilder, Is.Not.SameAs(SourceBuilder));
    }

    [Test]
    public void WithNamespace_StringParameterNull_ThrowsArgumentNullException()
    {
        void Call() => SourceBuilder.WithNamespace(null as string);
        Assert.That(Call, Throws.ArgumentNullException);
    }

    [Test]
    public void WithNamespace_String_ExpectedResults()
    {
        const string @namespace = "Microsoft.CodeAnalysis";

        var sourceBuilder = SourceBuilder.WithNamespace(@namespace);
        var result = sourceBuilder.Compile();

        string[] expectedResult =
        {
            "// Auto-generated code",
            $"namespace {@namespace}",
            "{",
            "}",
        };

        Assert.That(result, Is.EqualTo(expectedResult));
        Assert.That(sourceBuilder, Is.Not.SameAs(SourceBuilder));
    }

    [Test]
    public void WithNamespace_SymbolParameterNull_ThrowsArgumentNullException()
    {
        void Call() => SourceBuilder.WithNamespace(null as INamespaceSymbol);
        Assert.That(Call, Throws.ArgumentNullException);
    }

    [Test]
    public void WithNamespace_Symbol_ExpectedResults()
    {
        const string @namespace = "Microsoft.CodeAnalysis";
        var symbol = Substitute.For<INamespaceSymbol>();
        symbol.ToDisplayString().Returns(@namespace);

        var sourceBuilder = SourceBuilder.WithNamespace(symbol);
        var result = sourceBuilder.Compile();

        string[] expectedResult =
        {
            "// Auto-generated code",
            $"namespace {@namespace}",
            "{",
            "}",
        };

        Assert.That(result, Is.EqualTo(expectedResult));
        Assert.That(sourceBuilder, Is.Not.SameAs(SourceBuilder));
    }

    [Test]
    public void WithSourceElement_ParamNull_ThrowsArgumentNullException()
    {
        void Call() => SourceBuilder.WithSourceElement(null);
        Assert.That(Call, Throws.ArgumentNullException);
    }

    [Test]
    public void WithSourceElement_GlobalNamespace_ExpectedResults()
    {
        const string line1 = "// This is a placeholder result.";
        var element1 = Substitute.For<ISourceElementBuilder>();
        element1.Compile().Returns(new[] { line1 });

        const string line2 = "// This is a placeholder result.";
        var element2 = Substitute.For<ISourceElementBuilder>();
        element2.Compile().Returns(new[] { line2 });

        var sourceBuilder = SourceBuilder.WithSourceElement(element1)
            .WithSourceElement(element2);
        var result = sourceBuilder.Compile();

        // TODO: update this with spacing.
        string[] expectedResult =
        {
            "// Auto-generated code",
            line1,
            line2,
        };

        Assert.That(result, Is.EqualTo(expectedResult));
        Assert.That(sourceBuilder, Is.Not.SameAs(SourceBuilder));
    }

    [Test]
    public void WithSourceElement_LocalNamespace_ExpectedResults()
    {
        const string @namespace = "Microsoft.CodeAnalysis";
        var symbol = Substitute.For<INamespaceSymbol>();
        symbol.ToDisplayString().Returns(@namespace);

        const string line = "// This is a placeholder result.";
        var element = Substitute.For<ISourceElementBuilder>();
        element.Compile().Returns(new[] { line });

        var sourceBuilder = SourceBuilder.WithNamespace(symbol)
                                         .WithSourceElement(element);
        var result = sourceBuilder.Compile();

        string[] expectedResult =
        {
            "// Auto-generated code",
            $"namespace {@namespace}",
            "{",
            $"    {line}",
            "}",
        };

        Assert.That(result, Is.EqualTo(expectedResult));
        Assert.That(sourceBuilder, Is.Not.SameAs(SourceBuilder));
    }
}