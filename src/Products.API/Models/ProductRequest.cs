using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SharedModels;

#pragma warning disable CS8618
namespace Products.API.Models {
    public class ProductRequest {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public ProductRequestTypes Type { get; set; }

        public Guid ProductRowId { get; set; }
        [ForeignKey("ProductRowId")]
        public Product Product { get; set; }

        public Guid SellerId { get; set; }
        [ForeignKey("SellerId")]
        public Seller Seller { get; set; }
    }
}