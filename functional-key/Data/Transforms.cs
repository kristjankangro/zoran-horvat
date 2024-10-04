using Demo.Models;

namespace Demo.Data;

public static class Filtering
{
    /// <summary>
    /// Filter delegate extension 
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="dataSource"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>FilteredDataSource delegate</returns>
    public static FilteredDataSource<T> For<T>(this Filter<T> filter, DataSource<T> dataSource)
    {
        return phrase => filter(dataSource, phrase);
    }
}