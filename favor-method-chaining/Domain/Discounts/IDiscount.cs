using Bookstore.Domain.Common;
using Bookstore.Domain.Models;

namespace Bookstore.Domain.Discounts;

public interface IDiscount
{
    IEnumerable<DiscountApplication> GetDiscountAmounts(Money price, DiscountContext context);
}

public record DiscountApplication(string Label, Money Amount);

public record DiscountContext(Book? Book);