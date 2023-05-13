using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SharedModels;

#pragma warning disable CS8618
namespace General.API.Models {
    public class like {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool Like { get; set; }
        public Guid TargetId { get; set; }
        public LikeTypes Type { get; set; }

        public Guid CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
    }
}