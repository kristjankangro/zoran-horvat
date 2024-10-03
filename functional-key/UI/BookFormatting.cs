namespace Demo.UI;

using Demo.Models;
using Demo.Processes;

public static class BookFormatting
{
    public static FormatBookExt NamesThenTitle => (namesFormatter, book) =>
        book.Authors.Length == 0 ? $"{book.Title}"
        : $"{namesFormatter(book.Authors)}, {book.Title}";

}
