``` ini

BenchmarkDotNet=v0.11.0, OS=Windows 10.0.17134.191 (1803/April2018Update/Redstone4)
Intel Core i7-8650U CPU 1.90GHz (Max: 1.91GHz) (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.302
  [Host]     : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT


```
|    Method |       Mean |     Error |    StdDev |     Median |         Min |        Max |    Gen 0 | Allocated |
|---------- |-----------:|----------:|----------:|-----------:|------------:|-----------:|---------:|----------:|
|  Parse2KB |   126.3 us | 11.266 us | 33.217 us |   100.5 us |    93.40 us |   168.1 us |   7.8125 |   32.1 KB |
|  Parse4KB |   290.1 us |  5.769 us |  9.797 us |   283.7 us |   280.66 us |   323.1 us |  22.9492 |  94.59 KB |
|  Parse8KB |   712.1 us | 14.166 us | 21.203 us |   716.8 us |   683.69 us |   751.7 us |  55.6641 | 230.25 KB |
| Parse16KB | 1,419.1 us | 28.217 us | 43.931 us | 1,413.4 us | 1,363.33 us | 1,524.8 us | 111.3281 | 459.29 KB |
| Parse32KB | 2,935.5 us | 58.664 us | 82.239 us | 2,923.2 us | 2,772.34 us | 3,110.7 us | 230.4688 | 959.41 KB |
