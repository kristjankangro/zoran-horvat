namespace DemoApi.Models;

public class PublicationInfo
{
    public PartialDate? PublicationDate { get; private set; }
    public bool IsPublished { get; private set; }

    public static PublicationInfo CreatePublished(PartialDate date) =>
        new(date, true);
    
    public static PublicationInfo CreatePlanned(PartialDate date) =>
        new(date, false);
    
    public static PublicationInfo CreateUnpublished() =>
        new(null, false);

    public PublicationInfo(PartialDate? publicationDate, bool isPublished) =>
        (PublicationDate, IsPublished) = (publicationDate, isPublished);
    
    public DateOnly GetBeginning(DateOnly orElse) => PublicationDate?.Beginning ?? orElse;
}