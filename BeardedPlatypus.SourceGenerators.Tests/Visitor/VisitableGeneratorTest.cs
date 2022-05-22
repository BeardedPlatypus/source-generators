using System.Linq;
using System.Reflection;
using BeardedPlatypus.SourceGenerators.Annotations.Visitor;
using BeardedPlatypus.SourceGenerators.Visitor;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;

namespace BeardedPlatypus.SourceGenerators.Tests.Visitor;

[TestFixture]
public class VisitableGeneratorTest
{
    [Test]
    public void VisitableGenerator_CorrectResult()
    { 
        Compilation inputCompilation = CreateCompilation(@"
using BeardedPlatypus.SourceGenerators.Common.Visitor;

namespace BeardedPlatypus.Test
{
    [Visitable]
    public partial interface IElement { }

    public partial class ElementA : IElement { }

    public partial class ElementB : IElement { }
}
");


        VisitableGenerator generator = new();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, 
                                                          out var outputCompilation, 
                                                          out var diagnostics);

        Assert.That(diagnostics, Is.Empty);

        // - inputCompilation: IElement, ElementA, ElementB
        // - generated IElementVisitor
        // - generated IElement extension
        // - generated ElementA extension
        // - generated ElementB extension
        Assert.That(outputCompilation.SyntaxTrees.Count(), Is.EqualTo(5));

        GeneratorDriverRunResult runResult = driver.GetRunResult();
        
        Assert.That(runResult.GeneratedTrees.Length, Is.EqualTo(4));
        Assert.That(runResult.Diagnostics, Is.Empty);
    }
    
    private static Compilation CreateCompilation(string source) => 
        CSharpCompilation.Create("compilation", 
                                 new[] { CSharpSyntaxTree.ParseText(source) },
                                 new[]
                                 {
                                     MetadataReference.CreateFromFile(typeof(VisitableAttribute).GetTypeInfo().Assembly.Location),
                                     MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location),
                                 },
                                 new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));    
}