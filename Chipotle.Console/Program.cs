using Chipotle.CSV;

using var stream = new MemoryStream();
using var tokenizer = new MemoryManagedTokenizer(stream);
while (true)
{
    var input = Console.ReadLine();
    if (input?.Equals("q") == true)
    {
        break;
    }

    var section = await tokenizer.GetNextAsync();
    foreach (var item in section)
    {
        Console.WriteLine(item);
    }
}