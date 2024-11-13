namespace DemoApi.Models;

public abstract class PartialDate
{
    public abstract DateOnly Beginning { get; }
}

public class FullDate(DateOnly date) : PartialDate
{
    public DateOnly Date { get; private set; }
    public override DateOnly Beginning => Date;
}

public class YearMonth(int year, int month) : PartialDate
{
    private DateOnly Date => new DateOnly(year, month, 1);
    public int Month => Date.Month;
    public int Year => Date.Year;
    public override DateOnly Beginning => Date;
}

public class Year(int year) : PartialDate
{
    private DateOnly Date => new DateOnly(year, 1, 1);
    public int YearNumber => Date.Year;
    public override DateOnly Beginning => Date;
}