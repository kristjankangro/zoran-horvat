using Bookstore.Domain.Models;

namespace Bookstore.Domain.Discounts.Implementation;

public class BookAuthorDiscount : RestrictedDiscount
{
    private readonly Person _author;

    public BookAuthorDiscount(Person author, IDiscount other)
        : base(other, $" authored by {author.FullName}") =>
        _author = author;

    protected override bool AppliesTo(DiscountContext context) =>
        context.Book?.Authors.Any(author => author.Id == _author.Id) ?? false;
}
