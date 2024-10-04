using Demo.Models;

namespace Demo.Data;

static class BooksFiltering
{
    /// <summary> Concrete Filter delegate of BookType </summary>
    public static Filter<BookType> BooksFiltered
    {
        get
        {
            return async (DataSource<BookType> dataSource, string phrase) =>
                (await dataSource()).Where(b => b.Title.Contains(phrase, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}