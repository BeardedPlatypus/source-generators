using System.Collections.Generic;
using System.Linq;

namespace BeardedPlatypus.SourceGenerators.Utility.Internal
{
    internal static class SourceBuilderHelper
    {
        private static string Indent(string line) => "    " + line;

        internal static IEnumerable<string> WithScope(params IEnumerable<string>[] blocks)
        {
            yield return "{";
            IEnumerable<string> lines = blocks.SelectMany(block => block.Select(Indent));
            foreach (var line in lines) yield return line;
            yield return "}";
        }
    }
}