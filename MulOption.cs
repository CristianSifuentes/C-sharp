using System;

namespace C_sharp;

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