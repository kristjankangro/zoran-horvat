using Bookstore.Models;

namespace Bookstore.ViewModels;

public static class BookHeaderExtensions
{
    public static BookHeader ToBookHeader(this Book book) =>
        new(book.Title, book.Author.ToBookHeaderAuthorName());

    private static string ToBookHeaderAuthorName(this Person? author) =>
        author is null ? string.Empty
        : author.LastName is null ? author.FirstName
        : $"{author.FirstName} {author.LastName}";

    public static IEnumerable<BookHeader> ToBookHeaders(this IEnumerable<Book> books) =>
        books.Select(ToBookHeader);
}

