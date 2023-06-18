using General.API.Data;
using General.API.Models;

namespace General.API.Services {
    public class ReactionService : IReactionService {
        private readonly GeneralContext _context;

        public ReactionService(GeneralContext context) {
            _context = context;
        }

        //public List<Reaction> GetLikesByProductId(Guid productId) {
        //    // try {
        //    return _context.Reactions
        //        .Where(r => r.TargetId == productId && r.Type == ReactionTypes.ProductLike)
        //        .ToList();
        //    // } catch (InvalidOperationException ex) {
        //    //     throw new InvalidOperationException("The productId is invalid", ex);
        //    // }
        //}

        //public List<Reaction> GetLikesBySellerId(Guid sellerId) {
        //    // try {
        //    return _context.Reactions
        //        .Where(r => r.TargetId == sellerId && r.Type == ReactionTypes.SellerLike)
        //        .ToList();
        //    // } catch (InvalidOperationException ex) {
        //    //     throw new InvalidOperationException("The sellerId is invalid", ex);
        //    // }
        //}

        //public List<Reaction> GetLikesByCommentId(Guid commentId) {
        //    // try {
        //    return _context.Reactions
        //        .Where(r => r.TargetId == commentId && r.Type == ReactionTypes.CommentLike)
        //        .ToList();
        //    // } catch (InvalidOperationException ex) {
        //    //     throw new InvalidOperationException("The commentId is invalid", ex);
        //    // }
        //}
        public int GetLikes(Guid targetId, ReactionTypes type)
        {
            return _context.Reactions.Count(r => r.TargetId == targetId && r.Type == type && r.Like);
        }

        public int GetDislikes(Guid targetId, ReactionTypes type)
        {
            return _context.Reactions.Count(r => r.TargetId == targetId && r.Type == type && !r.Like);
        }

        public bool AddReaction(Guid userId, Guid targetId, ReactionTypes type, bool like) {
           
            if (_context.Reactions.Any(r => r.UserId == userId && r.TargetId == targetId && r.Type == type)) {
                return false;
            }

            _context.Reactions.Add(new Reaction {
                UserId = userId,
                TargetId = targetId,
                Type = type,
                Like = like
            });

            _context.SaveChanges();
            return true;

       
        }

        public bool DeleteReaction(Guid customerId, Guid targetId, ReactionTypes type)
        {
            var reaction = _context.Reactions.FirstOrDefault(r => r.UserId == customerId && r.TargetId == targetId && r.Type == type);
            if (reaction != null)
            {
                _context.Reactions.Remove(reaction);
                _context.SaveChanges();
                return true;
            }

            return false;
        }
    }
}