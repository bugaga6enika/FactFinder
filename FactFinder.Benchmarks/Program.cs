// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using FactFinder.Benchmarks;

Console.WriteLine("Start");

BenchmarkRunner.Run<IsFormatOldVsNew>();

Console.ReadLine();

