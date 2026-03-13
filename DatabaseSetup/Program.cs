using Data;
using Data.Models;
using System.Text.Json;

Console.WriteLine("Creating SQLite database and tables...");

//Create database if it doesn't exist
using (var context = new AppDbContext())
{
	var message = context.Database.EnsureCreated() ? "Database created successfully." : "Database already exists."; 
	Console.WriteLine(message);

	Console.WriteLine("Adding customer and order data...");

	//Add customers and orders, checking if the data already exists beforehand
	if(context.Customers.Any() || context.Orders.Any()) 
	{
		Console.WriteLine("Data already exists in database, ending script.");
		return;
	}

	var json = File.ReadAllText("../../../../dataset.json"); //extract data from JSON file

	var customers = JsonSerializer.Deserialize<List<Customer>>(json)!; //deserialise data into Customer objects

	//insert the data into the database and commit changes
	context.Customers.AddRange(customers);
	context.SaveChanges();

	Console.WriteLine("Customer and order data added.");

}








