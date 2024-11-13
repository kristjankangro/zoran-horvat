namespace Demo.Models;

public class PartialDate
{
    public DateOnly Date { get; private set; }
    public bool IsDaySpecified { get; private set; }
    public bool IsMonthSpecified { get; private set; }

    public static PartialDate Create(DateOnly fullDate) =>
        new(fullDate, true, true);

    public static PartialDate Create(int year, int month) =>
        new(new DateOnly(year, month, 1), false, true);

    public static PartialDate Create(int year) =>
        new(new DateOnly(year, 1, 1), false, false);

    private PartialDate(DateOnly date, bool isDaySpecified, bool isMonthSpecified) =>
        (Date, IsDaySpecified, IsMonthSpecified) = (date, isDaySpecified, isMonthSpecified);
}