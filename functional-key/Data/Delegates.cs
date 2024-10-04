namespace Demo;


/// <summary>DataSource generic delegate</summary>
/// <typeparam name="T">Generic</typeparam>
/// <returns>Task of IEnumerable of generic T type</returns>
public delegate Task<IEnumerable<T>> DataSource<T>();

/// <summary>
/// Filter generic delegate
/// </summary>
/// <param name="dataSource">Delegate as parameter</param>
/// <param name="phrase">Search string</param>
/// <typeparam name="T">Generic</typeparam>
/// <returns>Task of IEnumerable of generic T type</returns>
public delegate Task<IEnumerable<T>> Filter<T>(DataSource<T> dataSource, string phrase);
