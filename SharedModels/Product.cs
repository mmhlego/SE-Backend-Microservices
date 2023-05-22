using System.ComponentModel.DataAnnotations;

#pragma warning disable CS8618
namespace SharedModels {
    public class Product {
        [Key]
        public Guid RowId { get; set; } = Guid.NewGuid();
        public Guid ProductId { get; set; }
        public Guid Subcategory { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = "";
        public ProductStates State { get; set; }
    }
}