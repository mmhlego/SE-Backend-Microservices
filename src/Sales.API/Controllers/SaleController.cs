using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Chat.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sales.API.Models;
using Sales.API.Services;
using SharedModels;

namespace Sales.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SaleController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpGet]
        [Route("sales")]
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
                return NotFound(StatusResponse.Failed("پیدا نشد."));
            }

            return Ok(sales);
        }

        [HttpPut("sales/{id}/price")]
        [Authorize(Roles = "Admin, Seller , StoreKeeper , Owner")]
        public ActionResult<StatusResponse> UpdateSalePrice(Guid id, int newPrice)

        {
            Enum.TryParse(User.FindFirstValue(ClaimTypes.Role), out UserTypes role);
            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid UserId);
            var sale = _saleService.GetSaleById(id);
            if (sale == null)
                return NotFound(StatusResponse.Failed("پیدا نشد."));
            if (role == UserTypes.Seller)
            {
                if (sale.UserId != UserId)
                    return Unauthorized(StatusResponse.Failed("اجازه دسترسی ندارید."));
            }
            _saleService.UpdateSalePrice(id, newPrice);
            return Ok(StatusResponse.Success);
        }

        [HttpPut("sales/{id}/amount")]
        [Authorize(Roles = "Admin, Seller , StoreKeeper , Owner")]
        public ActionResult<StatusResponse> UpdateSaleAmount(Guid id, int amount)
        {
            Enum.TryParse(User.FindFirstValue(ClaimTypes.Role), out UserTypes role);
            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid UserId);
            var sale = _saleService.GetSaleById(id);
            if (sale == null)
                return NotFound(StatusResponse.Failed("پیدا نشد."));
            if (role == UserTypes.Seller)
            {
                if (sale.UserId != UserId)
                    return Unauthorized(StatusResponse.Failed("اجازه دسترسی ندارید"));
            }
            _saleService.UpdateSaleAmount(id, amount);
            return Ok(StatusResponse.Success);
        }


        [HttpPost]
        [Route("sales")]
        [Authorize(Roles = "Admin, Seller , StoreKeeper , Owner")]
        public ActionResult<StatusResponse> AddSale(Guid userId, Guid productId, int amount, int initialPrice)
        {
            Enum.TryParse(User.FindFirstValue(ClaimTypes.Role), out UserTypes role);
            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid UserId);
            if (role == UserTypes.Seller)
            {
                if (userId != UserId)
                    return Unauthorized(StatusResponse.Failed("اجازه دسترسی ندارید."));
            }
            _saleService.AddSale(userId, productId, amount, initialPrice);
            return Ok(StatusResponse.Success);
        }


        [HttpGet]
        [Route("getproductIds")]
        public ActionResult<List<Guid>> ProductsFtilter([FromQuery] decimal? priceFrom, [FromQuery] decimal? priceTo)
        { 
            return _saleService.FilterProductsByPrice(priceFrom, priceTo);
        }
    }
}