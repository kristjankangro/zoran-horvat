using LanguageExt;
using Microsoft.EntityFrameworkCore;
namespace Functional;

public class Person
{
    internal Guid Id { get; set; }
    internal string FirstName { get; set; } = string.Empty;
    internal string LastName { get; set; } = string.Empty;

    public Person(Guid id, string firstName, string lastName) =>
        (Id, FirstName, LastName) = (id, firstName, lastName);

    public override string ToString()
    {
        return $"Id: {Id}, FirstName: {FirstName}, LastName: {LastName}";
    }
}

public class Context : DbContext
{
    public DbSet<Person> Persons { get; set; }
}

public static class DbContextExtensions
{
    public static Option<T> TryFind<T>(this DbSet<T> dbSet, params object[] keyValues)
    where T : class => Prelude.Optional(dbSet.Find(keyValues));
}