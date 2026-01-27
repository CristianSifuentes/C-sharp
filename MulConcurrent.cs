using System;
using System.Numerics;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace MatrixMul;

#region MulOptions
/*
When applied to a class, the sealed modifier prevents other classes from inheriting from it. In the following example, class B inherits from class A, but no class can inherit from class B.

class A {}
sealed class B : A {}

*/

// ---------- Option pattern (production-friendly config) ----------
public sealed class MulOptions
{
    public int Workers { get; init; } = Math.Max(1, Environment.ProcessorCount);
    public int BlockRows { get; init; } = 16; // tune: how manu rows per job
    public int ChannelCapacity { get; init; } = 256;
}

#endregion

#region MulOption
public static class MulOption
{
    public static Action<MulOptionsBuilder> WithWorkers(int n) =>
        b =>
        {
            if (n > 0)
                b.Workers = n;
        };

    public static Action<MulOptionsBuilder> WithBlockRows(int n) =>
        b =>
        {
            if (n > 0)
                b.BlockRows = n;
        };

    public static Action<MulOptionsBuilder> WithChaneelCapacity(int n) =>
        b =>
        {
            if (n > 0)
                b.ChannelCapacity = n;
        };
}

#endregion

#region MulOptionsBuilder
public sealed class MulOptionsBuilder
{
    public int Workers { get; set; } = Math.Max(1, Environment.ProcessorCount);
    public int BlockRows { get; set; } = 16;
    public int ChannelCapacity { get; set; } = 256;

    public MulOptions Build() =>
        new()
        {
            Workers = Workers,
            BlockRows = BlockRows,
            ChannelCapacity = ChannelCapacity,
        };
}

#endregion

#region RowBlock
// ---------- Concurrency: worker pool + channel + cancellation ----------

/*
A record in C# is a concise class type for immutable data models, emphasizing value-based equality (data is equal if its contents are equal, not if it is the same instance in memory) and automatically generating useful methods such as ToString() and equality comparisons, facilitating work with DTOs (Data Transfer Objects) and immutability, available since C# 9. It allows non-destructive mutation with the with expression, deconstruction, and can be defined as a reference type (record class) or a value type (record struct).
*/

/*
The `internal` keyword is an access modifier for types and type members.

This page is about the `internal` access modifier. The `internal` keyword is also part of the `protected internal` access modifier.

Internal types and members can only be accessed within files of the same assembly, as in this example:

C#
public class BaseClass
{
// Only accessible within the same assembly.
internal static int x = 0;

}

*/
internal readonly record struct RowBlock(int Start, int EndExclusive);

#endregion

#region MatrixMultiply
public static class MatrixMultiply
{
    // Performs C = A * B using:
    // - CancellationToken (like Go context.Contex)
    // - Transpose(B) for cache locality
    // - Worker pool reading jobs from a Channel<RowBlock>
    public static async Task<Matrix<T>> MulConcurrent<T>(
        Matrix<T> a,
        Matrix<T> b,
        CancellationToken ct,
        params Action<MulOptionsBuilder>[] opts
    )
        where T : unmanaged, INumber<T>
    {
        if (a.Rows <= 0 || a.Cols <= 0 || b.Rows <=0 || b.Cols <= 0)
        {
            throw MatrixException.EmptyMatrix();
        }
        if (a.Cols != b.Rows)
        {
            throw MatrixException.DimensionMismatch();
        }
        
        var builder = new MulOptionsBuilder();
        foreach (var opt in opts) opt (builder);
        var cfg = builder.Build();

        var outM = new Matrix<T>(a.Rows, b.Cols);

        // Transpose B: inner loop reads contiguous memory
        var bt = b.Transpose();

        var channel = Channel.CreateBounded<RowBlock>(new BoundedChannelOptions(cfg.ChannelCapacity)
        {
            SingleWriter = true,
            SingleReader = false,
            FullMode = BoundedChannelFullMode.Wait
        });

        // Producer: emits row blocks
        var producer = Task.Run(async () =>
        {
            try
            {
                for (int start = 0; start < a.Rows; start += cfg.BlockRows)
                {
                    ct.ThrowIfCancellationRequested();

                    int end = Math.Min(a.Rows, start + cfg.BlockRows);
                    await channel.Writer.WriteAsync(new RowBlock(start, end), ct);
                }
            }
            finally
            {
                channel.Writer.TryComplete();
            }
            
            
        }, ct);

        // Workers
        Task[] workers = new Task[cfg.Workers];
        for (int w = 0; w < cfg.Workers; w++)
        {
            workers[w] = Task.Run(async () =>
            {
                // Hot variables (reduce repeated property lookups)
                int outCols = outM.Cols;
                int kMax = a.Cols;

                var reader = channel.Reader;
                while (await reader.WaitToReadAsync(ct).ConfigureAwait(false))
                {
                    while (reader.TryRead(out var job))
                    {
                        ct.ThrowIfCancellationRequested();

                        for (int i = job.Start; i < job.EndExclusive; i++)
                        {
                            int aRow = i * a.Cols;
                            int outRow = i * outM.Cols;

                            for (int j = 0; j < outCols; j++)
                            {
                                // dot (a row, b col) == dot(a row, bt row)
                                T sum = T.Zero;
                                int btRow = j * bt.Cols;
                                for (int k = 0; k <kMax; k++)
                                {
                                    sum += a.Data[aRow + k] * bt.Data[btRow + k];
                                    outM.Data[outRow + j] = sum;
                                }
                            }
                            
                        } 
                        
                    }
                }

            }, ct);
        }

        // Wait all: producer + workers
        // If cancellation happers, ct will tear down quickly.
        await producer.ConfigureAwait(false);
        await Task.WhenAll(workers).ConfigureAwait(false);

        return outM;
        


    }
}

#endregion
