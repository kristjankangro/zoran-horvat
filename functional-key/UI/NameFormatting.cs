namespace Demo.UI;

using Demo.Models;
using Demo.Processes;

public static class NameFormatting
{
    // public static string AcademicNameFormatter(NameType name) =>
    //     name switch
    //     {
    //         FullName { First: var first, Last: var last } => $"{last}, {first[..1]}",
    //         Mononym { Name: var mononym } => mononym,
    //         _ => throw new ArgumentException("Invalid name type", nameof(name))
    //     };
    public static NameFormatter AcademicNameFormatter => (NameType name) => name.Match(
        (first, last) => $"{last}",
        mononym => mononym);

    public static NameListFormatterExt AcademicNameListFormatter =>
        (NameFormatter nameFormatter, params NameType[] names) =>
            names switch
            {
                { Length: 0 } => string.Empty,
                [ var single ] => nameFormatter(single),
                [ var first, var second ] => $"{nameFormatter(first)}, {nameFormatter(second)}",
                [ var first, ..] => $"{nameFormatter(first)} et al.",
            };

    public static NameFormatter FullNameFormatter => (NameType name) => name.Match(
        (first, last) => $"{first} {last}",
        mononym => mononym);

    public static NameListFormatterExt CsvNameListFormatter => (NameFormatter nameFormatter, params NameType[] names) =>
        string.Join(", ", names.Select(nameFormatter.Invoke));
}
