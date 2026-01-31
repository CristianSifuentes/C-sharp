using System;
using System.Threading.Channels;

namespace C_sharp;

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
        // Analize this code get deep knowlage
        foreach (var opt in opts) opt (builder);
        var cfg = builder.Build();

        var outM = new Matrix<T>(a.Rows, b.Cols);

        // Transpose B: inner loop reads contiguous memory
        var bt = b.Transpose();

        #region Channel
        var channel = Channel.CreateBounded<RowBlock>(new BoundedChannelOptions(cfg.ChannelCapacity)
        {
            SingleWriter = true,
            SingleReader = false,
            FullMode = BoundedChannelFullMode.Wait
        });
        #endregion

        #region Producer
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
        #endregion

        #region Workers
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
        #endregion

        // Wait all: producer + workers
        // If cancellation happers, ct will tear down quickly.
        await producer.ConfigureAwait(false);
        await Task.WhenAll(workers).ConfigureAwait(false);

        return outM;
        
    }
}

#endregion
