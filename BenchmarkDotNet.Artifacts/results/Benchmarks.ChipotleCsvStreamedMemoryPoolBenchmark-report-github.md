``` ini

BenchmarkDotNet=v0.11.0, OS=Windows 10.0.17134.191 (1803/April2018Update/Redstone4)
Intel Core i7-8650U CPU 1.90GHz (Max: 1.91GHz) (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.302
  [Host]     : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT


```
|       Method |     Mean |    Error |   StdDev |   Median |      Min |      Max |     Gen 0 |     Gen 1 |     Gen 2 | Allocated |
|------------- |---------:|---------:|---------:|---------:|---------:|---------:|----------:|----------:|----------:|----------:|
|     Parse4MB | 182.3 ms | 3.956 ms | 11.41 ms | 178.6 ms | 164.8 ms | 213.6 ms | 2666.6667 | 2000.0000 | 1666.6667 | 1327608 B |
| FindLast_4MB |       NA |       NA |       NA |       NA |       NA |       NA |       N/A |       N/A |       N/A |       N/A |

Benchmarks with issues:
  ChipotleCsvStreamedMemoryPoolBenchmark.FindLast_4MB: DefaultJob
