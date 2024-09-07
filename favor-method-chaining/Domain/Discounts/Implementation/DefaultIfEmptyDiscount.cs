using Bookstore.Domain.Common;

namespace Bookstore.Domain.Discounts.Implementation;

public class DefaultIfEmptyDiscount : IDiscount
{
    private readonly IDiscount _primary;
    private readonly IDiscount _alternate;

    public DefaultIfEmptyDiscount(IDiscount primary, IDiscount alternate) =>
        (_primary, _alternate) = (primary, alternate);

    public IEnumerable<DiscountApplication> GetDiscountAmounts(Money price, DiscountContext context)
    {
        using IEnumerator<DiscountApplication> primaryEnumerator = _primary.GetDiscountAmounts(price, context).GetEnumerator();
        
        if (primaryEnumerator.MoveNext())
        {
            yield return primaryEnumerator.Current;
            while (primaryEnumerator.MoveNext()) yield return primaryEnumerator.Current;
            yield break;
        }

        foreach (var alternateItem in _alternate.GetDiscountAmounts(price, context))
        {
            yield return alternateItem;
        }
    }
}
