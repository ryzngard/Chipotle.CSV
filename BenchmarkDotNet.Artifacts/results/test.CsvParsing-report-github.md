``` ini

BenchmarkDotNet=v0.11.0, OS=Windows 10.0.17134.191 (1803/April2018Update/Redstone4)
Intel Core i7-8650U CPU 1.90GHz (Max: 1.91GHz) (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.302
  [Host]     : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT


```
|    Method |        Mean |     Error |    StdDev |         Min |         Max |      Median |  Gen 0 | Allocated |
|---------- |------------:|----------:|----------:|------------:|------------:|------------:|-------:|----------:|
|  Parse2KB |    44.11 us | 0.3278 us | 0.3066 us |    43.79 us |    44.66 us |    43.98 us | 0.3052 |   1.33 KB |
|  Parse4KB |   130.07 us | 1.4911 us | 1.2451 us |   128.21 us |   132.38 us |   130.06 us | 0.2441 |   1.47 KB |
|  Parse8KB |   318.22 us | 3.2933 us | 2.9194 us |   314.15 us |   324.53 us |   317.70 us |      - |   1.87 KB |
| Parse16KB |   633.21 us | 8.4734 us | 7.5115 us |   620.39 us |   651.51 us |   631.44 us |      - |   2.31 KB |
| Parse32KB | 1,320.97 us | 7.6220 us | 6.7567 us | 1,308.84 us | 1,334.00 us | 1,321.37 us |      - |   3.55 KB |
