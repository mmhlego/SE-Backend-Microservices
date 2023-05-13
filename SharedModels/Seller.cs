using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable CS8618
namespace SharedModels {
    public class Seller {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Information { get; set; } = "";
        public string Address { get; set; } = "";

        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}