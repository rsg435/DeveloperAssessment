using Data;
using Data.Helpers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var databasePath = Path.Join(PathHelper.GetLocalAppDataPath(), "mydatabase.db");

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite($"Data Source={databasePath}"));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/customers/{id}", async (int id, AppDbContext db) =>
{
	var customer = await db.Customers
		.Include(c => c.Orders)
		.FirstOrDefaultAsync(c => c.Id == id);

	if (customer == null)
		return Results.NotFound("No customer data found for this ID.");

	return Results.Ok(customer);
});

app.Run();
