namespace Bookstore.Domain.Discounts.Implementation;

internal static class DiscountApplicationExtensions
{
    internal static IEnumerable<DiscountApplication> WithSuffix(
        this IEnumerable<DiscountApplication> discounts, string suffix) =>
        discounts.Select(discount => discount with { Label = discount.Label + suffix });
}

