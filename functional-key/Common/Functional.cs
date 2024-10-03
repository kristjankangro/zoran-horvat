namespace Demo.Common;

public static class Functional
{
    public static R? BindOptional<T, R>(this T? obj, Func<T, R?> map)
        where T : class
        where R : class =>
        obj is null ? null : map(obj);
    
    public static R? MapOptional<T, R>(this T? obj, Func<T, R> map)
        where T : class
        where R : class =>
        obj is null ? null : map(obj);

    public static void DoOptional<T>(this T? obj, Action<T> action)
    {
        if (obj is not null) action(obj);
    }

    public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
    {
        foreach (T obj in sequence) action(obj);
    }
}

