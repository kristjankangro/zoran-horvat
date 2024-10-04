using Demo.Models;

namespace Demo.Data;

static class BooksFiltering
{
    public static Filter<BookType> FilterBooks
    {
        get
        {
            return async (DataSource<BookType> dataSource, string phrase) =>
                (await dataSource()).Where(b => b.Title.Contains(phrase, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}