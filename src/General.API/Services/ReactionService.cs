using System;
using General.API.Data;
using General.API.Models;
using Users.API.Data;

namespace General.API.Services {
    public class ReactionService : IReactionService
    {
        private readonly GeneralContext context;
      //  private readonly UsersContext UsersContext;
      //  private readonly ProductsContext productsContext;
        public ReactionService(GeneralContext context, UsersContext UsersContext)
        {
            this.context = context;
           // this.UsersContext = UsersContext;
        }

        public List<Reaction> GetLikesByProductId(Guid productId)
        {
            try
            {
                return context.Reactions
                    .Where(r => r.TargetId == productId && r.Type == ReactionTypes.ProductLike)
                    .ToList();
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("The productId is invalid", ex);
            }
        }

        public List<Reaction> GetLikesBySellerId(Guid sellerId)
        {
            try
            {
                return context.Reactions
                    .Where(r => r.TargetId == sellerId && r.Type == ReactionTypes.SellerLike)
                    .ToList();
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("The sellerId is invalid", ex);
            }
        }

        public List<Reaction> GetLikesByCommentId(Guid commentId)
        {
            try
            {
                return context.Reactions
                    .Where(r => r.TargetId == commentId && r.Type == ReactionTypes.CommentLike)
                    .ToList();
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("The commentId is invalid", ex);
            }
        }

        public bool AddReaction(Guid customerId, Guid targetId, ReactionTypes type, bool like)
        {
            try
            {
              
                // Check if reaction from customerId exists
                if (context.Reactions.Any(r => r.CustomerId == customerId && r.TargetId == targetId && r.Type == type))
                {
                    throw new ArgumentNullException("The reaction from the customerId already exists");
                }

                context.Reactions.Add(new Reaction
                {
                    CustomerId = customerId,
                    TargetId = targetId,
                    Type = type,
                    Like = like
                });

                context.SaveChanges();

                return true;
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException("An error has occurred while trying to add the reaction", ex);
            }
        }

        public bool DeleteReaction(Reaction reaction)
        {
            try
            {
                // Check if reaction exists
                var r = context.Reactions.SingleOrDefault(r => r.Id == reaction.Id);
                if (r == null)
                {
                    throw new ArgumentNullException("The reaction doesn't exist");
                }

                context.Reactions.Remove(r);
                context.SaveChanges();

                return true;
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException("An error has occurred while trying to delete the reaction", ex);
            }
        }
    }
}