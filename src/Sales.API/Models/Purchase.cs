using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SharedModels;

#pragma warning disable CS8618
namespace Sales.API.Models {
    public class Purchase {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public int TotalPrice { get; set; }
        public string Description { get; set; }
        public DateTime PurchaseDate { get; set; }

        public Guid OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        public Guid CustomerAddressId { get; set; }
        [ForeignKey("CustomerAddressId")]
        public CustomerAddress CustomerAddress { get; set; }

        public Guid? DiscountTokenId { get; set; } = null;
    }
}