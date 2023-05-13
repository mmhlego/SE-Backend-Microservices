using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SharedModels;

#pragma warning disable CS8618
namespace Sales.API.Models {
    public class Sale {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Amount { get; set; } = 0;

        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public Guid SellerId { get; set; }
        [ForeignKey("SellerId")]
        public Seller Seller { get; set; }

        public List<SalePrice> Prices { get; set; }
    }
}