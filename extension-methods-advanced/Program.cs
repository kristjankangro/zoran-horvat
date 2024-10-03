// using Functional;
using LanguageExt;
using OOP;


 #region part 2 functional
//
// var ctx = new Context();
//
// var guid = Guid.NewGuid();
//
// var person = ctx.Persons.Find(guid);
// if (person is not null) Console.WriteLine(person.FirstName);
//
// var maybe = Prelude.Optional(ctx.Persons.Find(guid));
// Console.WriteLine(maybe.Do(x => Console.WriteLine($"{x.FirstName} {x.LastName}")));
//
// //usage of dbcontext extension
// void Process(Person person) => Console.WriteLine($"{person.FirstName} {person.LastName}");
//
// ctx.Persons.TryFind(guid).Do(Process);
//
 #endregion

#region part 3 oop + prc + functional

var people = new[]
{
    new Person3(Guid.NewGuid(), "John", "Doe", 1.82f),
    new Person3(Guid.NewGuid(), "John", "Doe", 1.52f),
    new Person3(Guid.NewGuid(), "John", "Doe", 1.72f),
    new Person3(Guid.NewGuid(), "John", "Doe", 1.62f),
};

var heightDesc = Comparer<Person3>.Create((a, b) =>
    -a.Height.CompareTo(b.Height));
IEnumerable<Person3> tallest = people.Smallest(1, heightDesc);
foreach( var p in tallest) 
    Console.WriteLine($"{p.FirstName} {p.LastName} {p.Height}");
#endregion