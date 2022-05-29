using System.Collections.Generic;

namespace BeardedPlatypus.SourceGenerators.Utility.CodeGeneration
{
    /// <summary>
    /// <see cref="IDocBuilder"/> defines the methods required to build
    /// the doc strings of any method, class, or interface.
    /// </summary>
    public interface IDocBuilder
    {
        /// <summary>
        /// Add the summary doc-string and return the new builder state.
        /// </summary>
        /// <param name="summary">The summary doc-string.</param>
        /// <returns>
        /// The new <see cref="IDocBuilder"/> state.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="summary"/> is <c>null</c>.
        /// </exception>
        IDocBuilder WithSummary(string summary);

        /// <summary>
        /// Add the summary doc-string per line and return the new builder state.
        /// </summary>
        /// <param name="summary">The lines of the doc-string</param>
        /// <returns>
        /// The new <see cref="IDocBuilder"/> state.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when any value in <paramref name="summary"/> is <c>null</c>.
        /// </exception>
        /// <remarks>
        /// Each value in summary corresponds with a single line in the summary.
        /// </remarks>
        IDocBuilder WithSummary(params string[] summary);

        /// <summary>
        /// Add a new type-parameter doc-string and return the new builder state.
        /// </summary>
        /// <param name="name">The name of the type-parameter</param>
        /// <param name="docStr">The doc-string of the type-parameter.</param>
        /// <returns>
        /// The new <see cref="IDocBuilder"/> state.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="name"/> or <paramref name="docStr"/> is <c>null</c>.
        /// </exception>
        IDocBuilder WithTypeParam(string name, string docStr);

        /// <summary>
        /// Add the specified type-parameter doc-strings and return the new builder state.
        /// </summary>
        /// <param name="parameters">The doc-strings of the type-parameters.</param>
        /// <returns>
        /// The new <see cref="IDocBuilder"/> state.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="parameters"/> is <c>null</c>
        /// or any value contained in <paramref name="parameters"/> is <c>null</c>.
        /// </exception>
        IDocBuilder WithTypeParams(params (string Name, string DocStr)[] parameters);

        /// <summary>
        /// Add a new parameter doc-string and returns this builder.
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="docStr">The doc-string of the parameter.</param>
        /// <returns>
        /// The new <see cref="IDocBuilder"/> state.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="name"/> or <paramref name="docStr"/> is <c>null</c>.
        /// </exception>
        IDocBuilder WithParam(string name, string docStr);

        /// <summary>
        /// Add the specified parameter doc-strings and returns this builder.
        /// </summary>
        /// <param name="parameters">The doc-strings of the parameters.</param>
        /// <returns>
        /// This <see cref="IDocBuilder"/>
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="parameters"/> is <c>null</c>
        /// or any value contained in <paramref name="parameters"/> is <c>null</c>.
        /// </exception>
        IDocBuilder WithParams(params (string Name, string DocStr)[] parameters);

        /// <summary>
        /// Add the returns doc-string and return this builder.
        /// </summary>
        /// <param name="returns">The returns doc-string.</param>
        /// <returns>
        /// This <see cref="IDocBuilder"/>
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="returns"/> is <c>null</c>.
        /// </exception>
        IDocBuilder WithReturns(string returns);

        /// <summary>
        /// Add the returns doc-string per line and return this builder.
        /// </summary>
        /// <param name="returns">The lines of the doc-string</param>
        /// <returns>
        /// This <see cref="IDocBuilder"/>
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when any value in <paramref name="returns"/> is <c>null</c>.
        /// </exception>
        /// <remarks>
        /// Each value in returns corresponds with a single line in the returns doc-str.
        /// </remarks>
        IDocBuilder WithReturns(params string[] returns);

        /// <summary>
        /// Add a new exception doc-string and return this builder.
        /// </summary>
        /// <param name="name">The name of the exception</param>
        /// <param name="docStr">The doc-string of the exception.</param>
        /// <returns>
        /// This <see cref="IDocBuilder"/>
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="name"/> or <paramref name="docStr"/> is <c>null</c>.
        /// </exception>
        IDocBuilder WithException(string name, string docStr);

        /// <summary>
        /// Add the specified exception doc-strings and return this builder.
        /// </summary>
        /// <param name="exceptions">The doc-strings of the exceptions.</param>
        /// <returns>
        /// This <see cref="IDocBuilder"/>
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="exceptions"/> is <c>null</c>
        /// or any value contained in <paramref name="exceptions"/> is <c>null</c>.
        /// </exception>
        IDocBuilder WithExceptions(params (string Name, string DocStr)[] exceptions);

        /// <summary>
        /// Add the remarks doc-string and return this builder.
        /// </summary>
        /// <param name="remarks">The remarks doc-string.</param>
        /// <returns>
        /// This <see cref="IDocBuilder"/>
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="remarks"/> is <c>null</c>.
        /// </exception>
        IDocBuilder WithRemarks(string remarks);

        /// <summary>
        /// Add the remarks doc-string per line and return this builder.
        /// </summary>
        /// <param name="remarks">The lines of the doc-string</param>
        /// <returns>
        /// This <see cref="IDocBuilder"/>
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when any value in <paramref name="remarks"/> is <c>null</c>.
        /// </exception>
        /// <remarks>
        /// Each value in remarks corresponds with a single line in the remarks doc-str.
        /// </remarks>
        IDocBuilder WithRemarks(params string[] remarks);

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