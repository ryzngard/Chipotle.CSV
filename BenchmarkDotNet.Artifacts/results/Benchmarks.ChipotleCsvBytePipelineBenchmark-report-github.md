``` ini

BenchmarkDotNet=v0.11.0, OS=Windows 10.0.17134.191 (1803/April2018Update/Redstone4)
Intel Core i7-8650U CPU 1.90GHz (Max: 1.91GHz) (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.302
  [Host]     : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT


```
|    Method |        Mean |      Error |     StdDev |         Min |         Max |      Median |    Gen 0 | Allocated |
|---------- |------------:|-----------:|-----------:|------------:|------------:|------------:|---------:|----------:|
|  Parse2KB |    56.74 us |  0.2274 us |  0.1898 us |    56.52 us |    57.21 us |    56.68 us |   7.0801 |   29896 B |
|  Parse4KB |   168.88 us |  1.0695 us |  1.0004 us |   167.33 us |   170.94 us |   168.97 us |  20.7520 |   87904 B |
|  Parse8KB |   396.91 us |  7.9945 us |  9.5169 us |   389.48 us |   417.98 us |   391.60 us |  50.7813 |  213880 B |
| Parse16KB |   803.13 us | 15.4720 us | 17.8176 us |   777.96 us |   832.11 us |   801.51 us | 101.5625 |  426496 B |
| Parse32KB | 1,746.87 us | 34.5698 us | 73.6710 us | 1,619.40 us | 1,907.71 us | 1,770.51 us | 210.9375 |  890856 B |
|  Parse4MB |          NA |         NA |         NA |          NA |          NA |          NA |      N/A |       N/A |

Benchmarks with issues:
  ChipotleCsvBytePipelineBenchmark.Parse4MB: DefaultJob
