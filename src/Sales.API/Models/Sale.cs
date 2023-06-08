using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SharedModels;

#pragma warning disable CS8618
namespace Sales.API.Models {
    public class Sale {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public int Amount { get; set; } = 0;
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User user { get; set; }

        public List<SalePrice> Prices { get; set; }
    }
}