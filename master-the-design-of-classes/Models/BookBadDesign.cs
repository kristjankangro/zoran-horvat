namespace Demo.Models;

public class BookBadDesign
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

    public BookBadDesign(string title, IEnumerable<string> authors, string publisher, int edition,
        DateOnly publicationDate) =>
        (Title, AuthorsCollection, Publisher, Edition, PublicationDate) =
        (title, authors.Where(IsValidAuthor).ToList(), publisher, edition, publicationDate);

    private bool IsValidAuthor(string author)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(author, nameof(author));
        return true;
    }

    public void AppendAuthor(string author)
    {
        IsValidAuthor(author);
        AuthorsCollection.Add(author);
    }

    public void RemoveAuthor(string author)
    {
        AuthorsCollection.Remove(author);
    }

    public void AllAuthorsToUpperCase()
    {
        foreach (var author in AuthorsCollection)
        {
            author.ToUpperInvariant();
        }
    }

    public bool MoveAuthorUp(string author)
    {
        return SwapAuthors(AuthorsCollection.IndexOf(author), 1);
    }

    public bool MoveAuthorDown(string author)
    {
        return SwapAuthors(AuthorsCollection.IndexOf(author), -1);
    }

    private bool SwapAuthors(int index, int offset)
    {
        var (index1, index2) = (index, index + offset);
        (AuthorsCollection[index1], AuthorsCollection[index2]) = (AuthorsCollection[index2], AuthorsCollection[index1]);
        return true;
    }

    public bool MoveAuthorToStart(string author) => MoveAuthorToExtreme(AuthorsCollection.IndexOf(author), -1);

    public bool MoveAuthorToEnd(string author) => MoveAuthorToExtreme(AuthorsCollection.IndexOf(author), 1);

    private bool MoveAuthorToExtreme(int index, int step)
    {
        bool result = false;
        while (SwapAuthors(index, step))
        {
            (index, result) = (index + step, true);
        }

        return result;
    }
}