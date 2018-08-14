``` ini

BenchmarkDotNet=v0.11.0, OS=Windows 10.0.17134.191 (1803/April2018Update/Redstone4)
Intel Core i7-8650U CPU 1.90GHz (Max: 1.91GHz) (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.302
  [Host]     : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT


```
|    Method |        Mean |      Error |      StdDev |      Median |         Min |         Max |    Gen 0 | Allocated |
|---------- |------------:|-----------:|------------:|------------:|------------:|------------:|---------:|----------:|
|  Parse2KB |    54.79 us |  0.3734 us |   0.3493 us |    54.66 us |    54.35 us |    55.61 us |   7.0190 |   29512 B |
|  Parse4KB |   159.18 us |  2.0107 us |   1.7824 us |   158.46 us |   157.34 us |   163.59 us |  20.5078 |   86752 B |
|  Parse8KB |   388.07 us |  9.3407 us |  11.1194 us |   385.51 us |   378.73 us |   426.56 us |  49.8047 |  211064 B |
| Parse16KB |   854.54 us | 18.6744 us |  27.9509 us |   859.15 us |   797.97 us |   927.45 us |  99.6094 |  420864 B |
| Parse32KB | 2,012.68 us | 57.9204 us | 165.2501 us | 1,915.57 us | 1,822.28 us | 2,480.52 us | 208.9844 |  879016 B |
|  Parse4MB |          NA |         NA |          NA |          NA |          NA |          NA |      N/A |       N/A |

Benchmarks with issues:
  ChipotleCsvBytePipelineBenchmark.Parse4MB: DefaultJob
