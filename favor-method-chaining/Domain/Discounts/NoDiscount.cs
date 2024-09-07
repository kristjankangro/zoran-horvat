using Bookstore.Domain.Common;

namespace Bookstore.Domain.Discounts;

public class NoDiscount : IDiscount
{
    public IEnumerable<DiscountApplication> GetDiscountAmounts(Money price, DiscountContext context) =>
        Enumerable.Empty<DiscountApplication>();
}