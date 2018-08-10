``` ini

BenchmarkDotNet=v0.11.0, OS=Windows 10.0.17134.191 (1803/April2018Update/Redstone4)
Intel Core i7-8650U CPU 1.90GHz (Max: 1.41GHz) (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.302
  [Host]     : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT


```
|    Method |        Mean |      Error |     StdDev |      Median |         Min |        Max |    Gen 0 | Allocated |
|---------- |------------:|-----------:|-----------:|------------:|------------:|-----------:|---------:|----------:|
|  Parse2KB |    97.19 us |   2.272 us |   6.519 us |    98.83 us |    63.49 us |   106.0 us |   7.8125 |   32.1 KB |
|  Parse4KB |   293.27 us |   5.777 us |   9.810 us |   292.82 us |   278.50 us |   319.7 us |  22.9492 |  94.59 KB |
|  Parse8KB |   715.99 us |  13.930 us |  21.273 us |   725.91 us |   685.69 us |   768.9 us |  55.6641 | 230.25 KB |
| Parse16KB | 2,071.54 us | 140.994 us | 415.723 us | 2,307.00 us |   932.23 us | 2,544.7 us | 111.3281 | 459.29 KB |
| Parse32KB | 3,427.87 us | 284.761 us | 839.623 us | 3,019.58 us | 1,868.55 us | 4,924.7 us | 230.4688 | 959.41 KB |
