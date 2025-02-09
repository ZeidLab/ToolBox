using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace ZeidLab.ToolBox.Test.Benchmarks.Results;

[RPlotExporter, MemoryDiagnoser, RankColumn]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class ResultBenchmarks
{
    [Params(10_000, 100_000, 1_000_000)] public int Length { get; set; }
    private int[] _testParams = [];

    [GlobalSetup]
    public void SetupParams()
    {
        _testParams = Enumerable.Range(0, Length).ToArray();
    }

    [Benchmark, BenchmarkCategory("Alloc")]
    public List<ToolBox.Results.Result<int>> ToolBoxResultSuccessWithAlloc()
    {
        return _testParams
            .Select( x => ZeidLab.ToolBox.Results.Result.Success(x))
            .ToList();
    }

    [Benchmark, BenchmarkCategory("None-Alloc")]
    public void ToolBoxResultSuccess()
    {
        foreach (var t in _testParams)
        {
            _ = ZeidLab.ToolBox.Results.Result.Success(t);
        }
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Alloc")]
    public List<LanguageExt.Common.Result<int>> LanguageExtResultSuccessWithAlloc()
    {
        return _testParams
            .Select( x => new LanguageExt.Common.Result<int>(x))
            .ToList();
    }

    [Benchmark, BenchmarkCategory("None-Alloc")]
    public void LanguageExtResultSuccess()
    {
        foreach (var t in _testParams)
        {
            _ = new LanguageExt.Common.Result<int>(t);
        }
    }
}