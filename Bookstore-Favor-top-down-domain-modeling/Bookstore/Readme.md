# Source Code Bundle

## Action list
1. Create prototype ViewModel (tuple)
2. Create list view, Razor view 
3. Replace prototype ViewModel with BookHeader model
4. Create domain model Book with Title only, pretending that AuthorName is complex implementation
5. ...9.40

## Video

This package contains source code for the video [Favor Top-Down Domain Modeling in ASP.NET with Entity Framework Core](https://youtu.be/oXyXHJyltjA).

## Prerequisites

The source code depends on:

  - .NET 7.0 or later - The best option is to install the latest [.NET SDK](https://dotnet.microsoft.com/en-us/download/visual-studio-sdks).
  - SQL Server LocalDB - Follow the installation instructions on [this](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) address.

## Running the demo

The demo depends on the database. The connection string is specified in the `appsettings.Development.json` file. It is targeting the SQL Server LocalDB database named `Demo`, and you can keep it that way while playing with code.

You will need to bring the database into the state expected by the application before running it. To do so, open the terminal in the directory where the solution file is located, and run these two instructions:

```
dotnet ef database drop -f --project .\Bookstore\
dotnet ef database update --project .\Bookstore\
```

Once the database has been prepared, you can continue with running the demo application:

```
dotnet run --project .\Bookstore
```

Please note that the demo is the ASP.NET Core Web application, and you will need to access it through the browser.