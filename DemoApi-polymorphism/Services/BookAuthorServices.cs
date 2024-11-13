using Demo.Data;
using Demo.Models;

namespace Demo.Services;

public class BookAuthorServices
{
    private readonly BookstoreDbContext _dbContext;

    public BookAuthorServices(BookstoreDbContext dbContext) => _dbContext = dbContext;
    
    public async Task<Book> AppendAuthor(int bookId, Author author)
    {
        Book? book = await _dbContext.Books.FindAsync(bookId);
        ArgumentNullException.ThrowIfNull(book);

        book.Authors.Add(new BookAuthor(book, author, book.Authors.Count));
        await _dbContext.SaveChangesAsync();
        return book;
    }

    public async Task<bool> RemoveAuthor(int bookId, int authorId)
    {
        Book? book = await _dbContext.Books.FindAsync(bookId);
        ArgumentNullException.ThrowIfNull(book);

        BookAuthor? bookAuthor = book.Authors.FirstOrDefault(ba => ba.AuthorId == authorId);
        if (bookAuthor is null) return false;

        book.Authors.Remove(bookAuthor);
        foreach (BookAuthor later in book.Authors.Where(ba => ba.Ordinal > bookAuthor.Ordinal))
        {
            later.Ordinal -= 1;
        }

        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> MoveAuthorUp(int bookId, Author author)
    {
        Book? book = await _dbContext.Books.FindAsync(bookId);
        if (book is null) throw new ArgumentException("Book not found");

        BookAuthor? second = book.Authors.FirstOrDefault(ba => ba.AuthorId == author.Id);
        if (second is null) return false;

        BookAuthor? first = book.Authors.FirstOrDefault(ba => ba.Ordinal == second.Ordinal - 1);
        if (first is null) return false;

        second.Ordinal -= 1;
        first.Ordinal += 1;

        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> MoveAuthorDown(int bookId, Author author)
    {
        Book? book = await _dbContext.Books.FindAsync(bookId);
        if (book is null) throw new ArgumentException("Book not found");

        BookAuthor? first = book.Authors.FirstOrDefault(ba => ba.AuthorId == author.Id);
        if (first is null) return false;

        BookAuthor? second = book.Authors.FirstOrDefault(ba => ba.Ordinal == first.Ordinal + 1);
        if (second is null) return false;

        first.Ordinal += 1;
        second.Ordinal -= 1;

        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> MoveAuthorToFront(int bookId, Author author)
    {
        Book? book = await _dbContext.Books.FindAsync(bookId);
        if (book is null) throw new ArgumentException("Book not found");

        BookAuthor? bookAuthor = book.Authors.FirstOrDefault(ba => ba.AuthorId == author.Id);
        if (bookAuthor is null) return false;

        foreach (BookAuthor previous in book.Authors.Where(ba => ba.Ordinal < bookAuthor.Ordinal))
        {
            previous.Ordinal += 1;
        }
        bookAuthor.Ordinal = 1;

        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> MoveAuthorToBack(int bookId, Author author)
    {
        Book? book = await _dbContext.Books.FindAsync(bookId);
        if (book is null) throw new ArgumentException("Book not found");

        BookAuthor? bookAuthor = book.Authors.FirstOrDefault(ba => ba.AuthorId == author.Id);
        if (bookAuthor is null) return false;

        foreach (BookAuthor previous in book.Authors.Where(ba => ba.Ordinal > bookAuthor.Ordinal))
        {
            previous.Ordinal -= 1;
        }
        bookAuthor.Ordinal = book.Authors.Count;

        await _dbContext.SaveChangesAsync();
        return true;
    }
}

