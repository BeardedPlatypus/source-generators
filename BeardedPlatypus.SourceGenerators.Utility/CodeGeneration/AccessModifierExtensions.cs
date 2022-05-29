using System;

namespace BeardedPlatypus.SourceGenerators.Utility.CodeGeneration
{
    /// <summary>
    /// <see cref="AccessModifierExtensions"/> provides utility
    /// methods for dealing with <see cref="AccessModifier"/> enums.
    /// </summary>
    public static class AccessModifierExtensions
    {
        /// <summary>
        /// Convert <paramref name="accessModifier"/> to its corresponding <see cref="string"/>.
        /// </summary>
        /// <param name="accessModifier">The access modifier to convert.</param>
        /// <returns>
        /// The <see cref="string"/> corresponding with the provided <paramref name="accessModifier"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when an invalid <see cref="AccessModifier"/> is specified.
        /// </exception>
        public static string ToSyntaxString(this AccessModifier accessModifier)
        {
            switch (accessModifier)
            {
                case AccessModifier.Public:
                    return "public";
                case AccessModifier.Internal:
                    return "internal";
                case AccessModifier.Protected:
                    return "protected";
                case AccessModifier.Private:
                    return "private";
                default:
                    throw new ArgumentOutOfRangeException(nameof(accessModifier), accessModifier, null);
            }
        }
    }
}