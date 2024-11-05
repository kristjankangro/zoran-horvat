using Demo.Models;
using Demo.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Demo.Services;

public class BookServices
{
    private readonly BookstoreDbContext _dbContext;

    public BookServices(BookstoreDbContext dbContext) => _dbContext = dbContext;

    public async Task<Book> CreateUnpublishedOrdinalEdition(string title, string cultureName, int editionNumber, params Author[] authors) =>
        await CreateBook(title, cultureName, null, false, false, false, editionNumber, null, null, authors);
    
    public async Task<Book> CreatePlannedOrdinalEdition(string title, string cultureName, DateOnly plannedDate, int editionNumber, params Author[] authors) =>
        await CreateBook(title, cultureName, plannedDate, true, true, false, editionNumber, null, null, authors);
    
    public async Task<Book> CreatePlannedOrdinalEdition(string title, string cultureName, int plannedYear, int plannedMonth, int editionNumber, params Author[] authors) =>
        await CreateBook(title, cultureName, new DateOnly(plannedYear, plannedMonth, 1), false, true, false, editionNumber, null, null, authors);
    
    public async Task<Book> CreatePlannedOrdinalEdition(string title, string cultureName, int plannedYear, int editionNumber, params Author[] authors) =>
        await CreateBook(title, cultureName, new DateOnly(plannedYear, 1, 1), false, false, true, editionNumber, null, null, authors);
    
    public async Task<Book> CreatePublishedOrdinalEdition(string title, string cultureName, DateOnly publishedDate, int editionNumber, params Author[] authors) =>
        await CreateBook(title, cultureName, publishedDate, true, true, true, editionNumber, null, null, authors);
    
    public async Task<Book> CreatePublishedOrdinalEdition(string title, string cultureName, int publishedYear, int publishedMonth, int editionNumber, params Author[] authors) =>
        await CreateBook(title, cultureName, new DateOnly(publishedYear, publishedMonth, 1), false, true, true, editionNumber, null, null, authors);
    
    public async Task<Book> CreatePublishedOrdinalEdition(string title, string cultureName, int publishedYear, int editionNumber, params Author[] authors) =>
        await CreateBook(title, cultureName, new DateOnly(publishedYear, 1, 1), false, false, true, editionNumber, null, null, authors);

    public async Task<Book> CreateUnpublishedSeasonalEdition(string title, string cultureName, YearSeason editionSeason, int editionYear, params Author[] authors) =>
        await CreateBook(title, cultureName, null, false, false, false, null, editionSeason, editionYear, authors);
    
    public async Task<Book> CreatePlannedSeasonalEdition(string title, string cultureName, DateOnly plannedDate, YearSeason editionSeason, int editionYear, params Author[] authors) =>
        await CreateBook(title, cultureName, plannedDate, true, true, false, null, editionSeason, editionYear, authors);

    public async Task<Book> CreatePlannedSeasonalEdition(string title, string cultureName, int plannedYear, int plannedMonth, YearSeason editionSeason, int editionYear, params Author[] authors) =>
        await CreateBook(title, cultureName, new DateOnly(plannedYear, plannedMonth, 1), false, true, false, null, editionSeason, editionYear, authors);

    public async Task<Book> CreatePlannedSeasonalEdition(string title, string cultureName, int plannedYear, YearSeason editionSeason, int editionYear, params Author[] authors) =>
        await CreateBook(title, cultureName, new DateOnly(plannedYear, 1, 1), false, false, true, null, editionSeason, editionYear, authors);

    public async Task<Book> CreatePublishedSeasonalEdition(string title, string cultureName, DateOnly publishedDate, YearSeason editionSeason, int editionYear, params Author[] authors) =>
        await CreateBook(title, cultureName, publishedDate, true, true, true, null, editionSeason, editionYear, authors);
    
    public async Task<Book> CreatePublishedSeasonalEdition(string title, string cultureName, int publishedYear, int publishedMonth, YearSeason editionSeason, int editionYear, params Author[] authors) =>
        await CreateBook(title, cultureName, new DateOnly(publishedYear, publishedMonth, 1), false, true, true, null, editionSeason, editionYear, authors);
    
    public async Task<Book> CreatePublishedSeasonalEdition(string title, string cultureName, int publishedYear, YearSeason editionSeason, int editionYear, params Author[] authors) =>
        await CreateBook(title, cultureName, new DateOnly(publishedYear, 1, 1), false, false, true, null, editionSeason, editionYear, authors);

    private async Task<Book> CreateBook(
        string title, string cultureName,
        DateOnly? publicationDate, bool isDaySpecified, bool isMonthSpecified, bool isPublished,
        int? editionNumber, YearSeason? editionSeason, int? seasonalEditionYear,
        IEnumerable<Author> authors)
    {
        List<BookAuthor> bookAuthors = authors
            .Select((author, index) => new BookAuthor(null!, author, index + 1))
            .ToList();
        
        Book newBook = new(0, title, cultureName,
                           publicationDate, isDaySpecified, isMonthSpecified, isPublished,
                           editionNumber, editionSeason, seasonalEditionYear, bookAuthors);

        foreach (BookAuthor bookAuthor in bookAuthors) bookAuthor.Book = newBook;

        _dbContext.Books.Add(newBook);
        await _dbContext.SaveChangesAsync();

        return newBook;
    }

    public async Task<IEnumerable<Book>> GetBooks(int? id, string? titlePhrase)
    {
        IQueryable<Book> books = _dbContext.Books;
        if (id.HasValue) books = books.Where(book => book.Id == id.Value);
        if (!string.IsNullOrWhiteSpace(titlePhrase)) books = books.Where(book => book.Title.Contains(titlePhrase));

        return await books.ToListAsync();
    }

    public async Task<bool> DeleteBook(int id)
    {
        if (await _dbContext.Books.FindAsync(id) is Book book)
        {
            _dbContext.Books.Remove(book);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<bool> UpdateTitle(int id, string title)
    {
        if (await _dbContext.Books.FindAsync(id) is Book book)
        {
            book.Title = SanitizeTitle(title, CultureInfo.GetCultureInfo(book.CultureName));
            await _dbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<bool> UpdateCulture(int id, string cultureName)
    {
        if (await _dbContext.Books.FindAsync(id) is Book book)
        {
            book.CultureName = SanitizeCulture(cultureName);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }

    private static string SanitizeCulture(string cultureName) =>
        CultureInfo.GetCultureInfo(cultureName).Name;               // May throw

    private static string SanitizeTitle(string title, CultureInfo culture)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        // Capitalize letters according to culture, etc.
        return title;
    }
}

