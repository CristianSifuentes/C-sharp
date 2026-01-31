using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using C_sharp;
using MatrixMul;

public static class Program
{
    public static async Task Main()
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));

        var a = new Matrix<double>(256, 256);
        var b = new Matrix<double>(256, 256);

        a = MatrixFill.FillDeterministic(a, 1.0);
        b = MatrixFill.FillDeterministic(b, 2.0);

        var sw = Stopwatch.StartNew();
        var c = await MatrixMultiply.MulConcurrent(
            a,
            b,
            cts.Token,
            MulOption.WithWorkers(Environment.ProcessorCount),
            MulOption.WithBlockRows(32)
        );
        sw.Stop();

        Console.WriteLine($"C[0,0]={c.At(0,0):F6} C[10,10]={c.At(10,10):F6} elapsed={sw.Elapsed} workers={Environment.ProcessorCount}");
    }
}
