``` ini

BenchmarkDotNet=v0.11.0, OS=Windows 10.0.17134.191 (1803/April2018Update/Redstone4)
Intel Core i7-8650U CPU 1.90GHz (Max: 1.41GHz) (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.302
  [Host]     : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT
  Job-KOQYNN : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT

InvocationCount=1  UnrollFactor=1  

```
|                        Method |     Mean |     Error |    StdDev |      Min |      Max |   Median |      Gen 0 |      Gen 1 |      Gen 2 |   Allocated |
|------------------------------ |---------:|----------:|----------:|---------:|---------:|---------:|-----------:|-----------:|-----------:|------------:|
| DefaultLineParser_Streamed_1k |       NA |        NA |        NA |       NA |       NA |       NA |        N/A |        N/A |        N/A |         N/A |
|  DefaultLineParser_UpFront_1k |       NA |        NA |        NA |       NA |       NA |       NA |        N/A |        N/A |        N/A |         N/A |
|   StreamLineParser_1k_Chunk10 | 513.7 ms | 25.812 ms | 76.108 ms | 370.3 ms | 740.1 ms | 501.5 ms | 21000.0000 | 20000.0000 | 19000.0000 | 263015872 B |
|  StreamLineParser_1k_Chunk100 | 343.3 ms |  1.178 ms |  1.044 ms | 342.0 ms | 345.6 ms | 343.2 ms |  4000.0000 |  3000.0000 |  3000.0000 |  43052392 B |
| StreamLineParser_1k_Chunk1000 | 317.5 ms |  5.422 ms |  5.072 ms | 307.6 ms | 323.9 ms | 317.6 ms |  2000.0000 |  1000.0000 |  1000.0000 |  20055680 B |

Benchmarks with issues:
  LineParserTests.DefaultLineParser_Streamed_1k: Job-KOQYNN(InvocationCount=1, UnrollFactor=1)
  LineParserTests.DefaultLineParser_UpFront_1k: Job-KOQYNN(InvocationCount=1, UnrollFactor=1)
