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
            "namespace BeardedPlatypus.SourceGenerators.Tests.Visitor"                                                                                  + "\n" +
            "{"                                                                                                   + "\n" +
            "    public partial interface IElement"                                                               + "\n" +
            "    {"                                                                                               + "\n" +
            "        /// <summary>"                                                                               + "\n" +
            "        /// Accept the specified <paramref name=\"visitor\"/>."                                      + "\n" +
            "        /// </summary>"                                                                              + "\n" +
            "        /// <param name=\"visitor\">The visitor which visits this <see cref=\"IElement\"/>.</param>" + "\n" +
            "        void Accept(IElementVisitor visitor);"                                                       + "\n" +
            "    }"                                                                                               + "\n" +
            "}";

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
            "BeardedPlatypus.SourceGenerators.Tests.Visitor",
            classes);

        const string expectedResult = 
            "namespace BeardedPlatypus.SourceGenerators.Tests.Visitor"                                                                                  + "\n" +
            "{"                                                                    + "\n" +
            "    internal interface IElementVisitor"                               + "\n" +
            "    {"                                                                + "\n" + 
            "        /// <summary>"                                                + "\n" +
            "        /// Receive the specified <paramref name=\"element\"."        + "\n" +
            "        /// </summary>"                                               + "\n" +
            "        /// <param name=\"element\">The element to act upon.</param>" + "\n" +
            "        void Receive(ElementA element);"                              + "\n" +
            "\n" +
            "        /// <summary>"                                                + "\n" +
            "        /// Receive the specified <paramref name=\"element\"."        + "\n" +
            "        /// </summary>"                                               + "\n" +
            "        /// <param name=\"element\">The element to act upon.</param>" + "\n" +
            "        void Receive(ElementB element);"                              + "\n" +
            "\n" +
            "        /// <summary>"                                                + "\n" +
            "        /// Receive the specified <paramref name=\"element\"."        + "\n" +
            "        /// </summary>"                                               + "\n" +
            "        /// <param name=\"element\">The element to act upon.</param>" + "\n" +
            "        void Receive(ElementC element);"                              + "\n" +
            "    }"                                                                + "\n" +
            "}";

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

        const string expectedResult =
            "namespace BeardedPlatypus.SourceGenerators.Tests.Visitor"       + "\n" +
            "{"                                                              + "\n" +
            "    public partial class Element"                               + "\n" +
            "    {"                                                          + "\n" +
            "        public void Accept(IElementVisitorA visitor) =>"        + "\n" +
            "            visitor.Receive(this);"                             + "\n" +
            "\n" +
            "        public abstract void Accept(IElementVisitorB visitor);" + "\n" +
            "\n" +
            "        internal void Accept(IElementVisitorC visitor) =>"      + "\n" +
            "            visitor.Receive(this);"                             + "\n" +
            "    }"                                                          + "\n" +
            "}";

        Assert.That(result, Is.EqualTo(expectedResult));
    }
}