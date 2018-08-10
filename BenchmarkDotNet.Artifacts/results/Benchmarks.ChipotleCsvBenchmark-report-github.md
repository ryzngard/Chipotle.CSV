``` ini

BenchmarkDotNet=v0.11.0, OS=Windows 10.0.17134.191 (1803/April2018Update/Redstone4)
Intel Core i7-8650U CPU 1.90GHz (Max: 1.91GHz) (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.302
  [Host]     : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT


```
|    Method |       Mean |     Error |    StdDev |        Min |        Max |     Median |    Gen 0 | Allocated |
|---------- |-----------:|----------:|----------:|-----------:|-----------:|-----------:|---------:|----------:|
|  Parse2KB |         NA |        NA |        NA |         NA |         NA |         NA |      N/A |       N/A |
|  Parse4KB |   295.2 us |  5.854 us |  7.404 us |   282.2 us |   307.0 us |   297.6 us |  22.9492 |   96856 B |
|  Parse8KB |   697.8 us | 13.637 us | 18.666 us |   660.7 us |   741.3 us |   691.7 us |  55.6641 |  235776 B |
| Parse16KB | 1,432.8 us | 27.851 us | 45.760 us | 1,374.1 us | 1,540.2 us | 1,437.9 us | 111.3281 |  470312 B |
| Parse32KB | 2,957.5 us | 58.006 us | 75.424 us | 2,854.9 us | 3,069.9 us | 2,942.5 us | 230.4688 |  982432 B |

Benchmarks with issues:
  ChipotleCsvBenchmark.Parse2KB: DefaultJob
