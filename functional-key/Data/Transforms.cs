using Demo.Models;

namespace Demo.Data;

public static class Filtering
{
    public static FilteredDataSource<T> For<T>(this Filter<T> filter, DataSource<T> dataSource) =>
        phrase => filter(dataSource, phrase);
}