public record ImmutableBook(string Title, string[] Keywords)
{
    public string Title { get; } =
        string.IsNullOrWhiteSpace(Title) ? throw new ArgumentNullException(nameof(Title)) : Title.Trim();

    public string[] Keywords { get; } = Keywords.Concat(Title.Split(" ")).ToArray();
};