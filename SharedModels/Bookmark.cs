using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SharedModels;

#pragma warning disable CS8618
namespace SharedModels {
    public class Bookmark {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; }

        public Guid ProductId { get; set; }
    }
}