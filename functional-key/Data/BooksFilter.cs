using Demo.Models;

namespace Demo.Data;

static class BooksFiltering
{
    public static Filter<BookType> FilterBooks =>
        async (DataSource<BookType> dataSource, string phrase) =>
        (await dataSource()).Where(b => b.Title.Contains(phrase, StringComparison.InvariantCultureIgnoreCase));
}