using Demo.Data;
using static Demo.Data.BooksData;
using static Demo.Data.BooksFiltering;

await Runner.PromptAndReport(FilterBooks.For(GetBooks));