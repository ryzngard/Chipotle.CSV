``` ini

BenchmarkDotNet=v0.11.0, OS=Windows 10.0.17134.191 (1803/April2018Update/Redstone4)
Intel Core i7-8650U CPU 1.90GHz (Max: 1.91GHz) (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.302
  [Host]     : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT


```
|       Method |          Mean |         Error |        StdDev |        Median |           Min |          Max |      Gen 0 |   Allocated |
|------------- |--------------:|--------------:|--------------:|--------------:|--------------:|-------------:|-----------:|------------:|
|     Parse2KB |      88.10 us |      1.860 us |      5.483 us |      87.60 us |      78.61 us |     101.4 us |     7.0801 |     1.83 KB |
|     Parse4KB |     246.45 us |      4.885 us |     13.936 us |     244.63 us |     214.66 us |     283.1 us |    20.0195 |     2.67 KB |
|     Parse8KB |     577.29 us |     15.713 us |     45.835 us |     574.24 us |     468.97 us |     684.0 us |    47.8516 |      4.5 KB |
|    Parse16KB |   1,315.66 us |     32.885 us |     96.447 us |   1,305.71 us |   1,101.97 us |   1,576.5 us |    95.7031 |     7.59 KB |
|    Parse32KB |   2,684.44 us |     60.832 us |    177.451 us |   2,654.04 us |   2,340.89 us |   3,102.8 us |   199.2188 |    14.41 KB |
|     Parse4MB | 373,466.14 us | 19,899.956 us | 54,810.257 us | 363,443.55 us | 268,461.00 us | 518,044.9 us | 40000.0000 |  2577.33 KB |
| FindLast_4MB | 297,241.20 us | 18,463.347 us | 54,439.595 us | 271,198.45 us | 236,673.80 us | 464,998.5 us | 34000.0000 | 127560.3 KB |
