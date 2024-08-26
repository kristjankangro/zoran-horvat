using System.Globalization;

namespace Demo.Models;

public class Author
{
    private string _fullName = string.Empty;

    public Author(string fullName, CultureInfo culture)
    {
        FullName = fullName;
        Culture = culture;
    }

    public CultureInfo Culture { get; set; }

    public string FullName
    {
        get => _fullName;
        set => _fullName = !string.IsNullOrWhiteSpace(value) ? value.Trim() : throw new ArgumentNullException(nameof(value));
    }

    public void ToUpper() => FullName = Culture.TextInfo.ToUpper(FullName);

    public bool IsMatch(string fullName) => string.Compare(FullName, fullName, Culture, CompareOptions.IgnoreCase) == 0;
}