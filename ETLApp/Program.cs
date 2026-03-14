using Data.Helpers;
using Data;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Running ETL pipeline...");

//set up database connection
var path = PathHelper.GetLocalAppDataPath();
var databasePath = Path.Join(path, "mydatabase.db");

var options = new DbContextOptionsBuilder<AppDbContext>()
	.UseSqlite($"Data Source={databasePath}")
	.Options;

using (var context = new AppDbContext(options))
{
	//get active customers 
	var customers = context.Customers
	.Where(c => c.Status.Equals("active"))
	.Include(c => c.Orders)
	.ToList();

	if (!customers.Any())
	{
		Console.WriteLine("No customer data found.");
	}

	//set up data rows
	var rows = customers
	.SelectMany(c => c.Orders.Select(o => new
	{
		Name = $"{c.FirstName} {c.LastName}",
		Email = c.Email,
		Product = o.ProductName,
		Quantity = o.Quantity,
		UnitPrice = o.UnitPrice,
		TotalValue = o.Quantity * o.UnitPrice,
		TimeStamp = o.TimeStamp,
	}));

	//set up headers
	var lines = new List<string>
	{
	"Name,Email,Product,Quantity,UnitPrice,TotalValue,TimeStamp"
	};

	//add data rows
	lines.AddRange(rows.Select(r =>
		$"{r.Name},{r.Email},{r.Product},{r.Quantity},{r.UnitPrice},{r.TotalValue},{r.TimeStamp}"
	));

	//write to file
	var csvPath = Path.Join(path, "orders.csv");
	File.WriteAllLines(csvPath, lines);

	//open output folder
	PathHelper.OpenFolder(path);

	Console.WriteLine($"Pipeline completed - output has been saved to {csvPath}");
}