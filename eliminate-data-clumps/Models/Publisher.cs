namespace Demo.Models;

public class Publisher
{
    public int Id { get; private set; }
    public string Name { get; private set; }

    public static Publisher CreateNew(string name, string key) => new Publisher(0, key, name);

    public static Publisher CreateExisting(int id, string name, string key) =>
        new Publisher(id <= 0 ? throw new ArgumentException("Identity must be positive", nameof(id)) : id, key, name);

    private Publisher(int id, string key, string name)
    {
        Id = id;
        Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentException("Name must be non-empty", nameof(name));
    }
}

