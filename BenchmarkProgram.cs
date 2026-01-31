using System;
using BenchmarkDotNet.Running;

namespace C_sharp;

public static class BenchmarkProgram
{
    public static void Main (string[] args) 
    => BenchmarkRunner.Run<MatrixBenchmarks>();
    
}