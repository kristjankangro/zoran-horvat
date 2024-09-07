using Bookstore.Domain.Discounts.Implementation;
using Bookstore.Domain.Models;

namespace Bookstore.Domain.Discounts;

public class DemoDiscountServer : IDiscountServer
{
    private readonly Person _martinFowler;

    public DemoDiscountServer(Person author) => _martinFowler = author;

    //public IDiscount GetDiscounts() => new NoDiscount();

    // business request #1
    // 30% discount on martin fowler patterns books
    // or then 20% on books of patterns
    // or flat 10% all books RelativeDiscount(0.1M)

    //nested calls todod fix to chain
    public IDiscount GetDiscounts() =>
        RelativeDiscount(.3M).OnTopic("patterns").OnBooksBy(_martinFowler)
            .OrElse(
                RelativeDiscount(.2M).OnTopic("patterns"))
            .OrElse(
                RelativeDiscount(.1M).OnAllBooks());

    private IDiscount RelativeDiscount(decimal fact) => new RelativeDiscount(fact);
}