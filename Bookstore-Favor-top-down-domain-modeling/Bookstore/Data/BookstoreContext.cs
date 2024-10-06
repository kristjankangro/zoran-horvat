using System.Runtime.Serialization;
using Bookstore.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Data;

public class BookstoreContext : DbContext
{
    public DbSet<Book> Books => base.Set<Book>();
    public DbSet<Person> Authors => base.Set<Person>();

    public BookstoreContext(DbContextOptions<BookstoreContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Bookstore");
        
        modelBuilder.Entity<Book>(entity => 
        {
            entity.HasOne(book => book.Author).WithMany().HasForeignKey("AuthorId");
        });

        modelBuilder.Entity<Person>();
    }
}