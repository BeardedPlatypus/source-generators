using BeardedPlatypus.SourceGenerators.Utility.CodeGeneration;
using NUnit.Framework;

namespace BeardedPlatypus.SourceGenerators.Utility.Tests.CodeGeneration;

[TestFixture]
public class AccessModifierExtensionsTest
{
    [Test]
    [TestCase(AccessModifier.Public, "public", TestName = "public")]
    [TestCase(AccessModifier.Internal, "internal", TestName = "internal")]
    [TestCase(AccessModifier.Protected, "protected", TestName = "protected")]
    [TestCase(AccessModifier.Private, "private", TestName = "private")]
    public void ToString_ExpectedResults(AccessModifier modifier, string expectedResult)
    {
        var result = modifier.ToSyntaxString();
        Assert.That(result, Is.EqualTo(expectedResult));
    }
}