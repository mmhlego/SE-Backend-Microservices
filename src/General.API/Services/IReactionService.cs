using General.API.Models;


namespace General.API.Services
{
    public interface IReactionService
    {
        List<Reaction> GetLikesByProductId(Guid productId);
        List<Reaction> GetLikesBySellerId(Guid sellerId);
        List<Reaction> GetLikesByCommentId(Guid commentId);
        bool AddReaction(Guid customerId, Guid targetId, ReactionTypes type, bool like);
        bool DeleteReaction(Reaction reaction);
    }
}