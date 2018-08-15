``` ini

BenchmarkDotNet=v0.11.0, OS=Windows 10.0.17134.191 (1803/April2018Update/Redstone4)
Intel Core i7-8650U CPU 1.90GHz (Max: 1.91GHz) (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.302
  [Host]     : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT


```
|       Method |     Mean |    Error |   StdDev |   Median |      Min |      Max |      Gen 0 | Allocated |
|------------- |---------:|---------:|---------:|---------:|---------:|---------:|-----------:|----------:|
|     Parse4MB | 339.8 ms | 13.46 ms | 37.96 ms | 338.2 ms | 266.9 ms | 467.0 ms | 40000.0000 |   2.52 MB |
| FindLast_4MB | 301.7 ms | 19.51 ms | 57.54 ms | 274.4 ms | 242.2 ms | 448.0 ms | 34000.0000 | 124.57 MB |
