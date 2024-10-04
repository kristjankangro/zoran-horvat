using Demo.Models;

namespace Demo.Data;

public static class Filtering
{
    //extension returns function
    public static FilteredDataSource<T> For<T>(this Filter<T> filter, DataSource<T> dataSource)
    {
        return phrase => filter(dataSource, phrase);
    }
}