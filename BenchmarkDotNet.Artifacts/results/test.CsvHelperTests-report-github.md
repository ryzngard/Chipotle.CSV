``` ini

BenchmarkDotNet=v0.11.0, OS=Windows 10.0.17134.191 (1803/April2018Update/Redstone4)
Intel Core i7-8650U CPU 1.90GHz (Max: 1.91GHz) (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.302
  [Host]     : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT


```
|    Method |        Mean |      Error |     StdDev |         Min |         Max |      Median |    Gen 0 | Allocated |
|---------- |------------:|-----------:|-----------:|------------:|------------:|------------:|---------:|----------:|
|  Parse2KB |    62.88 us |  0.9359 us |  0.8296 us |    61.61 us |    64.77 us |    62.83 us |  10.8643 |   44.8 KB |
|  Parse4KB |   150.45 us |  1.7617 us |  1.6479 us |   148.26 us |   153.83 us |   150.15 us |  21.4844 |  88.89 KB |
|  Parse8KB |   339.24 us |  6.7532 us |  7.5061 us |   329.48 us |   357.64 us |   338.40 us |  44.4336 | 183.76 KB |
| Parse16KB |   654.56 us |  9.4596 us |  8.8485 us |   638.00 us |   668.51 us |   656.15 us |  83.9844 | 346.69 KB |
| Parse32KB | 1,319.44 us | 20.5462 us | 19.2189 us | 1,279.99 us | 1,347.04 us | 1,318.02 us | 169.9219 | 699.44 KB |
