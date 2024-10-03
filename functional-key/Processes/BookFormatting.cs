namespace Demo.Processes;

using Demo.Models;

public delegate string FormatBook(BookType book);

public delegate string FormatBookExt(NameListFormatter namesFormatter, BookType book);

public static class BookFormatters
{
    public static FormatBook Apply(this FormatBookExt formatter, NameListFormatter namesFormatter) =>
        book => formatter(namesFormatter, book);
}