using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using BeardedPlatypus.SourceGenerators.Utility.Internal;

namespace BeardedPlatypus.SourceGenerators.Utility.CodeGeneration
{
    /// <summary>
    /// <see cref="DocBuilder"/> implements <see cref="IDocBuilder"/>.
    /// </summary>
    public sealed class DocBuilder : IDocBuilder
    {
        private readonly IImmutableList<string> _summaryLines;
        private readonly IImmutableList<string> _returnsLines;
        private readonly IImmutableList<string> _remarksLines;

        private readonly IImmutableList<(string Name, string DocStr)> _params;
        private readonly IImmutableList<(string Name, string DocStr)> _typeParams;
        private readonly IImmutableList<(string Name, IImmutableList<string> DocStr)> _exceptions;

        public DocBuilder() : this(ImmutableList.Create<string>(), 
                                   null, 
                                   null, 
                                   ImmutableList.Create<(string Name, string DocStr)>(),
                                   ImmutableList.Create<(string Name, string DocStr)>(),
                                   ImmutableList.Create<(string Name, IImmutableList<string> DocStr)>())
        { }

        private DocBuilder(DocBuilder srcBuilder,
                           IImmutableList<string> updatedSummaryLines = null,
                           IImmutableList<string> updatedReturnsLines = null,
                           IImmutableList<string> updatedRemarksLines = null,
                           IImmutableList<(string Name, string DocStr)> updatedParams = null,
                           IImmutableList<(string Name, string DocStr)> updatedTypeParams = null,
                           IImmutableList<(string Name, IImmutableList<string> DocStr)> updatedExceptions = null) :
            this(updatedSummaryLines ?? srcBuilder._summaryLines,
                 updatedReturnsLines ?? srcBuilder._returnsLines,
                 updatedRemarksLines ?? srcBuilder._remarksLines,
                 updatedParams ?? srcBuilder._params,
                 updatedTypeParams ?? srcBuilder._typeParams,
                 updatedExceptions ?? srcBuilder._exceptions)
        {  }

        private DocBuilder(IImmutableList<string> summaryLines,
                           IImmutableList<string> returnsLines,
                           IImmutableList<string> remarksLines,
                           IImmutableList<(string Name, string DocStr)> @params,
                           IImmutableList<(string Name, string DocStr)> typeParams,
                           IImmutableList<(string Name, IImmutableList<string> DocStr)> exceptions)
        {
            _summaryLines = summaryLines;
            _returnsLines = returnsLines;
            _remarksLines = remarksLines;
            _params = @params;
            _typeParams = typeParams;
            _exceptions = exceptions;
        }

        public IDocBuilder WithSummary(string summary) =>
            WithSummary(SplitDocString(summary)?.ToArray());

        public IDocBuilder WithSummary(params string[] summary)
        {
            Ensure.ContainsNoNull(summary, nameof(summary));
            return new DocBuilder(this, updatedSummaryLines: summary.ToImmutableArray());
        }

        public IDocBuilder WithTypeParam(string name, string docStr) => 
            WithTypeParams((name, docStr));

        public IDocBuilder WithTypeParams(params (string Name, string DocStr)[] parameters)
        {
            ValidateStringTupleArray(parameters, nameof(parameters));
            return new DocBuilder(this, updatedTypeParams: _typeParams.AddRange(parameters));
        }

        public IDocBuilder WithParam(string name, string docStr) =>
            WithParams((name, docStr));

        public IDocBuilder WithParams(params (string Name, string DocStr)[] parameters)
        {
            ValidateStringTupleArray(parameters, nameof(parameters));
            return new DocBuilder(this, updatedParams: _params.AddRange(parameters));
        }

        public IDocBuilder WithReturns(string returns) =>
            WithReturns(SplitDocString(returns)?.ToArray());

        public IDocBuilder WithReturns(params string[] returns)
        {
            Ensure.ContainsNoNull(returns, nameof(returns));
            return new DocBuilder(this, updatedReturnsLines: returns.ToImmutableList());
        }

        public IDocBuilder WithException(string name, string docStr) =>
            WithExceptions((name, docStr));

        public IDocBuilder WithExceptions(params (string Name, string DocStr)[] exceptions)
        {
            ValidateStringTupleArray(exceptions, nameof(exceptions));

            IEnumerable<(string Name, IImmutableList<string>)> exceptionData = 
                exceptions.Select(v => (v.Name, (IImmutableList<string>) SplitDocString(v.DocStr).ToImmutableList()));
            return new DocBuilder(this, updatedExceptions: _exceptions.AddRange(exceptionData));
        }

        public IDocBuilder WithRemarks(string remarks) =>
            WithRemarks(SplitDocString(remarks)?.ToArray());

        public IDocBuilder WithRemarks(params string[] remarks)
        {
            Ensure.ContainsNoNull(remarks, nameof(remarks));
            return new DocBuilder(this, updatedRemarksLines: remarks.ToImmutableList());
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
            CompileBlock("summary", _summaryLines);

        private IEnumerable<string> CompileTypeParams() =>
            _typeParams.Select(ToTypeParamStr);

        private static string ToTypeParamStr((string Name, string DocStr) typeParam) =>
            $"/// <typeparam name=\"{typeParam.Name}\">{typeParam.DocStr}</typeparam>";

        private IEnumerable<string> CompileParams() =>
            _params.Select(ToParamStr);

        private static string ToParamStr((string Name, string DocStr) param) =>
            $"/// <param name=\"{param.Name}\">{param.DocStr}</param>";

        private IEnumerable<string> CompileReturns() =>
            CompileOptionalBlock("returns", _returnsLines);
        private IEnumerable<string> CompileRemarks() =>
            CompileOptionalBlock("remarks", _remarksLines);

        private static IEnumerable<string> CompileOptionalBlock(string header, IEnumerable<string> lines) =>
            lines != null ? CompileBlock(header, lines) : Enumerable.Empty<string>();

        private static IEnumerable<string> CompileBlock(string header, IEnumerable<string> lines)
        {
            yield return $"/// <{header}>";
            foreach (var line in lines) yield return $"/// {line}";
            yield return $"/// </{header}>";
        }

        private IEnumerable<string> CompileExceptions() =>
            _exceptions.SelectMany(ToExceptionStr);

        private static IEnumerable<string> ToExceptionStr((string Name, IImmutableList<string> DocStr) exception)
        {
            yield return $"/// <exception cref=\"{exception.Name}\">";
            foreach (var line in exception.DocStr) yield return $"/// {line}";
            yield return $"/// </exception>";
        }

        private static IEnumerable<string> SplitDocString(string docStr) =>
            docStr?.Replace("\r\n", "\n").Split('\n');

        private void ValidateStringTupleArray((string Name, string DocStr)[] tuples, string paramName)
        {
            Ensure.NotNull(tuples, paramName);
            foreach (var (name, docStr) in tuples)
            { 
                Ensure.NotNull(name, paramName); 
                Ensure.NotNull(docStr, paramName);
            }
        }
    }
}