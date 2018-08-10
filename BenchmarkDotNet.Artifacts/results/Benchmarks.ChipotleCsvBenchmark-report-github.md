``` ini

BenchmarkDotNet=v0.11.0, OS=Windows 10.0.17134.191 (1803/April2018Update/Redstone4)
Intel Core i7-8650U CPU 1.90GHz (Max: 1.91GHz) (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.302
  [Host]     : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT


```
|    Method |        Mean |     Error |    StdDev |         Min |         Max |      Median |    Gen 0 | Allocated |
|---------- |------------:|----------:|----------:|------------:|------------:|------------:|---------:|----------:|
|  Parse2KB |    91.17 us |  1.789 us |  2.388 us |    88.64 us |    95.19 us |    90.26 us |   6.9580 |  28.75 KB |
|  Parse4KB |   269.45 us |  5.980 us |  7.562 us |   263.94 us |   286.91 us |   265.17 us |  20.5078 |  84.55 KB |
|  Parse8KB |   648.62 us |  2.459 us |  2.300 us |   644.21 us |   652.44 us |   648.61 us |  49.8047 | 205.75 KB |
| Parse16KB | 1,335.14 us | 26.434 us | 43.433 us | 1,282.86 us | 1,423.75 us | 1,339.30 us |  99.6094 | 410.29 KB |
| Parse32KB | 2,803.02 us | 51.663 us | 61.501 us | 2,691.65 us | 2,889.64 us | 2,819.31 us | 207.0313 | 856.95 KB |
