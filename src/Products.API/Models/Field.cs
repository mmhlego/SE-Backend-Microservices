using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable CS8618
namespace Products.API.Models {
    public class Field {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }

        public Guid SubcategoryId { get; set; }
       
    }
}