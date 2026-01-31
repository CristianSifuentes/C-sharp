using System;

namespace C_sharp;

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
