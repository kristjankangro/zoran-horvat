using Demo.Models;

namespace Demo.Data;

static class BooksData
{
    /// <summary> Get data delegate property. Matching method for DataSource Generic delegate   </summary>
    /// <returns>Async method </returns>        
    public static DataSource<BookType> GetBooks =>
        () => Task.FromResult<IEnumerable<BookType>>(
        [
            Book.Create("Design Patterns", Name.CreateMany(
                Name.Create("Erich", "Gamma"), Name.Create("Richard", "Helm"),
                Name.Create("Ralph", "Johnson"), Name.Create("John", "Vlissides"))!)!,
            Book.Create("The C Programming Language", Name.CreateMany(
                Name.Create("Kernighan"), Name.Create("Ritchie"))!)!,
        ]);
}