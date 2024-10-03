namespace Demo;

public delegate Task<IEnumerable<T>> DataSource<T>();

public delegate Task<IEnumerable<T>> Filter<T>(DataSource<T> dataSource, string phrase);
