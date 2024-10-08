﻿using Bookstore.Data.Seeding;
using Bookstore.Data.Seeding.DataSeed;
using Bookstore.Domain.Models;
using Bookstore.Domain.Discounts;
using Microsoft.EntityFrameworkCore;
using Bookstore.Domain.Invoices;
using Bookstore.Data;
using Bookstore.Data.Implementation;
using Bookstore.Domain.Models.BibliographicFormatters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

ConfigurationManager configuration = builder.Configuration;

string bookstoreConnectionString =
    builder.Configuration.GetConnectionString("BookstoreConnection") ?? string.Empty;
builder.Services.AddDbContext<BookstoreDbContext>(options =>
    options.UseSqlServer(bookstoreConnectionString));

builder.Services.AddScoped<IUnitOfWork, BookstoreDbContext>();

decimal discountFactor = builder.Configuration.GetValue<decimal>("Discounts:RelativeDiscount", 0);
builder.Services.AddScoped<IDiscountServer, DemoDiscountServer>();

builder.Services.AddScoped<Person>(services =>
    {
        BookstoreDbContext dbContext = services.GetService(typeof(BookstoreDbContext)) as BookstoreDbContext ?? throw new Exception("DbContext not found");
        Person author = dbContext.People.Single(person => person.FirstName == "Martin" && person.LastName == "Fowler");
        return author;
    });

builder.Services.AddScoped(typeof(ISpecification<>), typeof(QueryableSpecification<>));

builder.Services.AddScoped<InvoiceFactory>(_ => new InvoiceFactory(
    DateOnly.FromDateTime(DateTime.UtcNow),
    configuration.GetValue<int>("Invoicing:ToleranceDays", 30),
    configuration.GetValue<int>("Invoicing:DelinquencyDays", 10)));

builder.Services.AddSingleton<IAuthorNameFormatter, FullNameFormatter>();
builder.Services.AddSingleton<IAuthorListFormatter, SeparatedAuthorsFormatter>();
builder.Services.AddSingleton<IBibliographicEntryFormatter>(_ => TitleAndAuthorsFormatter.Academic());

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<IDataSeed<Person>, AuthorsSeed>();
    builder.Services.AddScoped<IDataSeed<Book>, BooksSeed>();
    builder.Services.AddScoped<IDataSeed<BookPrice>, BookPricesSeed>();
    builder.Services.AddScoped<IDataSeed<Customer>, CustomersSeed>();
    builder.Services.AddScoped<IDataSeed<InvoiceRecord>, InvoicesSeed>();
}
else
{
    builder.Services.AddScoped<IDataSeed<Person>, NoSeed<Person>>();
    builder.Services.AddScoped<IDataSeed<Book>, NoSeed<Book>>();
    builder.Services.AddScoped<IDataSeed<BookPrice>, NoSeed<BookPrice>>();
    builder.Services.AddScoped<IDataSeed<Customer>, NoSeed<Customer>>();
    builder.Services.AddScoped<IDataSeed<InvoiceRecord>, NoSeed<InvoiceRecord>>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else 
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();

