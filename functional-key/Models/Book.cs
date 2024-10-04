namespace Demo.Models;

public class BookType(string Title, NameType[] Authors)
{
    public override string ToString() =>
        $"{Title} by {string.Join(", ", Authors.Select(a => a.ToString()))}";

    public string Title { get; init; } = Title;
    public NameType[] Authors { get; init; } = Authors;

    public void Deconstruct(out string Title, out NameType[] Authors)
    {
        Title = this.Title;
        Authors = this.Authors;
    }
}

public static class Book
{
    public static BookType? Create(string title, params NameType[] authors) =>
        string.IsNullOrWhiteSpace(title) ? null : new(title, authors);
}