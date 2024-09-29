using Bookstore.Domain.Discounts.Implementation;
using Bookstore.Domain.Models;

namespace Bookstore.Domain.Discounts;

public class DemoDiscountServerNested : IDiscountServer
{
    private readonly Person _martinFowler;

    public DemoDiscountServerNested(Person author) =>
        _martinFowler = author;

    public IDiscount GetDiscounts() =>
        FirstOf(
            OnBooksBy(_martinFowler, OnTopic("patterns", RelativeDiscount(.3M))),
            OnTopic("patterns", RelativeDiscount(.2M)),
            OnAllBooks(RelativeDiscount(.1M)));

    private IDiscount RelativeDiscount(decimal fact) => new RelativeDiscount(fact);
    private IDiscount OnTopic(string topic, IDiscount other) => new TitleContentDiscount(topic, other);
    private IDiscount OnBooksBy(Person author, IDiscount other) => new BookAuthorDiscount(author, other);

    private IDiscount OnAllBooks(IDiscount other) => new AllBooksDiscount(other);

    private IDiscount FirstOf(params IDiscount[] discounts)
        => discounts.Reverse().Aggregate((alt, primary) => new DefaultIfEmptyDiscount(primary, alt));
}