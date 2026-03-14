using Data;
using Data.Helpers;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

Console.WriteLine("Creating SQLite database and tables...");

//set up database connection
var path = PathHelper.GetLocalAppDataPath();
var databasePath = Path.Join(path, "mydatabase.db");

var options = new DbContextOptionsBuilder<AppDbContext>()
	.UseSqlite($"Data Source={databasePath}")
	.Options;

//Create database if it doesn't exist
using (var context = new AppDbContext(options))
{
	bool created = context.Database.EnsureCreated();

	//open the folder the database is in
	PathHelper.OpenFolder(path);

	var message = created ? "Database created successfully." : "Database already exists.";

	Console.WriteLine(message);

	Console.WriteLine("Adding customer and order data...");

	//Add customers and orders, checking if the data already exists beforehand
	if(context.Customers.Any() || context.Orders.Any()) 
	{
		Console.WriteLine("Data already exists in database, ending script.");
		return;
	}

	var filePath = Path.Combine(
	AppContext.BaseDirectory,
	"Data",
	"dataset.json");

	var json = File.ReadAllText(filePath); //extract data from JSON file

	var customers = JsonSerializer.Deserialize<List<Customer>>(json)!; //deserialise data into Customer objects

	//insert the data into the database and commit changes
	context.Customers.AddRange(customers);
	context.SaveChanges();

	Console.WriteLine("Customer and order data added.");

}








