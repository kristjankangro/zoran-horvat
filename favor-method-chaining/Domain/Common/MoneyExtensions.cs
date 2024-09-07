namespace Bookstore.Domain.Common;

public static class MoneyExtensions
{
    public static Money Sum(this IEnumerable<Money> sequence) =>
        sequence.Aggregate(Money.Zero, (sum, amount) => sum + amount);
    
    public static Money Sum<T>(this IEnumerable<T> sequence, Func<T, Money> selector) =>
        sequence.Select(selector).Sum();
}