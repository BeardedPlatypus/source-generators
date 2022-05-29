using System.Collections.Generic;
using System.Linq;
using BeardedPlatypus.SourceGenerators.Utility.Internal;

namespace BeardedPlatypus.SourceGenerators.Utility.CodeGeneration
{
    /// <summary>
    /// <see cref="DocBuilder"/> implements <see cref="IDocBuilder"/>.
    /// </summary>
    public sealed class DocBuilder : IDocBuilder
    {
        private IList<string> SummaryLines { get; set; } = new List<string>();
        private IList<string> ReturnsLines { get; set; } = null;
        private IList<string> RemarksLines { get; set; } = null;

        private IList<(string Name, string DocStr)> Params { get; } = 
            new List<(string Name, string DocStr)>();

        private IList<(string Name, string DocStr)> TypeParams { get; } = 
            new List<(string Name, string DocStr)>();

        private IList<(string Name, IEnumerable<string> DocStr)> Exceptions { get; } = 
            new List<(string Name, IEnumerable<string> DocStr)>();

        public void AddSummary(string summary) =>
            AddSummary(SplitDocString(summary)?.ToArray());

        public IDocBuilder WithSummary(string summary)
        {
            AddSummary(summary);
            return this;
        }

        public void AddSummary(params string[] summary)
        {
            Ensure.ContainsNoNull(summary, nameof(summary));
            SummaryLines = summary;
        }

        public IDocBuilder WithSummary(params string[] summary)
        {
            AddSummary(summary);
            return this;
        }

        public void AddTypeParam(string name, string docStr)
        {
            Ensure.NotNull(name, nameof(name));
            Ensure.NotNull(docStr, nameof(docStr));

            TypeParams.Add((name, docStr));
        }

        public IDocBuilder WithTypeParam(string name, string docStr)
        {
            AddTypeParam(name, docStr);
            return this;
        }

        public void AddTypeParams(params (string Name, string DocStr)[] parameters)
        {
            Ensure.NotNull(parameters, nameof(parameters));
            foreach (var (name, doc) in parameters) AddTypeParam(name, doc);
        }

        public IDocBuilder WithTypeParams(params (string Name, string DocStr)[] parameters)
        {
            AddTypeParams(parameters);
            return this;
        }

        public void AddParam(string name, string docStr)
        {
            Ensure.NotNull(name, nameof(name));
            Ensure.NotNull(docStr, nameof(docStr));

            Params.Add((name, docStr));
        }

        public IDocBuilder WithParam(string name, string docStr)
        {
            AddParam(name, docStr);
            return this;
        }

        public void AddParams(params (string Name, string DocStr)[] parameters)
        {
            Ensure.NotNull(parameters, nameof(parameters));
            foreach (var (name, doc) in parameters) AddParam(name, doc);
        }

        public IDocBuilder WithParams(params (string Name, string DocStr)[] parameters)
        {
            AddParams(parameters);
            return this;
        }

        public void AddReturns(string returns) => 
            AddReturns(SplitDocString(returns)?.ToArray());

        public IDocBuilder WithReturns(string returns)
        {
            AddReturns(returns);
            return this;
        }

        public void AddReturns(params string[] returns)
        {
            Ensure.ContainsNoNull(returns, nameof(returns));
            ReturnsLines = returns;
        }

        public IDocBuilder WithReturns(params string[] returns)
        {
            AddReturns(returns);
            return this;
        }

        public void AddException(string name, string docStr)
        {
            Ensure.NotNull(name, nameof(name));
            Ensure.NotNull(docStr, nameof(docStr));

            Exceptions.Add((name, SplitDocString(docStr)));
        }

        public IDocBuilder WithException(string name, string docStr)
        {
            AddException(name, docStr);
            return this;
        }

        public void AddExceptions(params (string Name, string DocStr)[] exceptions)
        {
            Ensure.NotNull(exceptions, nameof(exceptions));
            foreach (var (name, doc) in exceptions) AddException(name, doc);
        }

        public IDocBuilder WithExceptions(params (string Name, string DocStr)[] exceptions)
        {
            AddExceptions(exceptions);
            return this;
        }

        public void AddRemarks(string remarks) =>
            AddRemarks(SplitDocString(remarks)?.ToArray());

        public IDocBuilder WithRemarks(string remarks)
        {
            AddRemarks(remarks);
            return this;
        }

        public void AddRemarks(params string[] remarks)
        {
            Ensure.ContainsNoNull(remarks, nameof(remarks));
            RemarksLines = remarks;
        }

        public IDocBuilder WithRemarks(params string[] remarks)
        {
            AddRemarks(remarks);
            return this;
        }

        public IEnumerable<string> Compile()
        {
            IEnumerable<string>[] items =
            {
                CompileSummary(),
                CompileTypeParams(),
                CompileParams(),
                CompileReturns(),
                CompileExceptions(),
                CompileRemarks(),
            };

            return items.SelectMany(identity => identity);
        }

        private IEnumerable<string> CompileSummary() =>
            CompileBlock("summary", SummaryLines);

        private IEnumerable<string> CompileTypeParams() =>
            TypeParams.Select(ToTypeParamStr);

        private static string ToTypeParamStr((string Name, string DocStr) typeParam) =>
            $"/// <typeparam name=\"{typeParam.Name}\">{typeParam.DocStr}</typeparam>";

        private IEnumerable<string> CompileParams() =>
            Params.Select(ToParamStr);

        private static string ToParamStr((string Name, string DocStr) param) =>
            $"/// <param name=\"{param.Name}\">{param.DocStr}</param>";

        private IEnumerable<string> CompileReturns() =>
            CompileOptionalBlock("returns", ReturnsLines);
        private IEnumerable<string> CompileRemarks() =>
            CompileOptionalBlock("remarks", RemarksLines);

        private static IEnumerable<string> CompileOptionalBlock(string header, IEnumerable<string> lines) =>
            lines != null ? CompileBlock(header, lines) : Enumerable.Empty<string>();

        private static IEnumerable<string> CompileBlock(string header, IEnumerable<string> lines)
        {
            yield return $"/// <{header}>";
            foreach (var line in lines) yield return $"/// {line}";
            yield return $"/// </{header}>";
        }

        private IEnumerable<string> CompileExceptions() =>
            Exceptions.SelectMany(ToExceptionStr);

        private static IEnumerable<string> ToExceptionStr((string Name, IEnumerable<string> DocStr) exception)
        {
            yield return $"/// <exception cref=\"{exception.Name}\">";
            foreach (var line in exception.DocStr) yield return $"/// {line}";
            yield return $"/// </exception>";
        }

        private IEnumerable<string> SplitDocString(string docStr) =>
            docStr?.Replace("\r\n", "\n").Split('\n');
    }
}