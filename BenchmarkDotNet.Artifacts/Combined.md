``` ini

BenchmarkDotNet=v0.11.0, OS=Windows 10.0.17134.191 (1803/April2018Update/Redstone4)
Intel Core i7-8650U CPU 1.90GHz (Max: 1.91GHz) (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.302
  [Host]     : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT


```

# Chipotle CSV

## Byte Memory Pool

|    Method |          Mean |         Error |        StdDev |        Median |           Min |           Max |      Gen 0 |  Allocated |
|---------- |--------------:|--------------:|--------------:|--------------:|--------------:|--------------:|-----------:|-----------:|
|  Parse2KB |      74.41 us |      1.485 us |      4.355 us |      75.00 us |      61.58 us |      84.39 us |    13.1836 |    2.16 KB |
|  Parse4KB |     239.22 us |      9.180 us |     27.069 us |     230.20 us |     194.53 us |     290.14 us |    38.8184 |    3.85 KB |
|  Parse8KB |     547.75 us |     10.829 us |     28.904 us |     547.57 us |     486.51 us |     604.92 us |    94.7266 |    7.51 KB |
| Parse16KB |   1,143.40 us |     33.230 us |     97.979 us |   1,146.29 us |     929.95 us |   1,354.67 us |   188.4766 |    13.7 KB |
| Parse32KB |   2,321.62 us |     73.705 us |    217.322 us |   2,313.89 us |   1,914.36 us |   2,925.14 us |   394.5313 |   27.34 KB |
|  Parse4MB | 352,170.31 us | 21,830.107 us | 62,282.523 us | 334,005.25 us | 268,142.90 us | 527,612.90 us | 70000.0000 | 5153.17 KB |

## Byte Pipeline

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

# CsvHelper 

|    Method |          Mean |         Error |        StdDev |           Min |           Max |        Median |      Gen 0 |   Allocated |
|---------- |--------------:|--------------:|--------------:|--------------:|--------------:|--------------:|-----------:|------------:|
|  Parse2KB |      40.96 us |     0.6510 us |     0.6090 us |      39.87 us |      41.88 us |      40.87 us |     9.0332 |    37.04 KB |
|  Parse4KB |      93.03 us |     1.8165 us |     1.6992 us |      90.45 us |      95.71 us |      92.94 us |    17.3340 |    71.34 KB |
|  Parse8KB |     206.33 us |     1.5851 us |     1.3236 us |     202.94 us |     208.15 us |     206.78 us |    35.4004 |   145.05 KB |
| Parse16KB |     430.60 us |     8.5571 us |     8.0043 us |     413.68 us |     442.48 us |     431.76 us |    66.4063 |   272.26 KB |
| Parse32KB |     878.42 us |    13.8317 us |    12.9382 us |     857.26 us |     892.68 us |     884.30 us |   132.8125 |   546.75 KB |
|  Parse4MB | 151,885.42 us | 2,214.8690 us | 2,071.7897 us | 147,926.05 us | 155,547.65 us | 151,486.78 us | 23500.0000 | 97227.88 KB |
