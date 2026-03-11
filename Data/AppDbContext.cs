using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data
{
	public class AppDbContext : DbContext
	{
		public string DatabasePath { get; }

        public AppDbContext()
        {
			var folder = Environment.SpecialFolder.LocalApplicationData;
			var path = Environment.GetFolderPath(folder);
			DatabasePath = Path.Join(path, "mydatabase.db");
		}
        protected override void OnConfiguring(DbContextOptionsBuilder options)
		=> options.UseSqlite($"Data Source={DatabasePath}");

		public DbSet<Customer> Customers { get; set; }
		public DbSet<Order> Orders { get; set; }

	}
}
