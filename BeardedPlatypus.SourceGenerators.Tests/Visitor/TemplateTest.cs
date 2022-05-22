using System;
using BeardedPlatypus.SourceGenerators.Visitor;
using NUnit.Framework;

namespace BeardedPlatypus.SourceGenerators.Tests.Visitor;

[TestFixture]
public class TemplateTest
{
    [Test]
    public void VisitableInterfaceExtension_ExpectedResult()
    {
        string? result = Template.VisitableInterfaceExtension(
            "public",
            "IElement",
            "BeardedPlatypus.SourceGenerators.Tests.Visitor",
            "IElementVisitor");

        const string expectedResult = 
            @"// Auto-generated code
namespace BeardedPlatypus.SourceGenerators.Tests.Visitor
{
    public partial interface IElement
    {
        /// <summary>
        /// Accept the specified <paramref name=""visitor"" />.
        /// </summary>
        /// <param name=""visitor"">The visitor which visits this <see cref=""IElement""/>.</param>
        void Accept(IElementVisitor visitor);
    }
}";

        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [Test]
    public void VisitableExtensionFileName_ExpectedResults()
    {
        const string name = "IElement";
        string? result = Template.VisitableExtensionFileName(name);
        Assert.That(result, Is.EqualTo($"{name}.Visitable.cs"));
    }

    [Test]
    public void VisitorInterface_ExpectedResults()
    {
        string[] classes =
        {
            "ElementA",
            "ElementB",
            "ElementC",
        };

        string? result = Template.VisitorInterface(
            "internal",
            "IElementVisitor",
            "IElement",
            "BeardedPlatypus.SourceGenerators.Tests.Visitor",
            classes);

        const string expectedResult = @"// Auto-generated code
namespace BeardedPlatypus.SourceGenerators.Tests.Visitor
{
    /// <summary>
    /// <see cref=""IElementVisitor""/> defines the visitor interface to visit the 
    /// implementations of the <see cref=""IElement""/>.
    /// </summary>
    internal interface IElementVisitor
    {
        /// <summary>
        /// Receive the specified <paramref name=""element"" />.
        /// </summary>
        /// <param name=""element"">The element to act upon.</param>
        void Receive(ElementA element);

        /// <summary>
        /// Receive the specified <paramref name=""element"" />.
        /// </summary>
        /// <param name=""element"">The element to act upon.</param>
        void Receive(ElementB element);

        /// <summary>
        /// Receive the specified <paramref name=""element"" />.
        /// </summary>
        /// <param name=""element"">The element to act upon.</param>
        void Receive(ElementC element);
    }
}";

        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [Test]
    public void VisitorInterfaceFileName_ExpectedResult()
    {
        const string name = "IElementVisitor";
        string? result = Template.VisitorInterfaceFileName(name);
        Assert.That(result, Is.EqualTo($"{name}.cs"));
    }

    [Test]
    public void VisitableClassImplementationExtension()
    {
        Tuple<string, string, bool>[] visitors =
        {
            new ("public", "IElementVisitorA", false),
            new ("public", "IElementVisitorB", true),
            new ("internal", "IElementVisitorC", false),
        };

        string? result = Template.VisitableClassExtension(
            "public",
            "Element",
            "BeardedPlatypus.SourceGenerators.Tests.Visitor",
            visitors);

        const string expectedResult = @"// Auto-generated code
namespace BeardedPlatypus.SourceGenerators.Tests.Visitor
{
    public partial class Element
    {
        public void Accept(IElementVisitorA visitor) => visitor.Receive(this);

        public abstract void Accept(IElementVisitorB visitor);

        internal void Accept(IElementVisitorC visitor) => visitor.Receive(this);
    }
}";

        Assert.That(result, Is.EqualTo(expectedResult));
    }
}