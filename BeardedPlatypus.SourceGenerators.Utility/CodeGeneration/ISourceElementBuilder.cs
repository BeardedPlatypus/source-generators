using System.Collections.Generic;

namespace BeardedPlatypus.SourceGenerators.Utility.CodeGeneration
{
    /// <summary>
    /// <see cref="ISourceElementBuilder"/> defines the interface of elements
    /// that can be added to a <see cref="ISourceBuilder"/>.
    /// </summary>
    public interface ISourceElementBuilder
    {
        /// <summary>
        /// Compile the current data in a enumeration of lines.
        /// </summary>
        /// <returns>
        /// The lines corresponding with the set data.
        /// </returns>
        /// <remarks>
        /// Each string in the result corresponds with a single line.
        /// </remarks>
        IEnumerable<string> Compile();
    }
}