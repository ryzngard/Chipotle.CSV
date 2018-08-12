``` ini

BenchmarkDotNet=v0.11.0, OS=Windows 10.0.17134.191 (1803/April2018Update/Redstone4)
Intel Core i7-8650U CPU 1.90GHz (Max: 1.91GHz) (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.302
  [Host]     : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT


```
|    Method |          Mean |         Error |        StdDev |        Median |           Min |           Max |      Gen 0 |  Allocated |
|---------- |--------------:|--------------:|--------------:|--------------:|--------------:|--------------:|-----------:|-----------:|
|  Parse2KB |      74.41 us |      1.485 us |      4.355 us |      75.00 us |      61.58 us |      84.39 us |    13.1836 |    2.16 KB |
|  Parse4KB |     239.22 us |      9.180 us |     27.069 us |     230.20 us |     194.53 us |     290.14 us |    38.8184 |    3.85 KB |
|  Parse8KB |     547.75 us |     10.829 us |     28.904 us |     547.57 us |     486.51 us |     604.92 us |    94.7266 |    7.51 KB |
| Parse16KB |   1,143.40 us |     33.230 us |     97.979 us |   1,146.29 us |     929.95 us |   1,354.67 us |   188.4766 |    13.7 KB |
| Parse32KB |   2,321.62 us |     73.705 us |    217.322 us |   2,313.89 us |   1,914.36 us |   2,925.14 us |   394.5313 |   27.34 KB |
|  Parse4MB | 352,170.31 us | 21,830.107 us | 62,282.523 us | 334,005.25 us | 268,142.90 us | 527,612.90 us | 70000.0000 | 5153.17 KB |
