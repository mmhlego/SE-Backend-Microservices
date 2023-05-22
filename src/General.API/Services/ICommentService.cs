using General.API.Models;

namespace General.API.Services {
    public interface ICommentService {
        List<ProductComment> GetComments();
        ProductComment GetCommentById(Guid id);
        List<ProductComment> GetCommentsByCustomerId(Guid customerId);
        List<ProductComment> GetCommentsByProductId(Guid productId);
        void AddComment(Guid customerId, Guid productId, string content);
        void DeleteComment(Guid id);
    }
}