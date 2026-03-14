using System.Text.Json.Serialization;

namespace Data.Models
{
	public class Order
	{
		public int Id { get; set; }
		public required string ProductName { get; set; }
		public int Quantity { get; set; }
		public decimal UnitPrice { get; set; }
		public int CustomerId { get; set; }
		public DateTime TimeStamp { get; set; }
		[JsonIgnore]
		public Customer? Customer { get; set; }

	}
}
