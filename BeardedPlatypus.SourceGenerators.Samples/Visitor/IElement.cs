using BeardedPlatypus.SourceGenerators.Annotations.Visitor;

namespace BeardedPlatypus.SourceGenerators.Samples.Visitor;

/// <summary>
/// <see cref="IElement"/> defines an interface annotated with the
/// <see cref="VisitableAttribute"/>.
/// </summary>
/// <remarks>
/// Note that this interface is marked as partial in order to add the
/// Accept methods.
/// </remarks>
[Visitable]
public partial interface IElement
{
    /// <summary>
    /// Perform some custom operation.
    /// </summary>
    void SomeCustomOperation();
}