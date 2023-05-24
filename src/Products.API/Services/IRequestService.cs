using Products.API.Models;
using SharedModels;

namespace Products.API.Services {
    public interface IRequestService {
        List<ProductRequest> GetRequests(ProductRequestTypes type);
        Guid AddRequest(Guid sellerId, Product requestedProduct, ProductRequestTypes type);
        bool AcceptAddRequest(Guid requestId);
        bool MergeUpdateRequest(Guid requestId);
        bool DeleteRequest(Guid requestId);
    }
}