using Bookstore.Domain.Discounts.Implementation;
using Bookstore.Domain.Models;

namespace Bookstore.Domain.Discounts;

public class DemoDiscountServer : IDiscountServer
{
    private readonly Person _martinFowler;

    public DemoDiscountServer(Person author) => _martinFowler = author;

    public IDiscount GetDiscounts() =>
        RelativeDiscount(.3M).OnTopic("patterns").OnBooksBy(_martinFowler)
            .OrElse(
                RelativeDiscount(.2M).OnTopic("patterns"))
            .OrElse(
                RelativeDiscount(.1M).OnAllBooks());

    private IDiscount RelativeDiscount(decimal fact) => new RelativeDiscount(fact);
}