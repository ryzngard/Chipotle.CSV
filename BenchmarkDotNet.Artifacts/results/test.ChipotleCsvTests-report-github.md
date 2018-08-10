``` ini

BenchmarkDotNet=v0.11.0, OS=Windows 10.0.17134.191 (1803/April2018Update/Redstone4)
Intel Core i7-8650U CPU 1.90GHz (Max: 1.91GHz) (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.302
  [Host]     : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT


```
|    Method |        Mean |      Error |     StdDev |         Min |         Max |      Median |  Gen 0 | Allocated |
|---------- |------------:|-----------:|-----------:|------------:|------------:|------------:|-------:|----------:|
|  Parse2KB |    40.98 us |  0.3584 us |  0.2993 us |    40.60 us |    41.52 us |    40.95 us | 0.3052 |   1.33 KB |
|  Parse4KB |   123.56 us |  2.4659 us |  3.8391 us |   119.38 us |   135.88 us |   122.34 us | 0.2441 |   1.47 KB |
|  Parse8KB |   312.93 us |  6.0245 us |  8.0425 us |   294.25 us |   321.22 us |   315.40 us |      - |   1.87 KB |
| Parse16KB |   663.23 us |  6.5203 us |  5.0906 us |   653.41 us |   671.02 us |   663.38 us |      - |   2.31 KB |
| Parse32KB | 1,381.74 us | 12.6144 us | 11.7995 us | 1,358.22 us | 1,406.69 us | 1,383.30 us |      - |   3.55 KB |
