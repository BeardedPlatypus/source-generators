using BeardedPlatypus.SourceGenerators.Visitor;
using NUnit.Framework;

namespace BeardedPlatypus.SourceGenerators.Tests.Visitor;

[TestFixture]
public class VisitableAttributeTest
{
    [Test]
    public void Constructor_WithVisitorName_CorrectResult()
    {
        const string visitorName = "visitorName";
        var attribute = new VisitableAttribute(visitorName);

        Assert.That(attribute, Is.InstanceOf<System.Attribute>());
        Assert.That(attribute.VisitorInterfaceName, Is.EqualTo(visitorName));
    }

    [Test]
    public void Constructor_WithoutVisitorName_CorrectResult()
    {
        var attribute = new VisitableAttribute();

        Assert.That(attribute, Is.InstanceOf<System.Attribute>());
        Assert.That(attribute.VisitorInterfaceName, Is.Null);
    }
}