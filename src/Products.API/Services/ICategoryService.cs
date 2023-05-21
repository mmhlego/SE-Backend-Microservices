using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Products.API.Models;

namespace Products.API.Services
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetCategories();
        void AddCategory(string title, string iconName);
        void UpdateCategory(Category category);
        IEnumerable<Subcategory> GetSubcategories(Guid categoryId);
        void AddSubcategory(Guid categoryId, string title);
        void UpdateSubcategory(Subcategory subcategory);
    }
}