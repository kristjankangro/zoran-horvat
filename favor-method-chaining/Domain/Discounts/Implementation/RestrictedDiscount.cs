using Bookstore.Domain.Common;

namespace Bookstore.Domain.Discounts.Implementation;

public abstract class RestrictedDiscount : IDiscount
{
    private readonly IDiscount _other;
    private readonly string _labelSuffix;

    protected RestrictedDiscount(IDiscount other, string labelSuffix) =>
        (_other, _labelSuffix) = (other, labelSuffix);

    public IEnumerable<DiscountApplication> GetDiscountAmounts(Money price, DiscountContext context) =>
        AppliesTo(context) ? ApplyTo(price, context).WithSuffix(_labelSuffix)
        : Enumerable.Empty<DiscountApplication>();

    protected IEnumerable<DiscountApplication> ApplyTo(Money price, DiscountContext context) =>
        _other.GetDiscountAmounts(price, context);

    protected abstract bool AppliesTo(DiscountContext context);
}

