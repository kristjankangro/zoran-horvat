using System.Globalization;

namespace Demo.Common;

public static class HandleTransforms
{
    public static TransformHandle ToLowercase(CultureInfo culture) =>
        handle => new(handle.Components.Select(c => culture.TextInfo.ToLower(c)).ToArray());
    
    public static TransformHandle SplitOnWhiteSpace =>
        handle => new(handle.Components
            .SelectMany(c => c.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries))
            .ToArray());
    
    public static TransformHandle IntoLetterAndDigitRuns =>
        handle => new(handle.Components.SelectMany(SplitLetterAndDigitRuns).ToArray());
    
    private static IEnumerable<string> SplitLetterAndDigitRuns(string s)
    {
        int start = 0;
        while (start < s.Length && !char.IsLetterOrDigit(s[start])) start += 1;

        while (start < s.Length)
        {
            int end = start + 1;
            while (end < s.Length && char.IsLetterOrDigit(s[end])) end += 1;
            yield return s[start..end];
            start = end;
            while (start < s.Length && !char.IsLetterOrDigit(s[start])) start += 1;
        }
    }

    public static TransformHandle StopAtColon =>
        handle => new(StopStringsAtColon(handle.Components).ToArray());
    
    private static IEnumerable<string> StopStringsAtColon(IEnumerable<string> strings)
    {
        foreach (var s in strings)
        {
            int colon = s.IndexOf(':');
            if (colon >= 0)
            {
                yield return s[..colon];
                yield break;
            }
            yield return s;
        }
    }
}