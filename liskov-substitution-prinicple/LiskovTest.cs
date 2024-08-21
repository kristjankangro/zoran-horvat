using System.Text.RegularExpressions;

static class LiskovTest<TBase, TDerived1, TDerived2>
    where TDerived1 : TBase, new()
    where TDerived2 : TBase, new()
{
    public static void TryOut(string label, Action<TBase> what, Func<TBase, bool> check) =>
        TryOut(label, _ => { }, what, (_, final) => check(final));

    public static void TryOut(string label, Action<TBase> prepare, Action<TBase> what, Func<TBase, bool> check) =>
        TryOut(label, prepare, what, (_, final) => check(final));

    public static void TryOut(string label, Action<TBase> prepare, Action<TBase> what, Func<TBase, TBase, bool> check)
    {
        WritePrelude(label);
        
        WriteType<TDerived1>();
        TryOutInstance<TDerived1>(prepare, what, check);

        WriteType<TDerived2>();
        TryOutInstance<TDerived2>(prepare, what, check);

        Console.WriteLine();
    }

    public static void TryOutSingle(string label, Action<TBase> what, Func<TBase, bool> check) =>
        TryOutSingle(label, _ => { }, what, (_, final) => check(final));

    public static void TryOutSingle(string label, Action<TBase> prepare, Action<TBase> what, Func<TBase, bool> check) =>
        TryOutSingle(label, prepare, what, (_, final) => check(final));

    public static void TryOutSingle(string label, Action<TBase> prepare, Action<TBase> what, Func<TBase, TBase, bool> check)
    {
        WritePrelude(label);
        
        WriteType<TDerived1>();
        TryOutInstance<TDerived1>(prepare, what, check);

        Console.WriteLine();
    }

    public static void TryOutInstance<T>(Action<TBase> prepare, Action<TBase> what, Func<TBase, TBase, bool> check)
        where T : TBase, new()
    {
        TBase obj = new T();
        prepare(obj);
        TBase unmodified = new T();
        prepare(unmodified);

        TryOutInstance(obj, what, final => check(unmodified, final));
    }


    private static void TryOutInstance(TBase obj, Action<TBase> what, Func<TBase, bool> check)
    {
        string outcome = "Failed";
        try
        {
            what(obj);
            if (check(obj)) outcome = "Success";
        }
        catch
        {
            outcome = "Crashed";
        }
        Console.Write(outcome.PadRight(10));
    }

    private static void WritePrelude(string label) =>
        Console.Write(label.ToUpper().PadLeft(40) + ":");

    private static void WriteType<T>() =>
        Console.Write($"{GetTypeName<T>(),12}: ");

    private static string GetTypeName<Type>() =>
        Regex.Match(typeof(Type).Name, @"(^.*__)?(?<name>[^`]*)(`\d+)?").Groups["name"].Value;
}
