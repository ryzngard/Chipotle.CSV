``` ini

BenchmarkDotNet=v0.11.0, OS=Windows 10.0.17134.191 (1803/April2018Update/Redstone4)
Intel Core i7-8650U CPU 1.90GHz (Max: 1.91GHz) (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.302
  [Host]     : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT


```
|       Method |     Mean |    Error |   StdDev |   Median |      Min |      Max |      Gen 0 | Allocated |
|------------- |---------:|---------:|---------:|---------:|---------:|---------:|-----------:|----------:|
| FindLast_4MB | 290.6 ms | 16.83 ms | 49.35 ms | 268.9 ms | 240.4 ms | 415.3 ms | 35000.0000 | 128.48 MB |
