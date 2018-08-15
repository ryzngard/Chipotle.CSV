``` ini

BenchmarkDotNet=v0.11.0, OS=Windows 10.0.17134.191 (1803/April2018Update/Redstone4)
Intel Core i7-8650U CPU 1.90GHz (Max: 1.91GHz) (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.302
  [Host]     : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT


```
|    Method |        Mean |      Error |     StdDev |         Min |         Max |      Median |    Gen 0 | Allocated |
|---------- |------------:|-----------:|-----------:|------------:|------------:|------------:|---------:|----------:|
|  Parse2KB |    60.31 us |  0.8294 us |  0.7758 us |    59.44 us |    62.12 us |    60.17 us |   6.9580 |   29512 B |
|  Parse4KB |   174.48 us |  0.5148 us |  0.4815 us |   173.86 us |   175.35 us |   174.39 us |  20.5078 |   86752 B |
|  Parse8KB |   427.04 us |  2.8650 us |  2.6799 us |   423.16 us |   433.04 us |   426.32 us |  50.2930 |  211064 B |
| Parse16KB |   850.77 us |  2.0529 us |  1.8199 us |   846.24 us |   854.10 us |   851.02 us |  99.6094 |  420864 B |
| Parse32KB | 1,780.80 us | 10.8948 us | 10.1910 us | 1,766.01 us | 1,797.13 us | 1,777.89 us | 208.9844 |  879016 B |
|  Parse4MB |          NA |         NA |         NA |          NA |          NA |          NA |      N/A |       N/A |

Benchmarks with issues:
  ChipotleCsvBytePipelineBenchmark.Parse4MB: DefaultJob
