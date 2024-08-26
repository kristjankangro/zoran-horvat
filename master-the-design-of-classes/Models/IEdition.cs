namespace Demo.Models;

public interface IEdition
{
}

public record OrdinalEdition(int Number) : IEdition{}

public class SeasonalEdition : IEdition
{
    public enum YearSeason { Winter, Spring, Summer, Autumn}

    public SeasonalEdition(YearSeason season, int year)
    {
        Season = season;
        Year = year;
    }

    public YearSeason Season { get; set; }
    public int Year { get; set; }
}