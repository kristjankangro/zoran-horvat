using System.Globalization;

namespace Demo.Models;

public class Book
{
    public AuthorsList Authors { get; }
    private string _title = string.Empty; //primitive type
    private readonly Release _release;

    public string Title
    {
        get => _title;
        set => _title = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException(nameof(Title));
    }

    public Book(string title, AuthorsList authors, Release release)
    {
        (Title, Authors, _release) = (title, authors, release);
    }
}