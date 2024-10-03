namespace Demo.Models;

public record BookType(string Title, NameType[] Authors)
{
    public override string ToString() =>
        $"{Title} by {string.Join(", ", Authors.Select(a => a.ToString()))}";
}

public static class Book
{
    public static BookType? Create(string title, params NameType[] authors) =>
        string.IsNullOrWhiteSpace(title) ? null : new(title, authors);
}