using Products.API.Data;
using Products.API.Models;
namespace Products.API.Services {
    public class CategoryService : ICategoryService {
        private readonly ProductsContext _context;

        public CategoryService(ProductsContext context) {
            _context = context;
        }

        public List<Category> GetCategories() {
            // if (_context.Categories == null)
            //     throw new ArgumentNullException(nameof(_context.Categories));

            return _context.Categories.ToList();
        }

        public void AddCategory(string title, string iconName) {
            // if (string.IsNullOrEmpty(title))
            //     throw new ArgumentNullException("Category title is required.");

            // if (string.IsNullOrEmpty(iconName))
            //     throw new ArgumentNullException("Category icon is required.");

            _context.Categories.Add(new Category {
                Title = title,
                IconName = iconName
            });
            _context.SaveChanges();
        }

        public void UpdateCategory(Category category) {
            // if (category == null)
            //     throw new ArgumentNullException("Category is required to update.");

            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        public List<Subcategory> GetSubcategories(Guid categoryId) {
            // if (categoryId == default(Guid))
            //     throw new ArgumentNullException("Category Id is required.");

            // var subcategory = _context.Subcategories.Where(c => c.CategoryId == categoryId).ToList();

            // if (subcategory == null)
            //     throw new ArgumentException("Category not found.");

            // return subcategory;
            return _context.Subcategories.Where(c => c.CategoryId == categoryId).ToList();
        }

        public void AddSubcategory(Guid categoryId, string title) {
            // if (categoryId == default(Guid))
            //     throw new ArgumentNullException("Category Id is required.");

            // if (string.IsNullOrEmpty(title))
            //     throw new ArgumentNullException("Subcategory title is required.");

            var category = _context.Categories.FirstOrDefault(c => c.Id == categoryId);

            if (category == null)
                return;
            // throw new ArgumentException("Category not found.");

            var subcategory = _context.Subcategories.FirstOrDefault(c => c.CategoryId == categoryId && c.Title == title);

            if (subcategory != null)
                return;
            // throw new Exception("Subcategory already exist.");

            _context.Subcategories.Add(new Subcategory {
                Title = title,
                CategoryId = categoryId
            });
            _context.SaveChanges();
        }

        public void UpdateSubcategory(Subcategory subcategory) {
            // if (subcategory == null)
            //     throw new ArgumentNullException("Subcategory is required to update.");

            // var sub = _context.Subcategories.FirstOrDefault(c => c.Id == subcategory.Id);
            // if (sub == null)
            //     throw new ArgumentNullException("SubcategoryId is wrong.");
            if (!_context.Subcategories.Any(c => c.Id == subcategory.Id))
                return;

            // var category = _context.Categories.SingleOrDefault(c => c.Id == subcategory.CategoryId);
            // if (category == null)
            //     throw new ArgumentNullException("CategoryId is wrong.");

            // var category = _context.Categories.SingleOrDefault(c => c.Id == subcategory.CategoryId);
            // if (category == null)
            //     throw new ArgumentNullException("Category Is Deleted.");

            // sub.CategoryId = subcategory.Id;
            // sub.Title = subcategory.Title;
            // _context.Update(sub);
            _context.Update(subcategory);
            _context.SaveChanges();
        }
    }
}