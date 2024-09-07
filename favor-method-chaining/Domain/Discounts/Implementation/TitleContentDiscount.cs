namespace Bookstore.Domain.Discounts.Implementation;

public class TitleContentDiscount : RestrictedDiscount
{
    private readonly string _titleSubstring;

    public TitleContentDiscount(string titleSubstring, IDiscount other)
        : base(other, $" when '{titleSubstring}' in the title") =>
        _titleSubstring = !string.IsNullOrEmpty(titleSubstring) ? titleSubstring : throw new ArgumentException("Title substring must not be empty");

    protected override bool AppliesTo(DiscountContext context) =>
        context.Book?.Title.Contains(_titleSubstring, StringComparison.InvariantCultureIgnoreCase) ?? false;
}