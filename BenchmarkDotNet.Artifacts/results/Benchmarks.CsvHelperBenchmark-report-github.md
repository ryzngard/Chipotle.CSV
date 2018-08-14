``` ini

BenchmarkDotNet=v0.11.0, OS=Windows 10.0.17134.191 (1803/April2018Update/Redstone4)
Intel Core i7-8650U CPU 1.90GHz (Max: 1.91GHz) (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.302
  [Host]     : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT


```
|       Method |          Mean |         Error |       StdDev |        Median |           Min |           Max |      Gen 0 |   Allocated |
|------------- |--------------:|--------------:|-------------:|--------------:|--------------:|--------------:|-----------:|------------:|
|     Parse2KB |      46.07 us |     0.9755 us |     2.162 us |      45.16 us |      44.30 us |      52.87 us |     9.0332 |    37.04 KB |
|     Parse4KB |     106.02 us |     1.2670 us |     1.185 us |     105.81 us |     104.39 us |     108.10 us |    17.3340 |    71.34 KB |
|     Parse8KB |     240.17 us |     3.5748 us |     3.344 us |     239.97 us |     235.42 us |     246.89 us |    35.1563 |   145.05 KB |
|    Parse16KB |     470.42 us |     7.2070 us |     6.741 us |     469.13 us |     461.29 us |     481.92 us |    66.4063 |   272.26 KB |
|    Parse32KB |     965.74 us |     9.7597 us |     8.150 us |     969.54 us |     951.10 us |     978.40 us |   132.8125 |   546.75 KB |
|     Parse4MB | 164,011.38 us | 1,533.8173 us | 1,359.689 us | 164,037.98 us | 161,092.53 us | 166,912.40 us | 23500.0000 | 97227.85 KB |
| FindLast_4MB | 160,262.97 us | 1,733.9708 us | 1,537.120 us | 160,126.83 us | 157,256.23 us | 163,068.75 us | 23500.0000 | 97227.85 KB |
