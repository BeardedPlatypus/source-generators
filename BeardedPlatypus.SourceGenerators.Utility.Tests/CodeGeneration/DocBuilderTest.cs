﻿using BeardedPlatypus.SourceGenerators.Utility.CodeGeneration;
using NUnit.Framework;

namespace BeardedPlatypus.SourceGenerators.Utility.Tests.CodeGeneration;

[TestFixture]
public class DocBuilderTest
{
    private IDocBuilder DocBuilder { get; set; }

    [SetUp]
    public void SetUp()
    {
        DocBuilder = new DocBuilder();
    }

    [Test]
    public void Empty_ExpectedResult()
    {
        IEnumerable<string> result = DocBuilder.Compile();

        string[] expectedResult =
        {
            "/// <summary>",
            "/// </summary>"
        };
        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [Test]
    public void AddSummary_Null_ThrowsArgumentNullException()
    {
        const string? nullStr = null;
        void Call() => DocBuilder.AddSummary(nullStr);
        Assert.That(Call, Throws.ArgumentNullException);
    }

    [Test]
    public void WithSummary_Null_ThrowsArgumentNullException()
    {
        const string? nullStr = null;
        void Call() => DocBuilder.WithSummary(nullStr);
        Assert.That(Call, Throws.ArgumentNullException);
    }

    private static IEnumerable<TestCaseData> AddSummaryData()
    {
        const string singleLine = "This is a summary.";

        string[] expectedResultSingleLine =
        {
             "/// <summary>", 
            $"/// {singleLine}", 
             "/// </summary>"
        };

        yield return new TestCaseData(singleLine, expectedResultSingleLine).SetName("Single line");

        const string line1 = "This is line 1.";
        const string line2 = "This is line 2.";
        const string line3 = "This is line 3.";

        const string multiLine = $@"{line1}
{line2}
{line3}";

        string[] expectedResultMultiLine =
        {
             "/// <summary>",
            $"/// {line1}",
            $"/// {line2}",
            $"/// {line3}",
             "/// </summary>"
        };

        yield return new TestCaseData(multiLine, expectedResultMultiLine).SetName("Multiple lines");
    }

    [Test]
    [TestCaseSource(nameof(AddSummaryData))]
    public void AddSummary_ExpectedResult(string summary, string[] expectedResult)
    {
        
        DocBuilder.AddSummary(summary);
        IEnumerable<string> result = DocBuilder.Compile();

        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [Test]
    [TestCaseSource(nameof(AddSummaryData))]
    public void WithSummary_ExpectedResult(string summary, string[] expectedResult)
    {
        
        var docBuilder = DocBuilder.WithSummary(summary);
        IEnumerable<string> result = DocBuilder.Compile();

        Assert.That(result, Is.EqualTo(expectedResult));
        Assert.That(docBuilder, Is.SameAs(DocBuilder));
    }

    [Test]
    public void AddSummary_Params_ExpectedResult()
    {
        const string line1 = "This is line 1.";
        const string line2 = "This is line 2.";
        const string line3 = "This is line 3.";

        DocBuilder.AddSummary(
            line1,
            line2,
            line3);
        var result = DocBuilder.Compile();

        string[] expectedResult =
        {
             "/// <summary>",
            $"/// {line1}",
            $"/// {line2}",
            $"/// {line3}",
             "/// </summary>"
        };
        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [Test]
    public void WithSummary_Params_ExpectedResult()
    {
        const string line1 = "This is line 1.";
        const string line2 = "This is line 2.";
        const string line3 = "This is line 3.";

        var docBuilder = DocBuilder.WithSummary(
            line1,
            line2,
            line3);
        var result = DocBuilder.Compile();

        string[] expectedResult =
        {
             "/// <summary>",
            $"/// {line1}",
            $"/// {line2}",
            $"/// {line3}",
             "/// </summary>"
        };
        Assert.That(result, Is.EqualTo(expectedResult));
        Assert.That(docBuilder, Is.SameAs(DocBuilder));
    }

    private static IEnumerable<TestCaseData> AddParamNullData()
    {
        yield return new TestCaseData("name", null).SetName("docStr null");
        yield return new TestCaseData(null, "docStr").SetName("name null");
        yield return new TestCaseData(null, null).SetName("both null");
    }

    [Test]
    [TestCaseSource(nameof(AddParamNullData))]
    public void AddParam_ParameterNull_ThrowsArgumentNullException(string name,
                                                                   string docStr)
    {
        void Call() => DocBuilder.AddParam(name, docStr);
        Assert.That(Call, Throws.ArgumentNullException);
    }

    [Test]
    [TestCaseSource(nameof(AddParamNullData))]
    public void WithParam_ParameterNull_ThrowsArgumentNullException(string name,
                                                                    string docStr)
    {
        void Call() => DocBuilder.WithParam(name, docStr);
        Assert.That(Call, Throws.ArgumentNullException);
    }

    [Test]
    public void AddParam_ExpectedResults()
    {
        var param1 = (Name: "param1", DocStr: "Description 1.");
        var param2 = (Name: "param2", DocStr: "Description 2.");

        DocBuilder.AddParam(param1.Name, param1.DocStr);
        DocBuilder.AddParam(param2.Name, param2.DocStr);
        IEnumerable<string> result = DocBuilder.Compile();

        string[] expectedResult =
        {
             "/// <summary>",
             "/// </summary>",
            $"/// <param name=\"{param1.Name}\">{param1.DocStr}</param>",
            $"/// <param name=\"{param2.Name}\">{param2.DocStr}</param>",
        };

        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [Test]
    public void WithParam_ExpectedResults()
    {
        var param1 = (Name: "param1", DocStr: "Description 1.");
        var param2 = (Name: "param2", DocStr: "Description 2.");

        var docBuilder = DocBuilder.WithParam(param1.Name, param1.DocStr).
                                    WithParam(param2.Name, param2.DocStr);
        IEnumerable<string> result = DocBuilder.Compile();

        string[] expectedResult =
        {
             "/// <summary>",
             "/// </summary>",
            $"/// <param name=\"{param1.Name}\">{param1.DocStr}</param>",
            $"/// <param name=\"{param2.Name}\">{param2.DocStr}</param>",
        };

        Assert.That(result, Is.EqualTo(expectedResult));
        Assert.That(docBuilder, Is.SameAs(DocBuilder));
    }

    [Test]
    public void AddParams_ParameterNull_ExpectedResults()
    {
        void Call() => DocBuilder.AddParams(null);
        Assert.That(Call, Throws.ArgumentNullException);
    }

    [Test]
    public void WithParams_ParameterNull_ExpectedResults()
    {
        void Call() => DocBuilder.WithParams(null);
        Assert.That(Call, Throws.ArgumentNullException);
    }

    [Test]
    public void AddParams_ExpectedResults()
    {
        var param1 = (Name: "param1", DocStr: "Description 1.");
        var param2 = (Name: "param2", DocStr: "Description 2.");

        DocBuilder.AddParams(param1, param2);
        IEnumerable<string> result = DocBuilder.Compile();

        string[] expectedResult =
        {
             "/// <summary>",
             "/// </summary>",
            $"/// <param name=\"{param1.Name}\">{param1.DocStr}</param>",
            $"/// <param name=\"{param2.Name}\">{param2.DocStr}</param>",
        };

        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [Test]
    public void WithParams_ExpectedResults()
    {
        var param1 = (Name: "param1", DocStr: "Description 1.");
        var param2 = (Name: "param2", DocStr: "Description 2.");

        var docBuilder = DocBuilder.WithParams(param1, param2);
        IEnumerable<string> result = DocBuilder.Compile();

        string[] expectedResult =
        {
             "/// <summary>",
             "/// </summary>",
            $"/// <param name=\"{param1.Name}\">{param1.DocStr}</param>",
            $"/// <param name=\"{param2.Name}\">{param2.DocStr}</param>",
        };

        Assert.That(result, Is.EqualTo(expectedResult));
        Assert.That(docBuilder, Is.SameAs(DocBuilder));
    }

    [Test]
    [TestCaseSource(nameof(AddParamNullData))]
    public void AddTypeParam_ParameterNull_ThrowsArgumentNullException(string name,
                                                                       string docString)
    {
        void Call() => DocBuilder.AddTypeParam(name, docString);
        Assert.That(Call, Throws.ArgumentNullException);
    }

    [Test]
    [TestCaseSource(nameof(AddParamNullData))]
    public void WithTypeParam_ParameterNull_ThrowsArgumentNullException(string name,
                                                                        string docString)
    {
        void Call() => DocBuilder.WithTypeParam(name, docString);
        Assert.That(Call, Throws.ArgumentNullException);
    }

    [Test]
    public void AddTypeParam_ExpectedResults()
    {
        var param1 = (Name: "param1", DocStr: "Description 1.");
        var param2 = (Name: "param2", DocStr: "Description 2.");

        DocBuilder.AddTypeParam(param1.Name, param1.DocStr);
        DocBuilder.AddTypeParam(param2.Name, param2.DocStr);
        IEnumerable<string> result = DocBuilder.Compile();

        string[] expectedResult =
        {
             "/// <summary>",
             "/// </summary>",
            $"/// <typeparam name=\"{param1.Name}\">{param1.DocStr}</typeparam>",
            $"/// <typeparam name=\"{param2.Name}\">{param2.DocStr}</typeparam>",
        };

        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [Test]
    public void WithTypeParam_ExpectedResults()
    {
        var param1 = (Name: "param1", DocStr: "Description 1.");
        var param2 = (Name: "param2", DocStr: "Description 2.");

        var docBuilder =  DocBuilder.WithTypeParam(param1.Name, param1.DocStr)
                                    .WithTypeParam(param2.Name, param2.DocStr);
        IEnumerable<string> result = DocBuilder.Compile();

        string[] expectedResult =
        {
             "/// <summary>",
             "/// </summary>",
            $"/// <typeparam name=\"{param1.Name}\">{param1.DocStr}</typeparam>",
            $"/// <typeparam name=\"{param2.Name}\">{param2.DocStr}</typeparam>",
        };

        Assert.That(result, Is.EqualTo(expectedResult));
        Assert.That(docBuilder, Is.SameAs(docBuilder));
    }

    [Test]
    public void AddTypeParams_ParameterNull_ExpectedResults()
    {
        void Call() => DocBuilder.AddTypeParams(null);
        Assert.That(Call, Throws.ArgumentNullException);
    }

    [Test]
    public void WithTypeParams_ParameterNull_ExpectedResults()
    {
        void Call() => DocBuilder.WithTypeParams(null);
        Assert.That(Call, Throws.ArgumentNullException);
    }

    [Test]
    public void AddTypeParams_ExpectedResults()
    {
        var param1 = (Name: "param1", DocStr: "Description 1.");
        var param2 = (Name: "param2", DocStr: "Description 2.");

        DocBuilder.AddTypeParams(param1, param2);
        IEnumerable<string> result = DocBuilder.Compile();

        string[] expectedResult =
        {
             "/// <summary>",
             "/// </summary>",
            $"/// <typeparam name=\"{param1.Name}\">{param1.DocStr}</typeparam>",
            $"/// <typeparam name=\"{param2.Name}\">{param2.DocStr}</typeparam>",
        };

        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [Test]
    public void WithTypeParams_ExpectedResults()
    {
        var param1 = (Name: "param1", DocStr: "Description 1.");
        var param2 = (Name: "param2", DocStr: "Description 2.");

        var docBuilder = DocBuilder.WithTypeParams(param1, param2);
        IEnumerable<string> result = DocBuilder.Compile();

        string[] expectedResult =
        {
             "/// <summary>",
             "/// </summary>",
            $"/// <typeparam name=\"{param1.Name}\">{param1.DocStr}</typeparam>",
            $"/// <typeparam name=\"{param2.Name}\">{param2.DocStr}</typeparam>",
        };

        Assert.That(result, Is.EqualTo(expectedResult));
        Assert.That(docBuilder, Is.SameAs(DocBuilder));
    }

    [Test]
    public void AddReturns_ReturnsNull_ThrowsArgumentNull()
    {
        void Call() => DocBuilder.AddReturns(null as string);
        Assert.That(Call, Throws.ArgumentNullException);
    }

    private static IEnumerable<TestCaseData> AddReturnsData()
    {
        const string singleLine = "This is a summary.";

        string[] expectedResultSingleLine =
        {
             "/// <summary>",
             "/// </summary>",
             "/// <returns>", 
            $"/// {singleLine}", 
             "/// </returns>"
        };

        yield return new TestCaseData(singleLine, expectedResultSingleLine).SetName("Single line");

        const string line1 = "This is line 1.";
        const string line2 = "This is line 2.";
        const string line3 = "This is line 3.";

        const string multiLine = $@"{line1}
{line2}
{line3}";

        string[] expectedResultMultiLine =
        {
             "/// <summary>",
             "/// </summary>",
             "/// <returns>",
            $"/// {line1}",
            $"/// {line2}",
            $"/// {line3}",
             "/// </returns>",
        };

        yield return new TestCaseData(multiLine, expectedResultMultiLine).SetName("Multiple lines");
    }

    [Test]
    [TestCaseSource(nameof(AddReturnsData))]
    public void AddReturns_ExpectedResult(string summary, string[] expectedResult)
    {
        
        DocBuilder.AddReturns(summary);
        IEnumerable<string> result = DocBuilder.Compile();

        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [Test]
    [TestCaseSource(nameof(AddReturnsData))]
    public void WithReturns_ExpectedResult(string summary, string[] expectedResult)
    {
        
        var docBuilder = DocBuilder.WithReturns(summary);
        IEnumerable<string> result = DocBuilder.Compile();

        Assert.That(result, Is.EqualTo(expectedResult));
        Assert.That(docBuilder, Is.SameAs(DocBuilder));
    }

    [Test]
    public void AddReturns_Params_ExpectedResult()
    {
        const string line1 = "This is line 1.";
        const string line2 = "This is line 2.";
        const string line3 = "This is line 3.";

        DocBuilder.AddReturns(
            line1,
            line2,
            line3);
        var result = DocBuilder.Compile();

        string[] expectedResult =
        {
             "/// <summary>",
             "/// </summary>",
             "/// <returns>",
            $"/// {line1}",
            $"/// {line2}",
            $"/// {line3}",
             "/// </returns>"
        };
        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [Test]
    public void WithReturns_Params_ExpectedResult()
    {
        const string line1 = "This is line 1.";
        const string line2 = "This is line 2.";
        const string line3 = "This is line 3.";

        var docBuilder = DocBuilder.WithReturns(
            line1,
            line2,
            line3);
        var result = DocBuilder.Compile();

        string[] expectedResult =
        {
             "/// <summary>",
             "/// </summary>",
             "/// <returns>",
            $"/// {line1}",
            $"/// {line2}",
            $"/// {line3}",
             "/// </returns>"
        };
        Assert.That(result, Is.EqualTo(expectedResult));
        Assert.That(docBuilder, Is.SameAs(DocBuilder));
    }

    [Test]
    [TestCaseSource(nameof(AddParamNullData))]
    public void AddException_ParameterNull_ThrowsArgumentNullException(string name, 
                                                                       string docStr)
    {
        void Call() => DocBuilder.AddException(name, docStr);
        Assert.That(Call, Throws.ArgumentNullException);
    }

    [Test]
    [TestCaseSource(nameof(AddParamNullData))]
    public void WithException_ParameterNull_ThrowsArgumentNullException(string name,
                                                                        string docStr)
    {
        void Call() => DocBuilder.WithException(name, docStr);
        Assert.That(Call, Throws.ArgumentNullException);
    }

    [Test]
    public void AddException_ExpectedResults()
    {
        var param1 = (Name: "param1", DocStr: "Description 1.");
        var param2 = (Name: "param2", DocStr: "Description 2.");

        DocBuilder.AddException(param1.Name, param1.DocStr);
        DocBuilder.AddException(param2.Name, param2.DocStr);
        IEnumerable<string> result = DocBuilder.Compile();

        string[] expectedResult =
        {
             "/// <summary>",
             "/// </summary>",
            $"/// <exception cref=\"{param1.Name}\">",
            $"/// {param1.DocStr}",
            "/// </exception>",
            $"/// <exception cref=\"{param2.Name}\">",
            $"/// {param2.DocStr}",
            "/// </exception>",
        };

        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [Test]
    public void WithException_ExpectedResults()
    {
        var param1 = (Name: "param1", DocStr: "Description 1.");
        var param2 = (Name: "param2", DocStr: "Description 2.");

        var docBuilder = DocBuilder.WithException(param1.Name, param1.DocStr).
                                    WithException(param2.Name, param2.DocStr);
        IEnumerable<string> result = DocBuilder.Compile();

        string[] expectedResult =
        {
             "/// <summary>",
             "/// </summary>",
            $"/// <exception cref=\"{param1.Name}\">",
            $"/// {param1.DocStr}",
            "/// </exception>",
            $"/// <exception cref=\"{param2.Name}\">",
            $"/// {param2.DocStr}",
            "/// </exception>",
        };

        Assert.That(result, Is.EqualTo(expectedResult));
        Assert.That(docBuilder, Is.SameAs(DocBuilder));
    }

    [Test]
    public void AddExceptions_ParameterNull_ExpectedResults()
    {
        void Call() => DocBuilder.AddExceptions(null);
        Assert.That(Call, Throws.ArgumentNullException);
    }

    [Test]
    public void WithExceptions_ParameterNull_ExpectedResults()
    {
        void Call() => DocBuilder.WithExceptions(null);
        Assert.That(Call, Throws.ArgumentNullException);
    }

    [Test]
    public void AddExceptions_ExpectedResults()
    {
        var param1 = (Name: "param1", DocStr: "Description 1.");
        var param2 = (Name: "param2", DocStr: "Description 2.");

        DocBuilder.AddExceptions(param1, param2);
        IEnumerable<string> result = DocBuilder.Compile();

        string[] expectedResult =
        {
             "/// <summary>",
             "/// </summary>",
            $"/// <exception cref=\"{param1.Name}\">",
            $"/// {param1.DocStr}",
            "/// </exception>",
            $"/// <exception cref=\"{param2.Name}\">",
            $"/// {param2.DocStr}",
            "/// </exception>",
        };

        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [Test]
    public void WithExceptions_ExpectedResults()
    {
        var param1 = (Name: "param1", DocStr: "Description 1.");
        var param2 = (Name: "param2", DocStr: "Description 2.");

        var docBuilder = DocBuilder.WithExceptions(param1, param2);
        IEnumerable<string> result = DocBuilder.Compile();

        string[] expectedResult =
        {
             "/// <summary>",
             "/// </summary>",
            $"/// <exception cref=\"{param1.Name}\">",
            $"/// {param1.DocStr}",
            "/// </exception>",
            $"/// <exception cref=\"{param2.Name}\">",
            $"/// {param2.DocStr}",
            "/// </exception>",
        };

        Assert.That(result, Is.EqualTo(expectedResult));
        Assert.That(docBuilder, Is.SameAs(DocBuilder));
    }

    [Test]
    public void AddRemarks_ReturnsNull_ThrowsArgumentNull()
    {
        void Call() => DocBuilder.AddRemarks(null as string);
        Assert.That(Call, Throws.ArgumentNullException);
    }

    private static IEnumerable<TestCaseData> AddRemarksData()
    {
        const string singleLine = "This is a summary.";

        string[] expectedResultSingleLine =
        {
             "/// <summary>",
             "/// </summary>",
             "/// <remarks>", 
            $"/// {singleLine}", 
             "/// </remarks>"
        };

        yield return new TestCaseData(singleLine, expectedResultSingleLine).SetName("Single line");

        const string line1 = "This is line 1.";
        const string line2 = "This is line 2.";
        const string line3 = "This is line 3.";

        const string multiLine = $@"{line1}
{line2}
{line3}";

        string[] expectedResultMultiLine =
        {
             "/// <summary>",
             "/// </summary>",
             "/// <remarks>",
            $"/// {line1}",
            $"/// {line2}",
            $"/// {line3}",
             "/// </remarks>",
        };

        yield return new TestCaseData(multiLine, expectedResultMultiLine).SetName("Multiple lines");
    }

    [Test]
    [TestCaseSource(nameof(AddRemarksData))]
    public void AddRemarks_ExpectedResult(string summary, string[] expectedResult)
    {
        
        DocBuilder.AddRemarks(summary);
        IEnumerable<string> result = DocBuilder.Compile();

        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [Test]
    [TestCaseSource(nameof(AddRemarksData))]
    public void WithRemarks_ExpectedResult(string summary, string[] expectedResult)
    {
        
        var docBuilder = DocBuilder.WithRemarks(summary);
        IEnumerable<string> result = DocBuilder.Compile();

        Assert.That(result, Is.EqualTo(expectedResult));
        Assert.That(docBuilder, Is.SameAs(DocBuilder));
    }

    [Test]
    public void AddRemarks_Params_ExpectedResult()
    {
        const string line1 = "This is line 1.";
        const string line2 = "This is line 2.";
        const string line3 = "This is line 3.";

        DocBuilder.AddRemarks(
            line1,
            line2,
            line3);
        var result = DocBuilder.Compile();

        string[] expectedResult =
        {
             "/// <summary>",
             "/// </summary>",
             "/// <remarks>",
            $"/// {line1}",
            $"/// {line2}",
            $"/// {line3}",
             "/// </remarks>"
        };
        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [Test]
    public void WithRemarks_Params_ExpectedResult()
    {
        const string line1 = "This is line 1.";
        const string line2 = "This is line 2.";
        const string line3 = "This is line 3.";

        var docBuilder = DocBuilder.WithRemarks(
            line1,
            line2,
            line3);
        var result = DocBuilder.Compile();

        string[] expectedResult =
        {
             "/// <summary>",
             "/// </summary>",
             "/// <remarks>",
            $"/// {line1}",
            $"/// {line2}",
            $"/// {line3}",
             "/// </remarks>"
        };
        Assert.That(result, Is.EqualTo(expectedResult));
        Assert.That(docBuilder, Is.SameAs(DocBuilder));
    }
}