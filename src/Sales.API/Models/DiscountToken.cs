using System.ComponentModel.DataAnnotations;

#pragma warning disable CS8618
namespace Sales.API.Models {
    public class DiscountToken {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Token { get; set; }
        public DiscountTokenTypes Type;
        public int Amount { get; set; }
        public int AvailableCount { get; set; }
        public DateTime ExpireDate { get; set; }

        public List<Purchase> Purchase { get; set; }
    }
}