namespace Demo.Common;

public static class ListExtensions
{
    public static bool Remove<T>(this List<T> list, Func<T, bool> selector)
    {
        if (list.Find(selector) is int index)
        {
            list.RemoveAt(index);
            return true;
        }
        return false;
    }
    
    public static bool SwapWithPrevious<T>(this List<T> list, Func<T, bool> selector) =>
        list.Find(selector) is int index && list.SwapWithAdjacent(index, -1);

    public static bool SwapWithNext<T>(this List<T> list, Func<T, bool> selector) =>
        list.Find(selector) is int index && list.SwapWithAdjacent(index, 1);

    public static bool MoveToBeginning<T>(this List<T> list, Func<T, bool> selector) =>
        list.Find(selector) is int index && list.MoveToExtreme(index, -1);

    public static bool MoveToEnd<T>(this List<T> list, Func<T, bool> selector) =>
        list.Find(selector) is int index && list.MoveToExtreme(index, 1);

    public static void ChangeInPlace<T>(this List<T> list, Func<T, T> map)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = map(list[i]);
        }
    }

    private static bool SwapWithAdjacent<T>(this List<T> list, int index, int offset)
    {
        int index1 = index + offset;
        if (Math.Min(index, index1) < 0 || Math.Max(index, index1) >= list.Count) return false;

        (list[index], list[index1]) = (list[index1], list[index]);
        return true;
    }

    private static bool MoveToExtreme<T>(this List<T> list, int index, int step)
    {
        int index1 = index + step;
        if (Math.Min(index, index1) < 0 || Math.Max(index, index1) >= list.Count) return false;

        while (index1 >= 0 && index1 < list.Count)
        {
            (list[index], list[index1]) = (list[index1], list[index]);
            (index, index1) = (index1, index1 + step);
        }
        return true;
    }

    private static int? Find<T>(this List<T> list, Func<T, bool> selector)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (selector(list[i])) return i;
        }
        return null;
    }
}

