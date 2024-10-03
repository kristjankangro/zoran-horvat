namespace Demo.Models;

public delegate Task<IEnumerable<T>> FilteredDataSource<T>(string phrase);
