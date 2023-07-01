using Microsoft.AspNetCore.Mvc;
using Products.API.Data;
using Products.API.Models;
using SharedModels;

namespace Products.API.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductsContext _context;

        public ProductService(ProductsContext context)
        {
            _context = context;
        }

        public List<Product> GetProducts()
        {
            return _context.Products.ToList();
        }

        public Product? GetProductById(Guid id)
        {

            return _context.Products.FirstOrDefault(p => p.ProductId == id);
        }

        public Product? AddProduct(string name, string description, Guid subCategoryId, ProductStates state)
        {

            if (!_context.Subcategories.Any(sc => sc.Id == subCategoryId))
            {
                return null;
            }

            var product = new Product
            {
                ProductId = Guid.NewGuid(),
                SubcategoryId = subCategoryId,
                Name = name,
                Description = description,
                State = state
            };

            _context.Products.Add(product);
            _context.SaveChanges();
            return product;
        }

        public void UpdateProduct(Product updatedProduct)
        {
            // var product = _context.Products.FirstOrDefault(p => p.RowId == updatedProduct.RowId);
            // if (product == null) {
            // throw new ArgumentException("Product not found");
            if (!_context.Products.Any(p => p.RowId == updatedProduct.RowId))
            {
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

        public List<ProductImage> GetProductImages(Guid productId)
        {
            // var product = _context.Products.FirstOrDefault(p => p.RowId == productId);
            // if (product == null) {
            //     throw new ArgumentException("Product not found");
            // }

            return _context.ProductImages.Where(pi => pi.ProductId == productId).ToList();
        }

        public void UpdateProductImages(Guid productId, List<string> ImageUrls)
        {
            // var product = _context.Products.FirstOrDefault(p => p.RowId == productId);
            // if (product == null) {
            // throw new ArgumentException("Product not found");
            if (!_context.Products.Any(p => p.RowId == productId))
            {
                return;
            }

            // delete existing images
            var productImages = _context.ProductImages.Where(pi => pi.ProductId == productId);
            foreach (var pi in productImages)
            {
                _context.ProductImages.Remove(pi);
            }

            // add new images
            foreach (var url in ImageUrls)
            {
                var productImage = new ProductImage
                {
                    ProductId = productId,
                    ImageUrl = url
                };
                _context.ProductImages.Add(productImage);
            }

            _context.SaveChanges();
        }
        public List<Product> SearchProductsByName(string searchQuery, List<Product> products)
        {
            List<Product> matchedProducts = products.Where(p => IsNameMatch(p.Name, searchQuery)).ToList();

            return matchedProducts;
        }
        private bool IsNameMatch(string productName, string searchQuery)
        {
            string productNameLower = productName.ToLower();
            string searchQueryLower = searchQuery.ToLower();

            if (productNameLower.Contains(searchQueryLower))
                return true;

            int distance = CalculateLevenshteinDistance(searchQueryLower, productNameLower);
            int threshold = Math.Max(productNameLower.Length / 2, 1);

            return distance <= threshold;
        }
        private int CalculateLevenshteinDistance(string source, string target)
        {
            int sourceLength = source.Length;
            int targetLength = target.Length;
            int[,] distanceMatrix = new int[sourceLength + 1, targetLength + 1];

            for (int i = 0; i <= sourceLength; i++)
                distanceMatrix[i, 0] = i;

            for (int j = 0; j <= targetLength; j++)
                distanceMatrix[0, j] = j;

            for (int i = 1; i <= sourceLength; i++)
            {
                for (int j = 1; j <= targetLength; j++)
                {
                    int cost = source[i - 1] == target[j - 1] ? 0 : 1;
                    distanceMatrix[i, j] = Math.Min(
                        Math.Min(distanceMatrix[i - 1, j] + 1, distanceMatrix[i, j - 1] + 1),
                        distanceMatrix[i - 1, j - 1] + cost);
                }
            }

            return distanceMatrix[sourceLength, targetLength];
        }

        public List<Product> FilterProductsByPrice(decimal? priceFrom, decimal? priceTo)
        {

            var sales = _context.Sales.ToList();
            if (priceFrom != null)
                sales = sales.Where(c => c.Price >= priceFrom).ToList();
            if (priceTo != null)
                sales = sales.Where(c => c.Price < priceTo).ToList();
            List<Product> products = sales.Select(cc => _context.Products.FirstOrDefault(o => o.ProductId == cc.ProductId)).ToList();


            return (products);

        }
    }
}