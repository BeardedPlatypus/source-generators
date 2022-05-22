namespace BeardedPlatypus.SourceGenerators.Samples.Visitor;

// TODO: provide a more concrete and less contrived example.
/// <summary>
/// <see cref="SampleElementVisitor"/> is a contrived example of implementing the
/// <see cref="IElementVisitor"/> generated from the visitable <see cref="IElement"/>.
///
/// For each implementation of <see cref="IElement"/> it takes the type of the element
/// and stores it in the <see cref="ElementTypeName"/>.
/// </summary>
public sealed class SampleElementVisitor : IElementVisitor
{
    /// <summary>
    /// Gets the typename of the element for which Receive was called last.
    /// </summary>
    /// <remarks>
    /// If Receive was not called before, it returns null.
    /// </remarks>
    public string? ElementTypeName { get; private set; } = null;

    public void Visit(ElementA element) => 
        ElementTypeName = element.GetType().Name;

    public void Visit(ElementB element) =>
        ElementTypeName = element.GetType().Name;

    public void Visit(ElementC element) =>
        ElementTypeName = element.GetType().Name;
}