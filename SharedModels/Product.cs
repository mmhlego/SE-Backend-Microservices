using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Users.API.Models;

namespace Products.API.Models {
    public class Product {
        [Key]
        public Guid RowId { get; set; }
        public Guid ProductId { get; set; }
        public Guid Subcategory { get; set; }
        public Guid Sate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [ForeignKey("Sate")]
        public ProductStates States { get; set; }
    }
}