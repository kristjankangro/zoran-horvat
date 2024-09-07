using Bookstore.Domain.Discounts.Implementation;
using Bookstore.Domain.Models;

namespace Bookstore.Domain.Discounts;

public class DemoDiscountServer : IDiscountServer
{
    private readonly Person _martinFowler;

    public DemoDiscountServer(Person author) =>
        _martinFowler = author;

    public IDiscount GetDiscounts() => new NoDiscount();
}