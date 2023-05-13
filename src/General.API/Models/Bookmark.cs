using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SharedModels;

#pragma warning disable CS8618
namespace General.API.Models {
    public class Bookmark {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}