namespace Demo.Models;

public class Book
{
    private string _title = string.Empty;
    private string _publisher = string.Empty;
    private int _edition;

    public string Title
    {
        get => _title;
        set => _title = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException(nameof(Title));
    }

    public IEnumerable<string> Authors => AuthorsCollection;
    private List<string> AuthorsCollection { get; set; }

    public string Publisher
    {
        get => _publisher;
        set => _publisher = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException(nameof(Publisher));
    }

    public int Edition
    {
        get => _edition;
        set => _edition = value > 0 ? value : throw new ArgumentOutOfRangeException(nameof(Edition));
    }

    public DateOnly PublicationDate { get; set; }

    public Book(string title, IEnumerable<string> authors, string publisher, int edition, DateOnly publicationDate) =>
        (Title, AuthorsCollection, Publisher, Edition, PublicationDate) =
        (title, authors.Where(IsValidAuthor).ToList(), publisher, edition, publicationDate);

    private bool IsValidAuthor(string author)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(author, nameof(author));
        return true;
    }
}

