using Stackable = LiskovTest<IStackable<int>, Stack<int>, UniqueStack<int>>;
using Stack = LiskovTest<IStackProper<int>, Stack<int>, Stack<int>>;

static class InterfaceLiskovTest
{
    public static void Run()
    {
        Console.WriteLine($"Testing classes:".ToUpper());
        Console.WriteLine();

        Stackable.TryOut("Push leaves stackable non-empty", stack => stack.Push(1), stack => stack.Count > 0);
        Stackable.TryOut("Push leaves stackable non-empty", stack => stack.Push(1, 2), stack => stack.Count > 0);
        Stackable.TryOut("Push leaves stackable non-empty", stack => stack.Push(1, 1), stack => stack.Count > 0);

        Stackable.TryOut("Pop succeeds on non-empty stackable", stack => stack.Push(1), stack => stack.Pop() is int _);
        Stackable.TryOut("Pop succeeds on non-empty stackable", stack => stack.Push(1, 2), stack => stack.Pop() is int _);
        Stackable.TryOut("Pop succeeds on non-empty stackable", stack => stack.Push(1, 1), stack => stack.Pop() is int _);

        Stackable.TryOut("The last pushed item comes out first", stack => stack.Push(1, 2), stack => stack.Pop() == 2);
        Stackable.TryOut("The last pushed item comes out first", stack => stack.Push(1, 1), stack => stack.Pop() == 1);

        Stackable.TryOut("Stackable contains a pushed item", stack => stack.Push(1, 2), stack => stack.Contains(1));
        Stackable.TryOut("Stackable contains a pushed item", stack => stack.Push(1, 2), stack => stack.Contains(2));
        Stackable.TryOut("Stackable contains a pushed item", stack => stack.Push(1, 1), stack => stack.Contains(1));

        Console.WriteLine(new string('-', 82));

        Stack.TryOutSingle("Stack items come in LIFO order", stack => stack.Push(1, 2),
            stack => stack.Pop() >= stack.Pop());
        Stack.TryOutSingle("Stack items come in LIFO order", stack => stack.Push(1, 1),
            stack => stack.Pop() >= stack.Pop());
        Stack.TryOutSingle("Stack items come in LIFO order", stack => stack.Push(1, 2, 1), stack => stack.Pop(),
            stack => stack.Pop() >= stack.Pop());

        Stack.TryOutSingle("Push increments stack size", _ => { }, stack => stack.Push(1),
            (before, after) => after.Count == before.Count + 1);
        Stack.TryOutSingle("Push increments stack size", stack => stack.Push(1), stack => stack.Push(2),
            (before, after) => after.Count == before.Count + 1);
        Stack.TryOutSingle("Push increments stack size", stack => stack.Push(1), stack => stack.Push(1),
            (before, after) => after.Count == before.Count + 1);

        Stack.TryOutSingle("Pop decrements stack size", stack => stack.Push(1), stack => stack.Pop(),
            (before, after) => after.Count == before.Count - 1);
        Stack.TryOutSingle("Pop decrements stack size", stack => stack.Push(1, 2), stack => stack.Pop(),
            (before, after) => after.Count == before.Count - 1);
        Stack.TryOutSingle("Pop decrements stack size", stack => stack.Push(1, 1), stack => stack.Pop(),
            (before, after) => after.Count == before.Count - 1);

        Console.WriteLine();
        Console.WriteLine(new string('=', 82));
        Console.WriteLine();
    }
}

file interface IStackable<T> : IEnumerable<T>
{
    int Count { get; }
    void Push(T item, params T[] more);
    T Pop();
}

file interface IStackProper<T> : IStackable<T> { }

file class Stack<T> : IStackProper<T>
{
    private System.Collections.Generic.Stack<T> Items { get; } = new();

    public int Count => Items.Count;

    public void Push(T item, params T[] more)
    {
        Items.Push(item);
        foreach (var additional in more) Items.Push(additional);
    }

    public T Pop() => Items.Pop();

    public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}

file class UniqueStack<T> : IStackable<T>
{
    private Stack<T> Items { get; } = new();

    public int Count => Items.Count;

    public void Push(T item, params T[] more) => new T[] { item }.Concat(more)
        .Distinct()
        .Where(x => !Items.Contains(x))
        .ToList()
        .ForEach(x => Items.Push(x));

    public T Pop() => Items.Pop();

    public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}