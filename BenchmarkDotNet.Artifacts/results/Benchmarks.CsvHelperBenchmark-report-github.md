``` ini

BenchmarkDotNet=v0.11.0, OS=Windows 10.0.17134.191 (1803/April2018Update/Redstone4)
Intel Core i7-8650U CPU 1.90GHz (Max: 1.91GHz) (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.302
  [Host]     : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT


```
|    Method |        Mean |     Error |     StdDev |      Median |         Min |         Max |    Gen 0 | Allocated |
|---------- |------------:|----------:|-----------:|------------:|------------:|------------:|---------:|----------:|
|  Parse2KB |    89.79 us |  1.703 us |   1.822 us |    88.79 us |    87.97 us |    92.83 us |  10.8643 |   44.8 KB |
|  Parse4KB |   216.62 us |  4.264 us |   7.355 us |   215.37 us |   203.42 us |   232.57 us |  21.4844 |  88.89 KB |
|  Parse8KB |   676.79 us | 55.239 us | 162.873 us |   775.10 us |   435.14 us |   885.90 us |  44.4336 | 183.75 KB |
| Parse16KB |   980.47 us | 19.595 us |  42.598 us |   963.66 us |   921.45 us | 1,097.82 us |  83.9844 | 346.76 KB |
| Parse32KB | 1,942.80 us | 38.067 us |  49.498 us | 1,958.30 us | 1,852.31 us | 2,011.50 us | 169.9219 | 699.45 KB |
