using General.API.Models;

namespace General.API.Services {
    public interface IReactionService {
        //List<Reaction> GetLikesByProductId(Guid productId);
        //List<Reaction> GetLikesBySellerId(Guid sellerId);
        //List<Reaction> GetLikesByCommentId(Guid commentId);
        int GetLikes(Guid targetId, ReactionTypes type);
        int GetDislikes(Guid targetId, ReactionTypes type);
        bool AddReaction(Guid userId, Guid targetId, ReactionTypes type, bool like);
        bool DeleteReaction(Guid customerId, Guid targetId, ReactionTypes type);
    
}
}