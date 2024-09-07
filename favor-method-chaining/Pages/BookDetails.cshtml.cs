using Bookstore.Data.Seeding;
using Bookstore.Domain.Common;
using Bookstore.Domain.Models;
using Bookstore.Domain.Discounts;
using Bookstore.Domain.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Bookstore.Data;
using Bookstore.Data.Specifications;
using Bookstore.Common;

namespace Bookstore.Pages;

public class BookDetailsModel : PageModel
{
    public record PriceLine(string Label, Money Amount);

    private readonly ILogger<IndexModel> _logger;
    private readonly IUnitOfWork _dbContext;
    private readonly IDataSeed<BookPrice> _bookPricesSeed;
    private readonly ISpecification<Book> _allBooksSpec;
    private readonly IBibliographicEntryFormatter _recommendedBooksFormatter;
    private IDiscountServer Discounts { get; set; }

    public Book Book { get; private set; } = null!;

    public IReadOnlyList<PriceLine> PriceSpecification { get; private set; } = Array.Empty<PriceLine>();
    public IReadOnlyList<(Citation book, Guid id)> RecommendedBooks { get; private set; } = Array.Empty<(Citation, Guid)>();

    public BookDetailsModel(ILogger<IndexModel> logger, IUnitOfWork dbContext, IDataSeed<BookPrice> bookPricesSeed,
                            IDiscountServer discounts, ISpecification<Book> spec, IBibliographicEntryFormatter recommendedBooksFormatter) =>
        (_logger, _dbContext, _bookPricesSeed, Discounts, _allBooksSpec, _recommendedBooksFormatter) =
        (logger, dbContext, bookPricesSeed, discounts, spec, recommendedBooksFormatter);

    public async Task<IActionResult> OnGet(Guid id) => await _dbContext.Books
        .SingleOrNoneAsync(_allBooksSpec.ById(id))
        .AuditAsync(Populate)
        .Map(_ => base.Page() as IActionResult)
        .Reduce(() => Redirect("/books"));

    private async Task Populate(Book book)
    {
        this.Book = book;
        await this.PopulatePriceSpecification();
        await this.PopulateRecommendedBooks();
    }

    private async Task PopulatePriceSpecification()
    {
        await this._bookPricesSeed.SeedAsync();
        Money? originalPrice = (await _dbContext.BookPrices.All.For(this.Book).At(DateTime.Now).FirstOrDefaultAsync())?.Price;
        _logger.LogInformation($"Book: {this.Book.Title}; original price: {(originalPrice.HasValue ? originalPrice.Value.ToString() : "<null>")}", this.Book.Title, originalPrice);
        this.PriceSpecification = originalPrice.HasValue ? this.CalculatePriceLines(originalPrice.Value) : new List<PriceLine>();
    }

    private async Task PopulateRecommendedBooks()
    {
        string[] words = this.Book.Title.SplitIntoWords().Where(word => word.Length > 3).ToArray();
        _logger.LogInformation("Title: {title}; words: {words}", this.Book.Title, string.Join(", ", words));
        var candidateBooks = await _dbContext.Books.All.ToListAsync();

        this.RecommendedBooks = candidateBooks
            .Select(book => (book, score: this.GetRecommendationScore(book, words)))
            .Where(bookScore => bookScore.book.Id != this.Book.Id && bookScore.score > 0)
            .OrderByDescending(bookScore => bookScore.score)
            .Take(3)
            .Select(bookScore => bookScore.book)
            .Select(book => (this._recommendedBooksFormatter.ToCitation(book), book.Id))
            .ToList();
    }

    private int GetRecommendationScore(Book book, string[] targetWords) =>
        book.Title.SplitIntoWords().Intersect(targetWords).Count();

    private List<PriceLine> CalculatePriceLines(Money originalPrice)
    {
        List<PriceLine> priceLines = new();

        priceLines.Add(new("Original price", originalPrice));

        priceLines.AddRange(Discounts
            .GetDiscounts()
            .GetDiscountAmounts(originalPrice, new(this.Book))
            .Select(a => new PriceLine(a.Label, a.Amount)));

        if (priceLines.Count > 1)
        {
            PriceLine finalPrice =
                new("Final price", originalPrice - priceLines.Skip(1).Sum(line => line.Amount));
            priceLines.Add(finalPrice);
        }

        return priceLines;
    }
}

