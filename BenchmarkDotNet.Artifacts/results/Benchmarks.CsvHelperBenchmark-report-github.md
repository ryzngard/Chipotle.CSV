``` ini

BenchmarkDotNet=v0.11.0, OS=Windows 10.0.17134.191 (1803/April2018Update/Redstone4)
Intel Core i7-8650U CPU 1.90GHz (Max: 1.91GHz) (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.302
  [Host]     : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT


```
|    Method |       Mean |     Error |    StdDev |     Median |        Min |        Max |    Gen 0 | Allocated |
|---------- |-----------:|----------:|----------:|-----------:|-----------:|-----------:|---------:|----------:|
|  Parse1KB |         NA |        NA |        NA |         NA |         NA |         NA |      N/A |       N/A |
|  Parse4KB |   318.7 us | 23.257 us | 68.575 us |   357.7 us |   179.8 us |   388.6 us |  21.4844 |   91024 B |
|  Parse8KB |   826.8 us |  9.421 us |  8.812 us |   829.2 us |   797.7 us |   833.6 us |  43.9453 |  188163 B |
| Parse16KB |   972.2 us | 19.412 us | 34.505 us |   974.6 us |   919.5 us | 1,031.8 us |  83.9844 |  355093 B |
| Parse32KB | 2,002.2 us | 39.491 us | 84.159 us | 2,014.1 us | 1,889.2 us | 2,217.9 us | 167.9688 |  716250 B |

Benchmarks with issues:
  CsvHelperBenchmark.Parse1KB: DefaultJob
