using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace BeardedPlatypus.SourceGenerators.Utility.Internal
{
    /// <summary>
    /// <see cref="Ensure"/> provides methods to validate parameters.
    /// </summary>
    internal static class Ensure
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
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="obj"/> is <c>null</c>.
        /// </exception>
        public static void NotNull<T>(T obj, string objName) where T : class
        {
            if (obj is null) throw new ArgumentNullException(objName);
        }

        /// <summary>
        /// Ensure <paramref name="container"/> is not null and does not contain null.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements in <paramref name="container"/>.
        /// </typeparam>
        /// <param name="container">
        /// The object to verify.
        /// </param>
        /// <param name="containerName">
        /// The name of the parameter used to generate the exception.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="container"/> is <c>null</c> or contains a null value.
        /// </exception>
        public static void ContainsNoNull<T>(IEnumerable<T> container, string containerName) where T : class
        {
            container = container.ToImmutableArray();
            NotNull(container, nameof(containerName));
            foreach (T elem in container) NotNull(elem, containerName);
        }
    }
}