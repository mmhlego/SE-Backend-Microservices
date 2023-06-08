using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sales.API.Models;
using Sales.API.Models.Requests;

namespace Sales.API.Services {
    public interface IPaymentService
    {
        decimal EvaluateDiscount(string token);
        List<DiscountToken> GetDiscountTokens();
        DiscountToken CreateDiscountToken(PostDiscountToken postDiscountToken);
        bool DeleteDiscountToken(Guid id);
    }
}