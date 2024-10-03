namespace OOP;

public class Person3
{
    public float Height { get; }
    internal Guid Id { get; set; }
    internal string FirstName { get; set; } = string.Empty;
    internal string LastName { get; set; } = string.Empty;

    public Person3(Guid id, string firstName, string lastName, float height)
        => (Id, FirstName, LastName, Height) = (id, firstName, lastName, height);
}

public static class SortingExtensions
{
    public static IEnumerable<T> Smallest<T>
        (this IEnumerable<T> items, int count, IComparer<T> comparer)
    {
        List<T> sorted = new();
        foreach (var item in items)
        {
            sorted.Add(item);
            for (var i = sorted.Count - 1; i > 0; i--)
            {
                if (comparer.Compare(sorted[i], sorted[i - 1]) >= 0) break;
                (sorted[i], sorted[i - 1]) = (sorted[i - 1], sorted[i]);
                if (sorted.Count >= count) sorted.RemoveAt(sorted.Count - 1);
            }
        }
        return sorted;
    }
}