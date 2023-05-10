using System.ComponentModel.DataAnnotations;

#pragma warning disable CS8618
namespace Products.API.Models {
    public class Category {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string IconName { get; set; }

        public List<Subcategory> SubCategories { get; set; }
    }
}