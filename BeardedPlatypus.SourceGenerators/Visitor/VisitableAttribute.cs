using System;

namespace BeardedPlatypus.SourceGenerators.Visitor
{
    /// <summary>
    /// <see cref="VisitableAttribute"/> defines the attribute with which interfaces
    /// can be marked to be visitable, i.e. a visitor interface as well as the required
    /// methods will be generated at compile time.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public sealed class VisitableAttribute : Attribute
    {
        /// <summary>
        /// Creates a new default <see cref="VisitableAttribute"/> where the name of
        /// the visitor interface is derived from the interface it is attached to.
        /// </summary>
        public VisitableAttribute() : this(null) { }

        /// <summary>
        /// Creates a new <see cref="VisitableAttribute"/> with the specified visitor
        /// interface name.
        /// </summary>
        /// <param name="visitorInterfaceName"></param>
        public VisitableAttribute(string visitorInterfaceName)
        {
            VisitorInterfaceName = visitorInterfaceName;
        }

        /// <summary>
        /// Gets the name of the visitor interface which should be obtained.
        /// </summary>
        /// <remarks>
        /// If no visitor name is defined it is null, and will be derived from the
        /// interface name.
        /// </remarks>
        public string VisitorInterfaceName { get; }
    }
}
