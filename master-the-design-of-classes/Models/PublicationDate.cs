namespace Demo.Models;

public abstract record PublicationDate;

public sealed record FullDate(DateOnly Date) : PublicationDate;
public sealed record YearMonth(int Year, int Month) : PublicationDate;
public sealed record Year(int Number) : PublicationDate;