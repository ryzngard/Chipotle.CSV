``` ini

BenchmarkDotNet=v0.11.0, OS=Windows 10.0.17134.191 (1803/April2018Update/Redstone4)
Intel Core i7-8650U CPU 1.90GHz (Max: 1.91GHz) (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.302
  [Host]     : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT


```
|       Method |     Mean |    Error |   StdDev |      Min |      Max |   Median |      Gen 0 |     Gen 1 |     Gen 2 | Allocated |
|------------- |---------:|---------:|---------:|---------:|---------:|---------:|-----------:|----------:|----------:|----------:|
|     Parse4MB | 636.8 ms | 12.73 ms | 28.48 ms | 600.6 ms | 705.6 ms | 629.2 ms |  6000.0000 | 4000.0000 | 4000.0000 |   2.52 MB |
| FindLast_4MB | 716.4 ms | 14.28 ms | 28.86 ms | 665.4 ms | 780.2 ms | 718.3 ms | 17000.0000 | 5000.0000 | 3000.0000 |  54.99 MB |
