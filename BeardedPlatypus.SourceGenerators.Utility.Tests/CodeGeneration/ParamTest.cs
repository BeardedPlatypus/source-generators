using BeardedPlatypus.SourceGenerators.Utility.CodeGeneration;
using Microsoft.CodeAnalysis;
using NSubstitute;
using NUnit.Framework;

namespace BeardedPlatypus.SourceGenerators.Utility.Tests.CodeGeneration;

[TestFixture]
public class ParamTest
{
    [Test]
    public void Constructor_ExpectedResults()
    {
        const string paramName = "testData";
        const string paramTypeName = nameof(TestCaseData);
        const string paramDoc = "Some test-data.";

        var param = new Param(paramName, paramTypeName, paramDoc);

        Assert.That(param.TypeName, Is.EqualTo(paramTypeName));
        Assert.That(param.Name, Is.EqualTo(paramName));
        Assert.That(param.Doc, Is.EqualTo(paramDoc));
    }

    [Test]
    public void Constructor_NameNull_ThrowsArgumentNullException()
    {
        void Call() => new Param(null, nameof(TestCaseData), null);
        Assert.That(Call, Throws.ArgumentNullException
                                       .With.Property(nameof(ArgumentNullException.ParamName))
                                       .EqualTo("name"));
    }

    [Test]
    public void Constructor_TypeNameNull_ThrowsArgumentNullException()
    {
        void Call() => new Param("testData", null, null);
        Assert.That(Call, Throws.ArgumentNullException
                                       .With.Property(nameof(ArgumentNullException.ParamName))
                                       .EqualTo("typeName"));
    }

    [Test]
    public void Constructor_DocNull_ExpectedResults()
    {
        var param = new Param("testData", nameof(TestCaseData), null);

        Assert.That(param.Doc, Is.Null);
    }

    [Test]
    [TestCase("<param name=\"name\">Some test-data</param>", "Some test-data")]
    [TestCase("<param name=\"name\" >Some test-data</param >", "Some test-data")]
    public void Constructor_ParamDocWithXml_IsStrippedOfXmlTags(string paramDoc, string expectedDocValue)
    {
        var param = new Param("name", nameof(TestCaseData), paramDoc);

        Assert.That(param.Doc, Is.EqualTo(expectedDocValue));
    }

    private static IEnumerable<TestCaseData> NamedTypeSymbolConstructorData()
    {
        string name = nameof(TestCaseData);
        string qualifiedName = typeof(TestCaseData).FullName;

        var symbol = Substitute.For<INamedTypeSymbol>();
        symbol.Name.Returns(name);
        symbol.ToDisplayString().Returns(qualifiedName);

        yield return new TestCaseData(symbol, true, qualifiedName).SetName("With qualified name.");
        yield return new TestCaseData(symbol, false, name).SetName("Without qualified name.");
    }

    [Test]
    [TestCaseSource(nameof(NamedTypeSymbolConstructorData))]
    public void Constructor_TypeNamedTypeSymbol_ExpectedResults(INamedTypeSymbol typeSymbol, bool isFullyQualified, string expectedResult)
    {
        var param = new Param("name", typeSymbol, isFullyQualified, "Some test-data");
        Assert.That(param.TypeName, Is.EqualTo(expectedResult));
    }

    private static IEnumerable<TestCaseData> ParameterSymbolConstructorData()
    {
        const string name = "testData";
        const string docStr = "some test-data.";
        const string docXml = $"<param name=\"{name}\">{docStr}</param>";

        var symbol = Substitute.For<IParameterSymbol>();
        symbol.Name.Returns(name);
        symbol.GetDocumentationCommentXml()
              .Returns(docXml);

        const string typeName = nameof(TestCaseData);
        string? qualifiedTypeName = typeof(TestCaseData).FullName;
        var typeSymbol = Substitute.For<ITypeSymbol>();
        typeSymbol.Name.Returns(typeName);
        typeSymbol.ToDisplayString().Returns(qualifiedTypeName);

        symbol.Type.Returns(typeSymbol);

        yield return new TestCaseData(symbol, true, name, qualifiedTypeName, docStr);
        yield return new TestCaseData(symbol, false, name, typeName, docStr);
    }

    [Test]
    [TestCaseSource(nameof(ParameterSymbolConstructorData))]
    public void Constructor_FromParameterSymbol_ExpectedResults(IParameterSymbol parameterSymbol, 
                                                                bool isFullyQualified,
                                                                string expectedName,
                                                                string expectedTypeName,
                                                                string expectedDocs)
    {
        var param = new Param(parameterSymbol, isFullyQualified);

        Assert.That(param.Name, Is.EqualTo(expectedName));
        Assert.That(param.TypeName, Is.EqualTo(expectedTypeName));
        Assert.That(param.Doc, Is.EqualTo(expectedDocs));
    }

    private static IEnumerable<TestCaseData> GenerateDocStringData()
    {
        const string name = "someName";
        const string docStr = "some test-data.";
        const string docXml = $"<param name=\"{name}\">{docStr}</param>";

        yield return new TestCaseData(name, null, null)
            .SetName("No documentation.");
        yield return new TestCaseData(name, docStr, docXml)
            .SetName("With doc string.");
        yield return new TestCaseData(name, docXml, docXml)
            .SetName("With doc xml.");
    }

    [Test]
    [TestCaseSource(nameof(GenerateDocStringData))]
    public void GenerateDocString_ExpectedResults(string name, string doc, string expectedResult)
    {
        var param = new Param(name, nameof(TestCaseData), doc);

        var result = param.GenerateDocString();

        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [Test]
    public void GenerateParamString_ExpectedResults()
    {
        const string paramName = "testData";
        const string paramTypeName = nameof(TestCaseData);

        var param = new Param(paramName, paramTypeName, null);

        var result = param.GenerateParamString();

        const string expectedResult = $"{paramTypeName} {paramName}";
        Assert.That(result, Is.EqualTo(expectedResult));
    }
}