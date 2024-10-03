namespace Demo.Processes;

using Demo.Models;

public delegate string NameFormatter(NameType name);
public delegate string NameListFormatter(params NameType[] names);
public delegate string NameListFormatterExt(NameFormatter nameFormatter, params NameType[] names);

public static class NameFormatters
{
    public static NameListFormatter Apply(this NameListFormatterExt formatter, NameFormatter nameFormatter) =>
        names => formatter(nameFormatter, names);
}