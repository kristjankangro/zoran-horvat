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

    public IEnumerable<(string AuthorName, string Title)> Books { get; set; } = Array.Empty<(string, string)>();

    /// <summary>
    /// Just for mocking data as first step
    /// </summary>
    public IEnumerable<(string AuthorName, string Title)> LoremIpsumBooks =>
        new[]
        {
            ("Thomas Mann", "Doctor Faustus"),
            ("Aristotle", "Rhetoric"),
            (string.Empty, "1001 nights")
        };

    public BooksModel(ILogger<BooksModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        this.Books = LoremIpsumBooks.ToList();
        // this.Books = this._context.Books
        //     .Include(book => book.Author)
        //     .ToList()
        //     .ToBookHeaders();
    }
}