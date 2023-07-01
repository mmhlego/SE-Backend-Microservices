using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable CS8618
namespace Sales.API.Models
{
	public class OrderItem
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();
		public int Amount { get; set; }

		public Guid OrderId { get; set; }
		[ForeignKey("OrderId")]
		public Order Order { get; set; }

		public Guid SalePriceId { get; set; }
		[ForeignKey("SalePriceId")]
		public SalePrice SalePrice { get; set; }
	}
}