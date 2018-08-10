``` ini

BenchmarkDotNet=v0.11.0, OS=Windows 10.0.17134.191 (1803/April2018Update/Redstone4)
Intel Core i7-8650U CPU 1.90GHz (Max: 1.91GHz) (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.302
  [Host]     : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT


```
|    Method |        Mean |      Error |     StdDev |         Min |         Max |      Median |    Gen 0 | Allocated |
|---------- |------------:|-----------:|-----------:|------------:|------------:|------------:|---------:|----------:|
|  Parse2KB |    61.44 us |  0.6633 us |  0.6205 us |    60.21 us |    62.42 us |    61.40 us |  10.9253 |   44.8 KB |
|  Parse4KB |   149.14 us |  2.8126 us |  3.0094 us |   145.04 us |   154.59 us |   149.21 us |  21.4844 |  88.89 KB |
|  Parse8KB |   330.61 us |  5.8981 us |  5.5171 us |   322.46 us |   338.35 us |   329.52 us |  44.4336 | 183.76 KB |
| Parse16KB |   631.16 us |  8.3181 us |  7.7807 us |   618.67 us |   643.28 us |   631.08 us |  83.9844 | 346.69 KB |
| Parse32KB | 1,301.76 us | 18.0944 us | 16.9255 us | 1,275.93 us | 1,328.97 us | 1,299.57 us | 169.9219 | 699.44 KB |
