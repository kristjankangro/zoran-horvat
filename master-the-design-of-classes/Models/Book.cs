using System.Globalization;

namespace Demo.Models;

public class Book
{
    public AuthorsList Authors;
    private string _title = string.Empty; //primitive type
    private string _publisher = string.Empty; //primitive type
    private int _edition; //primitive type

    public string Title
    {
        get => _title;
        set => _title = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException(nameof(Title));
    }


    public string Publisher //primitive type
    {
        get => _publisher;
        set => _publisher = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException(nameof(Publisher));
    }

    public int Edition //primitive type
    {
        get => _edition;
        set => _edition = value > 0 ? value : throw new ArgumentOutOfRangeException(nameof(Edition));
    }

    public DateOnly PublicationDate { get; set; }

    public CultureInfo Culture { get; set; }

    public Book(string title, AuthorsList authors, string publisher, int edition,
        DateOnly publicationDate)
    {
        Authors = authors;
        (Title, Authors, Publisher, Edition, PublicationDate) =
            (title, authors, publisher, edition, publicationDate);
    }
}