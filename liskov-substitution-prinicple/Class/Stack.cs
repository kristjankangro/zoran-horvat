public class Stack<T> : IEnumerable<T>
{
    private List<T> Items { get; } = new();

    public virtual int Count => Items.Count;

    public virtual void Push(T item, params T[] more)
    {
        Items.Add(item);
        foreach (var additionalItem in more) Items.Add(additionalItem);
    }

    public virtual T Pop()
    {
        if (Items.Count == 0) throw new InvalidOperationException("Stack is empty");
        var result = Items[^1];
        Items.RemoveAt(Items.Count - 1);
        return result;
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = Items.Count - 1; i >= 0; i--) yield return Items[i];
        
    }

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}