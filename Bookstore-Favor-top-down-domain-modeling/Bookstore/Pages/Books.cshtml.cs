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

    public IEnumerable<BookHeader> Books { get; set; } =
        Array.Empty<BookHeader>();
    
    public BooksModel(ILogger<BooksModel> logger, BookstoreContext context)
    {
        _logger = logger;
        _context = context;
    }

    public void OnGet()
    {
        this.Books = this._context.Books
            .Include(book => book.Author)
            .ToList()
            .ToBookHeaders();
    }
}