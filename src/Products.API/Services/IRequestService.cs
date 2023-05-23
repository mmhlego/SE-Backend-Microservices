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
        Guid AddRequest(Product requestedProduct, ProductRequestTypes type , Guid SellerId);
        bool AcceptAddRequest(Guid requestId);
        bool MergeUpdateRequest(Guid requestId);
        bool DeleteRequest(Guid requestId);
    }

}