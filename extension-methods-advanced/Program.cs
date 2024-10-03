using Functional;
using LanguageExt;


#region part 2 functional

var ctx = new Context();

var guid = Guid.NewGuid();

var person = ctx.Persons.Find(guid);
if (person is not null) Console.WriteLine(person.FirstName);

var maybe = Prelude.Optional(ctx.Persons.Find(guid));
Console.WriteLine(maybe.Do(x => Console.WriteLine($"{x.FirstName} {x.LastName}")));

//usage of dbcontext extension
void Process(Person person) => Console.WriteLine($"{person.FirstName} {person.LastName}");

ctx.Persons.TryFind(guid).Do(Process);

#endregion