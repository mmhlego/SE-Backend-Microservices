using General.API.Data;
using General.API.Models;

namespace General.API.Services {
    public class ReactionService : IReactionService {
        private readonly GeneralContext _context;

        public ReactionService(GeneralContext context) {
            _context = context;
        }

        public List<Reaction> GetLikesByProductId(Guid productId) {
            // try {
            return _context.Reactions
                .Where(r => r.TargetId == productId && r.Type == ReactionTypes.ProductLike)
                .ToList();
            // } catch (InvalidOperationException ex) {
            //     throw new InvalidOperationException("The productId is invalid", ex);
            // }
        }

        public List<Reaction> GetLikesBySellerId(Guid sellerId) {
            // try {
            return _context.Reactions
                .Where(r => r.TargetId == sellerId && r.Type == ReactionTypes.SellerLike)
                .ToList();
            // } catch (InvalidOperationException ex) {
            //     throw new InvalidOperationException("The sellerId is invalid", ex);
            // }
        }

        public List<Reaction> GetLikesByCommentId(Guid commentId) {
            // try {
            return _context.Reactions
                .Where(r => r.TargetId == commentId && r.Type == ReactionTypes.CommentLike)
                .ToList();
            // } catch (InvalidOperationException ex) {
            //     throw new InvalidOperationException("The commentId is invalid", ex);
            // }
        }

        public bool AddReaction(Guid customerId, Guid targetId, ReactionTypes type, bool like) {
            // try {
            // Check if reaction from customerId exists
            if (_context.Reactions.Any(r => r.UserId == customerId && r.TargetId == targetId && r.Type == type)) {
                // throw new ArgumentNullException("The reaction from the customerId already exists");
                return false;
            }

            _context.Reactions.Add(new Reaction {
                UserId = customerId,
                TargetId = targetId,
                Type = type,
                Like = like
            });

            _context.SaveChanges();
            return true;

            // } catch (ArgumentNullException ex) {
            //     throw new ArgumentNullException("An error has occurred while trying to add the reaction", ex);
            // }
        }

        public bool DeleteReaction(Reaction reaction) {
            // try {
            // Check if reaction exists
            // var r = _context.Reactions.SingleOrDefault(r => r.Id == reaction.Id);
            // if (r == null) {
            //     throw new ArgumentNullException("The reaction doesn't exist");
            // }

            if (!_context.Reactions.Any(r => r.Id == reaction.Id))
                return false;

            _context.Reactions.Remove(reaction);
            _context.SaveChanges();

            return true;
            // } catch (ArgumentNullException ex) {
            //     throw new ArgumentNullException("An error has occurred while trying to delete the reaction", ex);
            // }
        }
    }
}