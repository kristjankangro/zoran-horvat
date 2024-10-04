using Demo.Models;

namespace Demo.Data;

static class BooksData
{
    /// <summary>
    /// Get data delegate property
    /// </summary>
    /// <returns>Async method </returns>
    public static DataSource<BookType> GetBooks
    {
        //public delegate Task<IEnumerable<T>> DataSource<T>(); method signature match
        get
        {
            return () => Task.FromResult<IEnumerable<BookType>>(
            [
                Book.Create("Design Patterns", Name.CreateMany(
                    Name.Create("Erich", "Gamma"), Name.Create("Richard", "Helm"),
                    Name.Create("Ralph", "Johnson"), Name.Create("John", "Vlissides"))!)!,
                Book.Create("The C Programming Language", Name.CreateMany(
                    Name.Create("Kernighan"), Name.Create("Ritchie"))!)!,
            ]);
        }
    }
}