using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Chat.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sales.API.Models;
using Sales.API.Models.Requests;
using Sales.API.Services;

namespace Sales.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class DiscountController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public DiscountController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("evaluate")]
        public ActionResult<decimal> EvaluateDiscount([FromBody] string token)
        {
            decimal discount = _paymentService.EvaluateDiscount(token);

            if (discount == 0)
                return Ok(StatusResponse.Failed("کد تخفیف پیدا نشد."));
            if (discount == -1)
                return Ok(StatusResponse.Failed("تاریخ انقضا سپری شده."));
            if (discount < 0)
                return Ok(StatusResponse.Failed("خطایی رخ داده."));
            return Ok(discount);
        }

        [HttpGet("discountTokens")]
        [Authorize(Roles = "Admin , Owner")]
        public ActionResult<List<DiscountToken>> GetDiscountTokens()
        {
            List<DiscountToken> discountTokens = _paymentService.GetDiscountTokens();

            return Ok(discountTokens);
        }

        [HttpPost("discountTokens")]
        //[Authorize(Roles = "Admin , Owner")]
        public ActionResult<DiscountToken> CreateDiscountToken([FromBody] PostDiscountToken discountToken)
        {
            
            DiscountToken createdToken = _paymentService.CreateDiscountToken(discountToken);

            return Ok(createdToken);
        }

        [HttpDelete("discountTokens/{id}")]
        [Authorize(Roles = "Admin , Owner")]
        public ActionResult<StatusResponse> DeleteDiscountToken(Guid id)
        {
            bool success = _paymentService.DeleteDiscountToken(id);

            if (success)
            {
                return Ok(StatusResponse.Success);
            }

            return Ok(StatusResponse.Failed("کد تخفیف پیدا نشد."));
        }


    }
}