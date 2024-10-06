namespace Bookstore.Models;

/// <summary>
/// EF model and domain model
/// </summary>
public class Book
{
    public int Id { get; private set; } = 0;

    public string Title { get; private set; } = string.Empty;
    // public Person? Author { get; private set; } = null;

    /// <summary>
    /// EF core requirement
    /// </summary>
    private Book() { }

    public static Book CreateNew(string title) =>
        new() { Title = title };
    
    // public static Book CreateNew(string title, Person author) =>
    //     new() { Title = title, Author = author };
}