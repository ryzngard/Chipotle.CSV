``` ini

BenchmarkDotNet=v0.11.0, OS=Windows 10.0.17134.191 (1803/April2018Update/Redstone4)
Intel Core i7-8650U CPU 1.90GHz (Max: 1.91GHz) (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.302
  [Host]     : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT


```
|    Method |      Mean |     Error |    StdDev |       Min |      Max |    Median |   Gen 0 | Allocated |
|---------- |----------:|----------:|----------:|----------:|---------:|----------:|--------:|----------:|
|  Parse2KB |  96.22 us |  1.919 us |  2.690 us |  90.79 us | 100.9 us |  95.48 us |  7.8125 |   32872 B |
|  Parse4KB | 295.44 us |  6.324 us |  9.657 us | 277.94 us | 323.8 us | 297.78 us | 22.9492 |   96856 B |
|  Parse8KB | 687.95 us | 13.731 us | 29.557 us | 609.81 us | 756.7 us | 687.62 us | 55.6641 |  235776 B |
| Parse16KB |        NA |        NA |        NA |        NA |       NA |        NA |     N/A |       N/A |
| Parse32KB |        NA |        NA |        NA |        NA |       NA |        NA |     N/A |       N/A |

Benchmarks with issues:
  ChipotleCsvBenchmark.Parse16KB: DefaultJob
  ChipotleCsvBenchmark.Parse32KB: DefaultJob
