using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable CS8618
namespace Sales.API.Models {
    public class SalePrice {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public long Price { get; set; }
        public DateTime UpdateDate { get; set; }

        public Guid SaleId { get; set; }
        [ForeignKey("SaleId")]
        public Sale Sale { get; set; }

        
    }
}