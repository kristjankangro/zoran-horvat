namespace Demo.Models;

public class BookAuthor
{
	private BookAuthor() { }		// Used by EF Core

	public BookAuthor(Book book, Author author, int ordinal) =>
		(BookId, Book, AuthorId, Author, Ordinal) = (book.Id, book, author.Id, author, ordinal);

    public int BookId { get; set; }
    public Book Book { get; set; } = default!;

    public int AuthorId { get; set; }
    public Author Author { get; set; } = default!;

	public int Ordinal { get; set; }
}