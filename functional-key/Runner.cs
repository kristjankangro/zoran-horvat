using Demo.Common;
using Demo.Models;

internal class Runner
{
    public static async Task PromptAndReport(FilteredDataSource<BookType> dataSource)
    {
        Console.Write("Enter search phrase: ");
        string phrase = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(phrase)) return;

        (await dataSource(phrase)).ForEach(Console.WriteLine);
    }
}