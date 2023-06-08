using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sales.API.Models;
using Sales.API.Models.Requests;
using Sales.API.Services;

namespace Sales.API.Controllers {
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly PaymentService _paymentService;

        public DiscountController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("evaluate")]
        public ActionResult<decimal> EvaluateDiscount([FromBody] string token)
        {   
            decimal discount = _paymentService.EvaluateDiscount(token);
            
            if (discount == 0)
                return NotFound();
            if(discount == -1)
                return StatusCode(410, "The discount token has expired");
            if (discount < 0)
                return BadRequest();
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
        [Authorize(Roles = "Admin , Owner")]
        public ActionResult<DiscountToken> CreateDiscountToken([FromBody] PostDiscountToken discountToken)
        {   
            DiscountToken createdToken = _paymentService.CreateDiscountToken(discountToken);

            return Ok(createdToken);
        }

        [HttpDelete("discountTokens/{id}")]
        [Authorize(Roles = "Admin , Owner")]
        public ActionResult DeleteDiscountToken(Guid id)
        {
            bool success = _paymentService.DeleteDiscountToken(id);

            if (success)
            {
                return Ok();
            }

            return NotFound();
        }

   
        }
}