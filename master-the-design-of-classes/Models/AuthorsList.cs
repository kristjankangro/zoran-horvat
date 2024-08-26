using System.Collections;

namespace Demo.Models;

public class AuthorsList : IEnumerable<string>
{
    private List<string> AuthorsCollection { get; set; }

    public AuthorsList(IEnumerable<string> authors) => AuthorsCollection = authors.Where(IsValidAuthor).ToList();

    private bool IsValidAuthor(string author)
    {
        return !string.IsNullOrWhiteSpace(author) ? true : throw new ArgumentException(nameof(author));
    }

    public void AppendAuthor(string author)
    {
        IsValidAuthor(author);
        AuthorsCollection.Add(author);
    }

    public bool RemoveAuthor(string author) => AuthorsCollection.Remove(FilterFor(author));

    private string? FirstOrDefaultAuthor(string value) =>
        AuthorsCollection.FirstOrDefault(author => author.Equals(value, StringComparison.InvariantCultureIgnoreCase));

    public void AllAuthorsToUpperCase()
    {
        foreach (var author in AuthorsCollection)
        {
            author.ToUpperInvariant();
        }
    }

    private Predicate<string> FilterFor(string value) => author => author.Equals(value, StringComparison.InvariantCultureIgnoreCase);

    public bool MoveAuthorUp(string author) => AuthorsCollection.SwapWithPrevious(FilterFor(author));
    public bool MoveAuthorDown(string author) => AuthorsCollection.SwapWithNext(FilterFor(author));

    private bool SwapAuthors(int index, int offset)
    {
        var (index1, index2) = (index, index + offset);
        if (Math.Min(index1, index2) < 0 || Math.Max(index1, index2) >= AuthorsCollection.Count) return false;

        (AuthorsCollection[index1], AuthorsCollection[index2]) = (AuthorsCollection[index2], AuthorsCollection[index1]);
        return true;
    }

    public bool MoveAuthorToStart(string author) => AuthorsCollection.MoveToBeginning(FilterFor(author));
    public bool MoveAuthorToEnd(string author) => AuthorsCollection.MoveToEnd(FilterFor(author));

    private bool MoveAuthorToExtreme(int index, int step)
    {
        bool result = false;
        while (SwapAuthors(index, step))
        {
            (index, result) = (index + step, true);
        }

        return result;
    }

    public IEnumerator<string> GetEnumerator() => AuthorsCollection.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}