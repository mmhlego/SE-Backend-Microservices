using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Products.API.Models;
using SharedModels;

namespace Products.API.Services {
    public interface IRequestService
    {
        List<ProductRequest> GetRequests(ProductRequestTypes type);
        void AddRequest(Product requestedProduct, ProductRequestTypes type);
        void AcceptAddRequest(Guid requestId);
        void MergeUpdateRequest(Guid requestId);
        void DeleteRequest(Guid requestId);
    }
}