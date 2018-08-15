``` ini

BenchmarkDotNet=v0.11.0, OS=Windows 10.0.17134.191 (1803/April2018Update/Redstone4)
Intel Core i7-8650U CPU 1.90GHz (Max: 1.91GHz) (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.302
  [Host]     : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT


```
|        Method |       method |     Mean |    Error |   StdDev |      Min |      Max |   Median |      Gen 0 |     Gen 1 |     Gen 2 |  Allocated |
|-------------- |------------- |---------:|---------:|---------:|---------:|---------:|---------:|-----------:|----------:|----------:|-----------:|
| **Find_Last_4MB** | **BytePipeline** |       **NA** |       **NA** |       **NA** |       **NA** |       **NA** |       **NA** |        **N/A** |       **N/A** |       **N/A** |        **N/A** |
| **Find_Last_4MB** |    **CsvHelper** | **137.5 ms** | **1.341 ms** | **1.047 ms** | **135.7 ms** | **139.9 ms** | **137.4 ms** | **23500.0000** |         **-** |         **-** | **99561358 B** |
| **Find_Last_4MB** |   **MemoryPool** |       **NA** |       **NA** |       **NA** |       **NA** |       **NA** |       **NA** |        **N/A** |       **N/A** |       **N/A** |        **N/A** |
| **Find_Last_4MB** |          **SMP** | **189.3 ms** | **3.768 ms** | **5.523 ms** | **174.8 ms** | **197.7 ms** | **191.2 ms** | **15000.0000** | **3333.3333** | **1666.6667** | **57659229 B** |

Benchmarks with issues:
  CsvBenchmarks.Find_Last_4MB: DefaultJob [method=BytePipeline]
  CsvBenchmarks.Find_Last_4MB: DefaultJob [method=MemoryPool]
