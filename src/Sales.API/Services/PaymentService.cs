using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sales.API.Data;
using Sales.API.Models;
using Sales.API.Models.Requests;

namespace Sales.API.Services {
    public class PaymentService : IPaymentService
    {
        private readonly SalesContext _context; 

        public PaymentService(SalesContext Context)
        {
            _context = Context;
        }

        public decimal EvaluateDiscount(string token)
        {
            DiscountToken? discountToken = _context.DiscountTokens.FirstOrDefault(t => t.Token == token);

            if (discountToken == null)
                return 0;

            if (discountToken.ExpireDate < DateTime.Now)
                return -1;
     
            decimal discountAmount = 0;
            if (discountToken.Type == DiscountTokenTypes.Percent)
            {
                discountAmount = discountToken.Amount / 100.0m;
            }
            else if (discountToken.Type == DiscountTokenTypes.Price)
            {
                discountAmount = discountToken.Amount;
            }

            return discountAmount;
        }

        public List<DiscountToken> GetDiscountTokens()
        {
            return _context.DiscountTokens.ToList();
        }

        public DiscountToken CreateDiscountToken(PostDiscountToken postDiscountToken)
        {
            DiscountToken discountToken = new DiscountToken()
            {
                Amount = postDiscountToken.Amount,
                AvailableCount = postDiscountToken.AvailableCount,
                ExpireDate = postDiscountToken.ExpireDate,
                Token = TokenGenerator(),
                Type = postDiscountToken.Type
            };
            _context.DiscountTokens.Add(discountToken);
            _context.SaveChanges();
            return discountToken;
        }

        public bool DeleteDiscountToken(Guid id)
        {
            DiscountToken? discountToken = _context.DiscountTokens.FirstOrDefault(t => t.Id == id);

            if (discountToken == null)
            {
                return false;
            }

            _context.DiscountTokens.Remove(discountToken);
            _context.SaveChanges();
            return true;
        }
        public string TokenGenerator()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var result = new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
            return result;
        }
    }
}