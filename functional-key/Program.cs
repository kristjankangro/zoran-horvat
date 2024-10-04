using Demo;
using Demo.Data;
using static Demo.Data.BooksFiltering;
using static Demo.Data.BooksData;


await Runner.PromptAndReport(BooksFiltered.For(GetBooks));



SeparateDelegate.Run(Generator.Go + SomeDelegateContainer.DoDelegate + SomeDelegateContainer.DoDelegate2);
