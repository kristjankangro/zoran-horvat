using System.Data;

public class UniqueStack<T> : Stack<T>
{

    public override void Push(T item, params T[] more)
    {
        PushSingle(item);
        foreach (var additionalItem in more) PushSingle(additionalItem);
    }

    private void PushSingle(T item)
    {
        if (this.Contains(item)) return;
        base.Push(item);
    }
    
}