namespace Demo.Common;

public static class HandleToSlugConversions
{
    public static HandleToSlug Concatenate =>
        handle => new(string.Join(string.Empty, handle.Components));
    
    public static HandleToSlug Hyphenate =>
        handle => new(string.Join('-', handle.Components));
}