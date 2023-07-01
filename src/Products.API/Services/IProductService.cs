using Products.API.Models;
using SharedModels;

namespace Products.API.Services
{
    public interface IProductService
    {
        List<Product> GetProducts();
        Product? GetProductById(Guid id);
        Product? AddProduct(string name, string description, Guid subCategory, ProductStates state);
        void UpdateProduct(Product updatedProduct);
        List<ProductImage> GetProductImages(Guid productId);
        void UpdateProductImages(Guid productId, List<string> ImageUrls);
        List<Product> SearchProductsByName(string searchQuery, List<Product> products);
        List<Product> FilterProductsByPrice(decimal? priceFrom, decimal? priceTo);
    }
}