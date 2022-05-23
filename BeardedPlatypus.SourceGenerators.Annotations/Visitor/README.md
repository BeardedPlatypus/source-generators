# BeardedPlatypus.SourceGenerators.Annotations.Visitor

The visitor pattern is managed through the [Visitable attribute](VisitableAttribute.cs). It can be attached
to any (partial) interface, after which an `Accept` method is added to the interface, and implementing classes,
as well as a visitor interface.

The user code:
```csharp
// -- User code --
// IElement.cs
[Visitable]
public partial interface IElement
{
    ...
}

// Element.cs
public partial class Element : IElement 
{
    ...
}
```

The code which is generated based on the user code:
```csharp
// -- Auto-generated code --
// IElement.Visitable.cs
public partial interface IElement
{
    void Accept(BeardedPlatypus.SourceGenerators.Samples.Visitor.IElementVisitor visitor);
}

// Element.Visitable.cs
public partial class ElementA
{
    public void Accept(IElementVisitor visitor) => visitor.Visit(this);
}

// IElementVisitor.cs
public interface IElementVisitor
{
    void Visit(Element element);
}
```

For more details see the [Samples/Visitor code](../../BeardedPlatypus.SourceGenerators.Samples/Visitor/README.md)
