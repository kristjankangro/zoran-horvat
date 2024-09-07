using Bookstore.Domain.Common;

namespace Bookstore.Domain.Discounts.Implementation;

public class RelativeDiscount : IDiscount
{
    private readonly decimal _factor;

    public RelativeDiscount(decimal factor) =>
        _factor = factor > 0 && factor < 1 ? factor
        : throw new ArgumentException("Multiplying factor must be positive and smaller than 1.");

    public IEnumerable<DiscountApplication> GetDiscountAmounts(Money price, DiscountContext context) =>
        price == Money.Zero ? Enumerable.Empty<DiscountApplication>()
        : new[] { new DiscountApplication($"{_factor:P2} discount", price * _factor) };
}

