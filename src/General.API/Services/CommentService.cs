using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using General.API.Data;
using General.API.Models;
using Microsoft.EntityFrameworkCore;
using SharedModels;
using Users.API.Data;

namespace General.API.Services {
    public class CommentService : ICommentService
    {
        private readonly GeneralContext _context;
       // private readonly UsersContext _userContext;

        public CommentService(GeneralContext context, UsersContext userContext)
        {
            _context = context;
         //   _userContext = userContext;
        }

        public List<ProductComment> GetComments()
        {
            return _context.ProductComments.ToList();
        }

        public ProductComment GetCommentById(Guid id)
        {
            var comment = _context.ProductComments.Where(c => c.Id == id).FirstOrDefault();

            if (comment == null)
            {
                throw new Exception("No comment with the specified id was found");
            }

            return comment;
        }

        public List<ProductComment> GetCommentsByCustomerId(Guid customerId)
        {


            var comment = _context.ProductComments.Where(u => u.CustomerId == customerId).FirstOrDefault();

           
            if(comment == null)
            {
                throw new Exception("No comment with the specified id was found");
            }

            return _context.ProductComments.Where(c => c.CustomerId == customerId).ToList();
        }

        public List<ProductComment> GetCommentsByProductId(Guid productId)
        {
            return _context.ProductComments.Where(c => c.ProductId == productId).ToList();
        }

        public void AddComment(Guid customerId, Guid productId, string content)
        {
            //var user = _userContext.Customers.Where(u => u.Id == customerId).FirstOrDefault();

            //if (user == null)
            //{
            //    throw new Exception("No user with the specified id was found");
            //}

            var productComment = new ProductComment
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                ProductId = productId,
                Content = content,
                IssueDate = DateTime.Now
            };

            _context.ProductComments.Add(productComment);
            _context.SaveChanges();
        }

        public void DeleteComment(Guid id)
        {
            var comment = _context.ProductComments.Where(c => c.Id == id).FirstOrDefault();

            if (comment == null)
            {
                throw new Exception("No comment with the specified id was found");
            }

            _context.ProductComments.Remove(comment);
            _context.SaveChanges();
        }
    }


}