using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using MatrixMul;
using Microsoft.Extensions.Options;

namespace C_sharp;

[MemoryDiagnoser]
public class MatrixBenchmarks
{
    private Matrix<double> _a = default;
    private Matrix<double> _b = default;

    [GlobalSetup]
    public void Setup()
    {
        _a = MatrixFill.FillDeterministic(new Matrix<double>(512, 512), 1.0);
        _b = MatrixFill.FillDeterministic(new Matrix<double>(512, 512), 2.0);
    }
    [Benchmark]
    public async Task MulConcurrent_512()
    {
        _ = await MatrixMultiply.MulConcurrent(
            _a, _b, CancellationToken.None,
            MulOption.WithWorkers(System.Environment.ProcessorCount),
            MulOption.WithBlockRows(32));
        
    }
}



    public static void Main(string[] args) => BenchmarkRunner.Run<MatrixBenchmarks>();
}