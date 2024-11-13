using Demo.Data;
using Demo.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Demo.Common;
using System.Globalization;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var services = builder.Services;

services.AddDbContext<BookstoreDbContext>(options =>
	options.UseSqlServer(configuration.GetConnectionString("Bookstore")));

services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

services.AddScoped<DataSeed>();

var app = builder.Build();

app.MapGet("/", async (DataSeed dataSeed) => 
{
	await dataSeed.SeedBooks();
	return "The database is seeded!";
});

app.MapGet("/authors", (BookstoreDbContext dbContext, HttpContext httpContext) => "Nothing to see");

app.MapGet("/books", (BookstoreDbContext dbContext, HttpContext httpContext) => "Nothing to see");

app.MapGet("/publishers", (BookstoreDbContext dbContext, HttpContext httpContext) => "Nothing to see");
app.Run();