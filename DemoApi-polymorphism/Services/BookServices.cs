using Demo.Models;
using Demo.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using DemoApi.Models;

namespace Demo.Services;

public class BookServices
{
    private readonly BookstoreDbContext _dbContext;

    public BookServices(BookstoreDbContext dbContext) => _dbContext = dbContext;

    public async Task<IEnumerable<Book>> GetBooksFromNewest(int authorId) =>
        (await _dbContext.Books
            .Where(book => book.Authors.Any(author => author.AuthorId == authorId))
            .ToListAsync()).OrderByDescending(book => book.Publication.GetBeginning(DateOnly.MaxValue));

    public async Task<Book> CreateBook(string title, string cultureName, PublicationInfo publication, Edition edition, IEnumerable<Author> authors)
    {
        List<BookAuthor> bookAuthors = authors
            .Select((author, index) => new BookAuthor(null!, author, index + 1))
            .ToList();
        
        Book newBook = new(0, title, cultureName, publication, edition, bookAuthors);

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

