# Chipotle.CSV
A csv parser written while eating a burrito. Meant to be lean as possible. A project to try out the new System.IO.Pipelines and Memory<T>/Span<T> features for dotnet

## Usage 

Just run `dotnet build` in the root directory. 

## Benchmarks

All benchmarks are made with Benchmark.NET run with `dotnet run -c release --filter *KB* --project .\test\test.csproj`
