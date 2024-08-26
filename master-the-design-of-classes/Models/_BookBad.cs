using System.Globalization;

namespace Demo.Models;

public class _BookBad
{
    private string _title = string.Empty; //primitive type
    private string _publisher = string.Empty; //primitive type
    private int _edition; //primitive type

    public string Title
    {
        get => _title;
        set => _title = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException(nameof(Title));
    }

    public IEnumerable<string> Authors => AuthorsCollection;
    private List<string> AuthorsCollection { get; set; }

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

    public _BookBad(string title, IEnumerable<string> authors, string publisher, int edition,
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

    public bool RemoveAuthor(string author) =>
        FirstOrDEfaultAuthor(author) is string found && AuthorsCollection.Remove(found);

    private string? FirstOrDEfaultAuthor(string author) =>
        AuthorsCollection.FirstOrDefault(author => author.Equals(author, StringComparison.InvariantCultureIgnoreCase));

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