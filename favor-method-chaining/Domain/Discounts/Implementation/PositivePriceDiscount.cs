using Bookstore.Domain.Common;

namespace Bookstore.Domain.Discounts.Implementation;

public class PositivePriceDiscount : IDiscount
{
    private readonly IDiscount _other;

    public PositivePriceDiscount(IDiscount other) => _other = other;

    public IEnumerable<DiscountApplication> GetDiscountAmounts(Money price, DiscountContext context) =>
        price == Money.Zero ? Enumerable.Empty<DiscountApplication>()
        : _other.GetDiscountAmounts(price, context);
}
