namespace Meta;

public class Person
{
    internal Guid Id { get; set; }
    internal string FirstName { get; set; } = string.Empty;
    internal string LastName { get; set; } = string.Empty;

    public Person(Guid id, string firstName, string lastName) =>
        (Id, FirstName, LastName) = (id, firstName, lastName);

    //public PersonDto ToDto() => new (Id, $"{FirstName} {LastName}"); //bad
}

//okeish
public static class Person1Extensions
{
    public static PersonDto ToPersonDto(this Person person) =>
        new(person.Id, $"{person.FirstName} {person.LastName}");
}

public record PersonDto(Guid Id, string FullName)
{
    // public static PersonDto From(Person person) =>
    // (Id, FullName) = (person.Id, $"{person.FirstName} {person.LastName}";
    // bad
}