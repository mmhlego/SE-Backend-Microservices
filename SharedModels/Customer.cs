using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable CS8618
namespace SharedModels {
    public class Customer {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public long Balance { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}