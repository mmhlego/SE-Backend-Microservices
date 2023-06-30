using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SharedModels;

#pragma warning disable CS8618
namespace Products.API.Models
{
	public class ProductImage
	{
		[Key]
		public Guid Id { get; set; }
		public string ImageUrl { get; set; }

		public Guid ProductId { get; set; }
	}
}