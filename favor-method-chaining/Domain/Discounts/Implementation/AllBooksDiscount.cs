namespace Bookstore.Domain.Discounts.Implementation;

public class AllBooksDiscount : RestrictedDiscount
{
    public AllBooksDiscount(IDiscount other) : base(other, " on all books") { }

    protected override bool AppliesTo(DiscountContext context) => true;
}
