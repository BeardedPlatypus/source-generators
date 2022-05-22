using System.Collections.Generic;
using NUnit.Framework;

namespace BeardedPlatypus.SourceGenerators.Samples.Visitor;

[TestFixture]
public class SampleElementVisitorTest
{
    [Test]
    public void Constructor_ExpectedResults()
    {
        var visitor = new SampleElementVisitor();
        Assert.That(visitor.ElementTypeName, Is.Null);
    }

    private static IEnumerable<TestCaseData> VisitTestCaseData()
    {
        yield return new TestCaseData(new ElementA(), nameof(ElementA));
        yield return new TestCaseData(new ElementB(), nameof(ElementB));
        yield return new TestCaseData(new ElementC(), nameof(ElementC));
    }

    [Test]
    [TestCaseSource(nameof(VisitTestCaseData))]
    public void Visit_ExpectedResults(IElement element, string expectedTypeName)
    {
        var visitor = new SampleElementVisitor();
        
        element.Accept(visitor);

        Assert.That(visitor.ElementTypeName, Is.EqualTo(expectedTypeName));
    }
    
}