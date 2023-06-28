using Products.API.Models;

namespace Products.API.Services {
    public interface ICategoryService {
        List<Category> GetCategories();
        void AddCategory(string title, string iconName);
        void UpdateCategory(Category category);
        List<Subcategory> GetSubcategories(Guid categoryId);
        void AddSubcategory(Guid categoryId, string title);
        void UpdateSubcategory(Subcategory subcategory);
        Category? GetCategoryById(Guid categoryId);
        Subcategory? GetSubcategoryById(Guid subcategoryId);
        List<Field>? GetFields(Guid subcategoryId);
        void UpdateField(Guid fieldId, string title);
        void AddField(Guid subcategoryId, string title);
        Field GetFieldById(Guid id);
        void DeleteFieldList(List<Field> fields);
    }
}