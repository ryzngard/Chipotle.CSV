``` ini

BenchmarkDotNet=v0.11.0, OS=Windows 10.0.17134.191 (1803/April2018Update/Redstone4)
Intel Core i7-8650U CPU 1.90GHz (Max: 1.91GHz) (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.302
  [Host]     : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT


```
|    Method |          Mean |         Error |        StdDev |           Min |           Max |        Median |      Gen 0 |   Allocated |
|---------- |--------------:|--------------:|--------------:|--------------:|--------------:|--------------:|-----------:|------------:|
|  Parse2KB |      40.96 us |     0.6510 us |     0.6090 us |      39.87 us |      41.88 us |      40.87 us |     9.0332 |    37.04 KB |
|  Parse4KB |      93.03 us |     1.8165 us |     1.6992 us |      90.45 us |      95.71 us |      92.94 us |    17.3340 |    71.34 KB |
|  Parse8KB |     206.33 us |     1.5851 us |     1.3236 us |     202.94 us |     208.15 us |     206.78 us |    35.4004 |   145.05 KB |
| Parse16KB |     430.60 us |     8.5571 us |     8.0043 us |     413.68 us |     442.48 us |     431.76 us |    66.4063 |   272.26 KB |
| Parse32KB |     878.42 us |    13.8317 us |    12.9382 us |     857.26 us |     892.68 us |     884.30 us |   132.8125 |   546.75 KB |
|  Parse4MB | 151,885.42 us | 2,214.8690 us | 2,071.7897 us | 147,926.05 us | 155,547.65 us | 151,486.78 us | 23500.0000 | 97227.88 KB |
