using Demo.Common;
using Demo.Models;
using Demo.Data;

using static Demo.Data.BooksData;
using static Demo.Data.BooksFiltering;

await PromptAndReport(FilterBooks.For(GetBooks));

async Task PromptAndReport(FilteredDataSource<BookType> dataSource)
{
    Console.Write("Enter search phrase: ");
    string phrase = Console.ReadLine() ?? string.Empty;
    if (string.IsNullOrWhiteSpace(phrase)) return;

    (await dataSource(phrase)).ForEach(Console.WriteLine);
}
