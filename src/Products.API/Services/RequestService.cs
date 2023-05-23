using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Products.API.Data;
using Products.API.Models;
using SharedModels;


namespace Products.API.Services
{

    public class RequestService : IRequestService
    {
        private readonly ProductsContext _context;

        public RequestService(ProductsContext context)
        {
            _context = context;
        }

        public List<ProductRequest> GetRequests(ProductRequestTypes type)
        {
            return _context.ProductRequests
                .Include(pr => pr.Product)
                .Where(pr => pr.Type == type)
                .ToList();
        }

        public Guid AddRequest(Product requestedProduct, ProductRequestTypes type, Guid SellerId)
        {
            var request = new ProductRequest
            {
                Type = type,
                Product = requestedProduct,
                SellerId = SellerId
            };
            _context.ProductRequests.Add(request);
            _context.SaveChanges();
            return request.Id;
        }

        public bool AcceptAddRequest(Guid requestId)
        {
            var request = _context.ProductRequests
                .Include(pr => pr.Product)
                .FirstOrDefault(pr => pr.Id == requestId);
            if (request == null || request.Type != ProductRequestTypes.AddRequest)
            {
                return false;
            }
            var product = new Product
            {
                ProductId = request.Product.ProductId,
                Subcategory = request.Product.Subcategory,
                Name = request.Product.Name,
                Description = request.Product.Description,
                State = ProductStates.Available
            };
            _context.Products.Add(product);
            request.ProductRowId = product.RowId;
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            request.Product = null; // Remove the product reference from the request to avoid saving it twice
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            _context.SaveChanges();
            return true;
        }

        public bool MergeUpdateRequest(Guid requestId)
        {
            var request = _context.ProductRequests
                .Include(pr => pr.Product)
                .FirstOrDefault(pr => pr.Id == requestId);
            if (request == null || request.Type != ProductRequestTypes.UpdateRequest || request.ProductRowId == default(Guid)
                )
            {
                return false;
            }
            var product = _context.Products.FirstOrDefault(p => p.RowId == request.ProductRowId);
            if (product == null)
            {
                return false;
            }
            product.Name = request.Product.Name;
            product.Description = request.Product.Description;
            _context.Products.Update(product);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteRequest(Guid requestId)
        {
            var request = _context.ProductRequests.FirstOrDefault(pr => pr.Id == requestId);
            if (request == null)
            {
                return false;
            }
            _context.ProductRequests.Remove(request);
            _context.SaveChanges();
            return true;
        }
    }

}