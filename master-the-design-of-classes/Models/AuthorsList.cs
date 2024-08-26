using System.Collections;
using System.Globalization;

namespace Demo.Models;

public class AuthorsList : IEnumerable<Author>
{
    private List<Author> AuthorsCollection { get; set; }
    public IEnumerable<CultureInfo> Cultures => AuthorsCollection.Select(a => a.Culture).Distinct();

    public AuthorsList(IEnumerable<Author> authors) => AuthorsCollection = authors.ToList();

    private bool IsValidAuthor(string name)
    {
        return !string.IsNullOrWhiteSpace(name) ? true : throw new ArgumentException(nameof(name));
    }

    public void AppendAuthor(Author author) => AuthorsCollection.Add(author);

    public bool RemoveAuthor(string author) => AuthorsCollection.Remove(FilterByName(author));

    private Author? FirstOrDefaultAuthor(string value) =>
        AuthorsCollection.FirstOrDefault(author => author.IsMatch(value));

    public void AllAuthorsToUpperCase() => AuthorsCollection.ForEach(author => author.ToUpper());

    private Predicate<Author> FilterByName(string name) =>
        author => author.IsMatch(name);
    

    public bool MoveAuthorUp(string author) => AuthorsCollection.SwapWithPrevious(FilterByName(author));
    public bool MoveAuthorDown(string author) => AuthorsCollection.SwapWithNext(FilterByName(author));

    private bool SwapAuthors(int index, int offset)
    {
        var (index1, index2) = (index, index + offset);
        if (Math.Min(index1, index2) < 0 || Math.Max(index1, index2) >= AuthorsCollection.Count) return false;

        (AuthorsCollection[index1], AuthorsCollection[index2]) = (AuthorsCollection[index2], AuthorsCollection[index1]);
        return true;
    }

    public bool MoveAuthorToStart(string author) => AuthorsCollection.MoveToBeginning(FilterByName(author));
    public bool MoveAuthorToEnd(string author) => AuthorsCollection.MoveToEnd(FilterByName(author));

    private bool MoveAuthorToExtreme(int index, int step)
    {
        bool result = false;
        while (SwapAuthors(index, step))
        {
            (index, result) = (index + step, true);
        }

        return result;
    }

    public IEnumerator<Author> GetEnumerator() => AuthorsCollection.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}