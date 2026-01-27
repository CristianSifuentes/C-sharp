```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7623/25H2/2025Update/HudsonValley2)
Intel Core i5-8300H CPU 2.30GHz (Coffee Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK 10.0.100
  [Host]     : .NET 10.0.0 (10.0.0, 10.0.25.52411), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 10.0.0 (10.0.0, 10.0.25.52411), X64 RyuJIT x86-64-v3


```
| Method            | Mean     | Error    | StdDev   | Gen0     | Gen1     | Gen2     | Allocated |
|------------------ |---------:|---------:|---------:|---------:|---------:|---------:|----------:|
| MulConcurrent_512 | 66.64 ms | 1.301 ms | 2.064 ms | 857.1429 | 857.1429 | 142.8571 |      4 MB |
