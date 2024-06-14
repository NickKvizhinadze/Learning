// See https://aka.ms/new-console-template for more information

using Dometrain.EFCore.DatabaseFirst.Context;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");

var context = new AdventureWorksDbContext();

var customers = context.Customers
    .Where(c => c.FirstName.StartsWith("C"))
    .Include(customer => customer.SalesOrderHeaders)
    .ToList();

foreach (var customer in customers)
{
    Console.WriteLine($"{customer.FullName} - {customer.SalesOrderHeaders?.Count}");
}