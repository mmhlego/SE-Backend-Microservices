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

        public List<ProductRequest> GetRequests(ProductRequestTypes type) {
            return _context.ProductRequests.Where(pr => pr.Type == type).ToList();
        }

        public void AddRequest(Product requestedProduct, ProductRequestTypes type) {
            var request = new ProductRequest {
                Type = type,
                Product = requestedProduct
            };

            _context.ProductRequests.Add(request);
            _context.SaveChanges();
        }

        public void AcceptAddRequest(Guid requestId) {
            var request = _context.ProductRequests
                .Include(pr => pr.Product)
                .FirstOrDefault(pr => pr.Id == requestId && pr.Type == ProductRequestTypes.AddRequest);
            if (request == null) {
                throw new ArgumentException("Add request not found");
            }

            request.Product.State = ProductStates.Available;

            _context.ProductRequests.Remove(request);
            _context.SaveChanges();
        }

        public void MergeUpdateRequest(Guid requestId) {
            var request = _context.ProductRequests
                .Include(pr => pr.Product)
                .FirstOrDefault(pr => pr.Id == requestId && pr.Type == ProductRequestTypes.UpdateRequest);
            if (request == null) {
                throw new ArgumentException("Update request not found");
            }

            var updatedProduct = request.Product;

            // get all update requests for the same product and merge them
            var updateRequests = _context.ProductRequests
                .Where(pr => pr.Type == ProductRequestTypes.UpdateRequest && pr.Product.RowId == updatedProduct.RowId)
                .ToList();
            foreach (var updateRequest in updateRequests) {
                if (updateRequest.Product.State != ProductStates.UpdatePending) {
                    continue;
                }

                updatedProduct.Name = updateRequest.Product.Name != null ? updateRequest.Product.Name : updatedProduct.Name;
                updatedProduct.Description = updateRequest.Product.Description != null ? updateRequest.Product.Description : updatedProduct.Description;
                // updatedProduct.Subcategory = updateRequest.Product.Subcategory.GetValueOrDefault(updatedProduct.Subcategory);
                updatedProduct.Subcategory = updateRequest.Product.Subcategory != Guid.Empty ? updateRequest.Product.Subcategory : updatedProduct.Subcategory;
            }


            // update the original product
            var product = _context.Products.FirstOrDefault(p => p.RowId == updatedProduct.RowId);
            if (product == null) {
                throw new ArgumentException("Product not found");
            }

            product.Name = updatedProduct.Name;
            product.Description = updatedProduct.Description;
            product.Subcategory = updatedProduct.Subcategory;
            product.State = ProductStates.Available;

            // delete all update requests
            foreach (var updateRequest in updateRequests) {
                _context.ProductRequests.Remove(updateRequest);
            }

            _context.SaveChanges();
        }

        public void DeleteRequest(Guid requestId) {
            var request = _context.ProductRequests.FirstOrDefault(pr => pr.Id == requestId);
            if (request == null) {
                throw new ArgumentException("Request not found");
            }

            _context.ProductRequests.Remove(request);
            _context.SaveChanges();
        }
    }
}