using System;

namespace BeardedPlatypus.SourceGenerators.Utility.Internal
{
    /// <summary>
    /// <see cref="EnsureExtensions"/> provides methods to validate parameters.
    /// </summary>
    internal static class EnsureExtensions
    {
        /// <summary>
        /// Ensure <paramref name="obj"/> is not null.
        /// </summary>
        /// <typeparam name="T">
        /// The type of <paramref name="obj"/>.
        /// </typeparam>
        /// <param name="obj">
        /// The object to verify.
        /// </param>
        /// <param name="objName">
        /// The name of the parameter used to generate the exception.
        /// </param>
        /// <returns>
        /// <paramref name="obj"/> if <paramref name="obj"/> is not <c>null</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="obj"/> is <c>null</c>.
        /// </exception>
        public static T EnsureNotNull<T>(this T obj, string objName) where T : class => 
            obj ?? throw new ArgumentNullException(objName);
    }
}