namespace Demo.Models;

public abstract record PublicationDate;

public sealed record FullDate(DateOnly Date);
public sealed record YearMonth(int Year, int Month);
public sealed record Year(int Number) : PublicationDate;