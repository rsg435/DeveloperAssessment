using Data;

Console.WriteLine("Creating SQLite database and tables...");

using (var db = new AppDbContext())
{
	var message = db.Database.EnsureCreated() ? "Database created successfully" : "Database already exists";
	Console.WriteLine(message);
}

Console.WriteLine("Adding customer and order data...");

//add customers and orders