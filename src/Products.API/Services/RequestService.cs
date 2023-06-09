using Microsoft.EntityFrameworkCore;
using Products.API.Data;
using Products.API.Models;
using SharedModels;


namespace Products.API.Services {

    public class RequestService : IRequestService {
        private readonly ProductsContext _context;

        public RequestService(ProductsContext context) {
            _context = context;
        }

        public List<ProductRequest> GetRequests(ProductRequestTypes? type) {
            if (type == null)
                return _context.ProductRequests.ToList();
            return _context.ProductRequests
                .Include(pr => pr.Product)
                .Where(pr => pr.Type == type)
                .ToList();
        }
        public ProductRequest? GetRequestById(Guid id)
        {
            return _context.ProductRequests.FirstOrDefault(p => p.Id == id);
        }
        public Guid AddRequest(Guid sellerId, Product requestedProduct, ProductRequestTypes type) {
            switch (type) {
                case ProductRequestTypes.AddRequest:
                    requestedProduct.State = ProductStates.AddPending;
                    break;
                case ProductRequestTypes.UpdateRequest:
                    requestedProduct.State = ProductStates.UpdatePending;
                    break;
            }

            var request = new ProductRequest {
                SellerId = sellerId,
                Type = type,
                Product = requestedProduct
            };
            _context.ProductRequests.Add(request);
            _context.SaveChanges();

            return request.Id;
        }

        public bool AcceptAddRequest(Guid requestId) {
            var request = _context.ProductRequests
                .Include(pr => pr.Product)
                .FirstOrDefault(pr => pr.Id == requestId && pr.Type == ProductRequestTypes.AddRequest);
            if (request == null) {
                return false;
                // throw new ArgumentException("Add request not found");
            }
            var newProduct = new Product {
                ProductId = request.Product.ProductId,
                Subcategory = request.Product.Subcategory,
                Name = request.Product.Name,
                Description = request.Product.Description,
                State = ProductStates.Available
            };

            _context.Products.Add(newProduct);
            _context.ProductRequests.Remove(request);
            _context.Products.Remove(request.Product);
            _context.SaveChanges();
            return true;
        }

        public bool MergeUpdateRequest(Guid requestId) {
            var request = _context.ProductRequests
                .Include(pr => pr.Product)
                .FirstOrDefault(pr => pr.Id == requestId && pr.Type == ProductRequestTypes.UpdateRequest);

            if (request == null)
                return false;

            var product = _context.Products.FirstOrDefault(p => p.RowId == request.ProductRowId);
            if (product == null) {
                return false;
            }
            product.Name = request.Product.Name;
            product.Description = request.Product.Description;
            _context.Products.Update(product);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteRequest(Guid requestId) {
            var request = _context.ProductRequests.FirstOrDefault(pr => pr.Id == requestId);
            if (request == null) {
                return false;
            }
            _context.ProductRequests.Remove(request);
            _context.SaveChanges();
            return true;
        }
    }
}