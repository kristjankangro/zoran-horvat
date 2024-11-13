using System.Globalization;

namespace Demo.Models;

public class Book
{
    public int Id { get; private set; }
    public string Title { get; set; }
    public string CultureName { get; set; }
    public PublicationInfo Publication { get; private set; }
    public Edition? Edition { get; private set; }    

    public ICollection<BookAuthor> Authors { get; private set; }

    public Book(int id, string title, string cultureName, PublicationInfo publication, Edition edition, IEnumerable<BookAuthor> authors)
    {
        Id = id >= 0 ? id : throw new ArgumentOutOfRangeException(nameof(id));
        Title = !string.IsNullOrWhiteSpace(title) ? title : throw new ArgumentException(nameof(title));
        CultureName = CultureInfo.GetCultureInfo(cultureName).Name;     // Throws if invalid culture
        Publication = publication;
        Edition = edition;
        Authors = authors.ToList();
    }
}
