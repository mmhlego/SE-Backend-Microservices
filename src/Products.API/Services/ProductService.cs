using Products.API.Data;
using Products.API.Models;
using SharedModels;

namespace Products.API.Services {
    public class ProductService : IProductService {
        private readonly ProductsContext _context;

        public ProductService(ProductsContext context) {
            _context = context;
        }

        public List<Product> GetProducts() {
            return _context.Products.ToList();
        }

        public Product? GetProductById(Guid id) {
            // var product = _context.Products.FirstOrDefault(p => p.RowId == id);
            // if (product == null) {
            //     throw new ArgumentException("Product not found");
            // }
            // return product;
            return _context.Products.FirstOrDefault(p => p.RowId == id);
        }

        public void AddProduct(string name, string description, Guid subCategory, ProductStates state) {
            // var subCat = _context.Subcategories.FirstOrDefault(sc => sc.Id == subCategory);
            // if (subCat == null) {
            // throw an exception if the subcategory doesn't exist
            // throw new ArgumentException("Subcategory not found");

            if (!_context.Subcategories.Any(sc => sc.Id == subCategory)) {
                return;
            }

            var product = new Product {
                ProductId = Guid.NewGuid(),
                Subcategory = subCategory,
                Name = name,
                Description = description,
                State = state
            };

            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void UpdateProduct(Product updatedProduct) {
            // var product = _context.Products.FirstOrDefault(p => p.RowId == updatedProduct.RowId);
            // if (product == null) {
            // throw new ArgumentException("Product not found");
            if (!_context.Products.Any(p => p.RowId == updatedProduct.RowId)) {
                return;
            }

            // update only specified properties
            // if (!string.IsNullOrEmpty(updatedProduct.Name)) product.Name = updatedProduct.Name;
            // if (!string.IsNullOrEmpty(updatedProduct.Description)) product.Description = updatedProduct.Description;
            // product.Subcategory = updatedProduct.Subcategory;
            // product.State = updatedProduct.State;

            _context.Products.Update(updatedProduct);
            _context.SaveChanges();
        }

        public List<ProductImage> GetProductImages(Guid productId) {
            // var product = _context.Products.FirstOrDefault(p => p.RowId == productId);
            // if (product == null) {
            //     throw new ArgumentException("Product not found");
            // }

            return _context.ProductImages.Where(pi => pi.ProductId == productId).ToList();
        }

        public void UpdateProductImages(Guid productId, List<string> ImageUrls) {
            // var product = _context.Products.FirstOrDefault(p => p.RowId == productId);
            // if (product == null) {
            // throw new ArgumentException("Product not found");
            if (!_context.Products.Any(p => p.RowId == productId)) {
                return;
            }

            // delete existing images
            var productImages = _context.ProductImages.Where(pi => pi.ProductId == productId);
            foreach (var pi in productImages) {
                _context.ProductImages.Remove(pi);
            }

            // add new images
            foreach (var url in ImageUrls) {
                var productImage = new ProductImage {
                    ProductId = productId,
                    ImageUrl = url
                };
                _context.ProductImages.Add(productImage);
            }

            _context.SaveChanges();
        }
    }
}