using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sales.API.Models;
using Sales.API.Services;
using SharedModels;

namespace Sales.API.Controllers
{
    [ApiController]
    [Route("sales")]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SaleController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpGet]
        public ActionResult<List<Sale>> GetSales(Guid userId, Guid productId)
        {
            var sales = new List<Sale>();

            if (userId != Guid.Empty)
            {
                sales = _saleService.GetSalesByUserId(userId);
            }
            else if (productId != Guid.Empty)
            {
                sales = _saleService.GetSalesByProductId(productId);
            }

            if (sales.Count == 0)
            {
                return NotFound();
            }

            return Ok(sales);
        }

        [HttpPut("{id}/price")]
        [Authorize(Roles = "Admin, Seller , StoreKeeper , Owner")]
        public ActionResult UpdateSalePrice(Guid id, int newPrice)

        {
            Enum.TryParse(User.FindFirstValue(ClaimTypes.Role), out UserTypes role);
            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid UserId);
            var sale = _saleService.GetSaleById(id);
            if (sale == null)
                return NotFound();
            if ( role == UserTypes.Seller)
            {
                if (sale.UserId != UserId)
                    return Forbid();
            }
            _saleService.UpdateSalePrice(id, newPrice);
            return Ok();
        }

        [HttpPut("{id}/amount")]
        [Authorize(Roles = "Admin, Seller , StoreKeeper , Owner")]
        public ActionResult UpdateSaleAmount(Guid id, int amount)
        {   
            Enum.TryParse(User.FindFirstValue(ClaimTypes.Role), out UserTypes role);
            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid UserId);
            var sale = _saleService.GetSaleById(id);
            if (sale == null)
                return NotFound();
            if (role == UserTypes.Seller)
            {
                if (sale.UserId != UserId)
                    return Forbid();
            }
            _saleService.UpdateSaleAmount(id, amount);
            return Ok();
        }


        [HttpPost]
        [Authorize(Roles = "Admin, Seller , StoreKeeper , Owner")]
        public ActionResult AddSale(Guid userId, Guid productId, int amount, int initialPrice)
        {
            Enum.TryParse(User.FindFirstValue(ClaimTypes.Role), out UserTypes role);
            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid UserId);
            if (role == UserTypes.Seller)
            {
                if (userId != UserId)
                    return Forbid();
            }
            _saleService.AddSale(userId, productId, amount, initialPrice);
            return Ok();
        }
    }
}