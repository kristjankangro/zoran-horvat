using Bookstore.Data;
using Bookstore.Models;
using Bookstore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Pages;

public class BooksModel : PageModel
{
    private readonly ILogger<BooksModel> _logger;
    private readonly BookstoreContext _context;

    public IEnumerable<BookHeader> Books { get; set; } = Array.Empty<BookHeader>();

    /// <summary>
    /// Just for mocking data as first step
    /// </summary>
    public IEnumerable<Book> LoremIpsumBooks =>
        new Book[]
        {
            Book.CreateNew("Doctor Faustus"),
            Book.CreateNew("Rhetoric"),
            Book.CreateNew("1001 nights"),
        };

    /// <summary>
    /// mock author while authorname is not implemented
    /// </summary>
    private IEnumerable<string> LoremAuthors => Enumerable.Range(0, 1_000)
        .SelectMany(_ => new[] { "Lorem", "Lorem Ipsum", string.Empty });

    public BooksModel(ILogger<BooksModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        this.Books = LoremIpsumBooks
            .Zip(LoremAuthors, (b, a) => new BookHeader(b.Title, a)).ToList();
        // this.Books = this._context.Books
        //     .Include(book => book.Author)
        //     .ToList()
        //     .ToBookHeaders();
    }
}