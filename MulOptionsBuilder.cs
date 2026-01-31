using System;

namespace C_sharp;

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
