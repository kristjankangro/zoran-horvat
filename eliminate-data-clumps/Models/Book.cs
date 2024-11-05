using System.Globalization;

namespace Demo.Models;

public class Book
{
    public int Id { get; private set; }
    public string Title { get; set; }
    public string CultureName { get; set; }

    public DateOnly? PublicationDate { get; private set; }
    public bool IsDaySpecified { get; private set; }
    public bool IsMonthSpecified { get; private set; }
    public bool IsPublished { get; private set; }
    
    public int? EditionNumber { get; private set; }
    public YearSeason? EditionSeason { get; private set; }
    public int? SeasonalEditionYear { get; private set; }

    public ICollection<BookAuthor> Authors { get; private set; }

    public Book(int id, string title, string cultureName,
                DateOnly? publicationDate, bool isDaySpecified, bool isMonthSpecified, bool isPublished,
                int? editionNumber, YearSeason? editionSeason, int? seasonalEditionYear,
                IEnumerable<BookAuthor> authors)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        string validCultureName = CultureInfo.GetCultureInfo(cultureName).Name;     // Throws if invalid
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(editionNumber ?? 0, nameof(editionNumber));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(seasonalEditionYear ?? 0, nameof(seasonalEditionYear));

        if (isPublished && publicationDate is null)
            throw new ArgumentException("Publication date must be specified for published books.");
        
        if (publicationDate is null && isDaySpecified)
            throw new ArgumentException("Day must not be specified if publication date is not specified.");
        
        if (publicationDate is null && isMonthSpecified)
            throw new ArgumentException("Month must not be specified if publication date is not specified.");
        
        if (publicationDate is not null && isDaySpecified && !isMonthSpecified)
            throw new ArgumentException("Month must be specified if day is specified.");
        
        if (editionNumber is null && editionSeason is null && seasonalEditionYear is null)
            throw new ArgumentException("Edition must be specified");
        
        if (editionNumber is not null && editionSeason is not null)
            throw new ArgumentException("Ordinal and seasonal edition must not be specified together");
        
        if (editionNumber is not null && seasonalEditionYear is not null)
            throw new ArgumentException("Ordinal and seasonal edition must not be specified together");
        
        if (editionSeason is not null && seasonalEditionYear is null)
            throw new ArgumentException("Seasonal edition year must be specified");
        
        if (editionSeason is null && seasonalEditionYear is not null)
            throw new ArgumentException("Edition season must be specified");

        Id = id;
        Title = title;
        CultureName = validCultureName;
        
        PublicationDate = publicationDate;
        IsDaySpecified = isDaySpecified;
        IsMonthSpecified = isMonthSpecified;
        IsPublished = isPublished;

        EditionNumber = editionNumber;
        EditionSeason = editionSeason;
        SeasonalEditionYear = seasonalEditionYear;

        Authors = new List<BookAuthor>(authors);
    }
}
