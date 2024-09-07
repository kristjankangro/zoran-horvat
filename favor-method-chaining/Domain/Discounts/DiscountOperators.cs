using Bookstore.Domain.Discounts.Implementation;
using Bookstore.Domain.Models;

namespace Bookstore.Domain.Discounts;

public static class DiscountOperators
{
    public static IDiscount OnTopic(this IDiscount appliesTo, string topic) =>
        new TitleContentDiscount(topic, appliesTo);

    public static IDiscount OnBooksBy(this IDiscount appliesTo, Person author) =>
        new BookAuthorDiscount(author, appliesTo);

    public static IDiscount OnAllBooks(this IDiscount appliesTo) => new AllBooksDiscount(appliesTo);

    public static IDiscount OrElse(this IDiscount discount, IDiscount alt) => new DefaultIfEmptyDiscount(discount, alt);
}