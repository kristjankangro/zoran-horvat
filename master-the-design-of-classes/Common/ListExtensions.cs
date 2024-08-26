public static class ListExtensions
{
    public static bool Remove<T>(this List<T> list, Predicate<T> selector)
    {
        if (list.Find(selector) is not int index) return false;

        list.RemoveAt(index);
        return true;
    }

    public static bool SwapWithPrevious<T>(this List<T> list, Predicate<T> selector) =>
        list.Find(selector) is int index && list.SwapAdjacent(index, -1);

    public static bool SwapWithNext<T>(this List<T> list, Predicate<T> selector) =>
        list.Find(selector) is int index && list.SwapAdjacent(index, 1);


    public static bool MoveToBeginning<T>(this List<T> list, Predicate<T> selector) =>
        list.Find(selector) is int index && list.MoveToExtreme(index, -1);


    public static bool MoveToEnd<T>(this List<T> list, Predicate<T> selector) =>
        list.Find(selector) is int index && list.MoveToExtreme(index, 1);

    public static void ChangeInPlace<T>(this List<T> list, Func<T, T> map)
    {
        for (int i = 0; i < list.Count; i++) list[i] = map(list[i]);
    }

    private static bool SwapAdjacent<T>(this List<T> list, int index, int offset)
    {
        var index1 = index + offset;
        if (!CanSwap(list, index, index1)) return false;

        (list[index], list[index1]) = (list[index1], list[index]);
        return true;
    }

    private static bool MoveToExtreme<T>(this List<T> list, int index, int step)
    {
        var index1 = index + step;
        if (!CanSwap(list, index, index1)) return false;
        
        while (index1 >= 0 && index1 < list.Count)
        {
            (list[index], list[index1]) = (list[index1], list[index]);
            (index, index1) = (index1, index1 + step);
        }

        return true;
    }

    private static int? FindIndex<T>(this List<T> list, Predicate<T> selector)
    {
        for (int i = 0; i < list.Count; i++) if (selector(list[i])) return i;

        return null;
    }

    private static bool CanSwap<T>(List<T> list, int index, int index1) => Math.Min(index, index1) >= 0 && Math.Max(index, index1) < list.Count;
}