namespace ConsoleApp1;

public class MutableBook(string title, string[] keywords)
{
    private string _title = ValidTitle(title);
    private string[] _keywords = keywords.Concat(title.Split(" ")).ToArray();

    public string Title
    {
        get => _title;
        set => _title = ValidTitle(value);
    }

    private static string ValidTitle(string title) => string.IsNullOrWhiteSpace(title)
        ? throw new ArgumentNullException(nameof(Title))
        : title.Trim();
}