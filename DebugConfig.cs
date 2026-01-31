using System;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;

namespace C_sharp;

public sealed class DebugConfig: ManualConfig
{
    public DebugConfig()
    {
        AddJob(Job.ShortRun
            .WithId("Debug")
            .WithRuntime(CoreRuntime.Core80)
            .WithGcServer(true)
            .WithJit(Jit.RyuJit)
            .WithPlatform(Platform.X64)
            .WithEnvironmentVariable(
              new EnvironmentVariable("COMPlus_TieredCompilation", "0")
            ));
        Options |= ConfigOptions.DisableOptimizationsValidator;

    }

}
