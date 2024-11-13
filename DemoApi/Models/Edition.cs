namespace Demo.Models;

public class Edition
{
	public int? Number { get; private set; }
    public YearSeason? Season { get; private set; }
    public int? Year { get; private set; }

	public static Edition CreateOrdinal(int number) =>
		new Edition(
			number > 0 ? number : throw new ArgumentOutOfRangeException(nameof(number)),
			null, null);

	public static Edition CreateSeasonal(YearSeason season, int year) =>
		new Edition(null, season,
			year > 0 ? year : throw new ArgumentOutOfRangeException(nameof(year)));

	private Edition(int? number, YearSeason? season, int? seasonYear) =>
		(Number, Season, Year) = (number, season, seasonYear);
}