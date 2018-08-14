``` ini

BenchmarkDotNet=v0.11.0, OS=Windows 10.0.17134.191 (1803/April2018Update/Redstone4)
Intel Core i7-8650U CPU 1.90GHz (Max: 1.91GHz) (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.302
  [Host]     : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT


```
|       Method |         Mean |         Error |        StdDev |       Median |           Min |          Max |      Gen 0 |   Allocated |
|------------- |-------------:|--------------:|--------------:|-------------:|--------------:|-------------:|-----------:|------------:|
|     Parse2KB |     109.5 us |      2.237 us |      6.527 us |     109.4 us |      95.81 us |     126.5 us |     7.0801 |     1.83 KB |
|     Parse4KB |     302.0 us |      8.733 us |     25.335 us |     299.0 us |     246.24 us |     368.9 us |    20.0195 |     2.67 KB |
|     Parse8KB |     643.5 us |     15.602 us |     45.265 us |     645.3 us |     540.28 us |     749.8 us |    47.8516 |      4.5 KB |
|    Parse16KB |   1,256.4 us |     28.570 us |     82.887 us |   1,254.4 us |   1,057.42 us |   1,476.0 us |    95.7031 |     7.59 KB |
|    Parse32KB |   2,501.4 us |     71.993 us |    212.273 us |   2,492.4 us |   2,122.25 us |   3,051.7 us |   199.2188 |    14.41 KB |
|     Parse4MB | 316,286.1 us | 13,584.932 us | 36,727.654 us | 310,665.7 us | 254,320.00 us | 438,991.2 us | 40000.0000 |  2577.33 KB |
| FindLast_4MB | 246,885.0 us | 13,474.472 us | 39,729.784 us | 228,286.0 us | 205,929.40 us | 341,081.8 us | 34000.0000 | 127560.3 KB |
