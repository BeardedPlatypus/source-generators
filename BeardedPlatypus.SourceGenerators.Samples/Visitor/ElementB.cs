namespace BeardedPlatypus.SourceGenerators.Samples.Visitor;

/// <summary>
/// <see cref="ElementB"/> implements <see cref="IElement"/>.
/// </summary>
/// <remarks>
/// Note that this class is marked as partial in order to generate the
/// required Accept methods.
/// </remarks>
public partial class ElementB : IElement
{
    public void SomeCustomOperation() { }
}