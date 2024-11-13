using System.Globalization;

namespace Demo.Models;

public class Author
{
    public int Id { get; private set; }
    public string DisplayName { get; private set; }
    public string FirstName { get; private set; }
    public string? MiddleNames { get; private set; }
    public string LastName { get; private set; }

    public string CultureName { get; private set; }

    public static Author CreateNew(string displayName, string firstName, string lastName, string cultureName) =>
        new Author(0, displayName, firstName, null, lastName, cultureName);

    public static Author CreateExisting(int id, string displayName, string firstName, string? middleNames, string lastName, string cultureName) =>
        new Author(id <= 0 ? throw new ArgumentException("Identity must be positive", nameof(id)) : id, displayName, firstName, middleNames, lastName, cultureName);

    private Author(int id, string displayName, string firstName, string? middleNames, string lastName, string cultureName)
    {
        Id = id;
        DisplayName = displayName;
        FirstName = firstName;
        MiddleNames = middleNames;
        LastName = lastName;
        CultureName = cultureName;
    }
}