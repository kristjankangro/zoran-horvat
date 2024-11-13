namespace Demo.Common;

public static class FunctionalExtensions
{
	public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
	{
		foreach (var item in sequence)
		{
			action(item);
		}
	}
}