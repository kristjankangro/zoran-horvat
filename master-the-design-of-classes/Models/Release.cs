using System.Globalization;

namespace Demo.Models;

public class Release
{
    public Publisher Publisher { get; set; }
    public IEdition Edition { get; set; }
    public PublicationDate PublicationDate { get; set; }
    public CultureInfo Culture { get; set; }

    public Release(Publisher publisher, IEdition edition, PublicationDate publicationDate, CultureInfo culture)
    {
        Publisher = publisher;
        Edition = edition;
        PublicationDate = publicationDate;
        Culture = culture;
    }
    
}