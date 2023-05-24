using Products.API.Models;

namespace Products.API.Services {
    public interface ICategoryService {
        List<Category> GetCategories();
        void AddCategory(string title, string iconName);
        void UpdateCategory(Category category);
        List<Subcategory> GetSubcategories(Guid categoryId);
        void AddSubcategory(Guid categoryId, string title);
        void UpdateSubcategory(Subcategory subcategory);
    }
}