``` ini

BenchmarkDotNet=v0.11.0, OS=Windows 10.0.17134.191 (1803/April2018Update/Redstone4)
Intel Core i7-8650U CPU 1.90GHz (Max: 1.91GHz) (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.302
  [Host]     : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT


```
|   Method |      Mean |     Error |     StdDev |       Min |       Max |    Median |  Gen 0 | Allocated |
|--------- |----------:|----------:|-----------:|----------:|----------:|----------:|-------:|----------:|
| Parse2KB |  44.56 us | 0.7153 us |  0.5585 us |  43.91 us |  45.54 us |  44.42 us | 0.3052 |   1.33 KB |
| Parse4KB | 131.44 us | 1.3865 us |  1.0825 us | 129.48 us | 132.64 us | 131.26 us | 0.2441 |   1.47 KB |
| Parse8KB | 330.94 us | 6.7727 us | 11.5006 us | 317.31 us | 363.97 us | 326.72 us |      - |   1.87 KB |
