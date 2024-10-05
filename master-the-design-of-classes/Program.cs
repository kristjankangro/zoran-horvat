using System.Globalization;
using Demo.Models;


var authors = new AuthorsList(
[
    new Author("Enn Lepp", CultureInfo.CurrentCulture),
    new Author("Peeter Pärn", CultureInfo.CurrentCulture)
]);

var publisher = new Publisher();
IEdition edition = new SeasonalEdition(SeasonalEdition.YearSeason.Summer, 2025);
var release = new Release(publisher, edition, new Year(2025), new CultureInfo("et-ET"));

var book = new Book("Greatest book eva!", authors, release);