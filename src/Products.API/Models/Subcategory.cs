using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable CS8618
namespace Products.API.Models {
    public class Subcategory {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }

        public Guid CategoryId { get; set; }
    }
}