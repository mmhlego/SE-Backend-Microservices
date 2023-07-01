using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Chat.API.Models;
using MassTransit;
using MassTransit.Internals.GraphValidation;
using MassTransit.Transports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sales.API.Models;
using Sales.API.Services;
using SharedModels;
using SharedModels.Events;

namespace Sales.API.Controllers
{
	[ApiController]
	[Route("api/[controller]/")]
	public class SaleController : ControllerBase
	{
		private readonly ISaleService _saleService;
        private readonly IPublishEndpoint _publishEndpoint;
        public SaleController(ISaleService saleService,IPublishEndpoint publishEndpoint)
		{
			_saleService = saleService;
            _publishEndpoint = publishEndpoint;
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
				return Ok(StatusResponse.Failed("پیدا نشد."));
			}

			return Ok(sales);
		}

		[HttpGet("sales/{id}/history")]
		public ActionResult<List<SalePrice>> GetPriceHistory([FromQuery] Guid id)
		{
			var sale = _saleService.GetSaleById(id);
			if (sale == null)
				return Ok(StatusResponse.Failed("فروش موردنظر یافت نشد")); ;

			var salePrices = _saleService.GetPriceHistoryBySaleId(id);

			return Ok(salePrices);
		}

		[HttpPut("sales/{id}/price")]
		[Authorize(Roles = "Seller , StoreKeeper")]
		public ActionResult<StatusResponse> UpdateSalePrice(Guid id, int newPrice)
		{
			_ = Enum.TryParse(User.FindFirstValue(ClaimTypes.Role), out UserTypes role);
			_ = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid UserId);
			var sale = _saleService.GetSaleById(id);

			if (sale == null)
				return Ok(StatusResponse.Failed("پیدا نشد."));
			if (role == UserTypes.Seller)
			{
				if (sale.UserId != UserId)
					return Ok(StatusResponse.Failed("اجازه دسترسی ندارید."));
			}
			_publishEndpoint.Publish(new MessageEvent
			{
				TargetId = UserId,
				Content = "محصول با آیدی : " + sale.Id + "اضافه شد.",
				Type = MessageTypes.ProductAvailable
			}).Wait();
            _publishEndpoint.Publish(new EmailEvent
            {
                Code = "محصول با آیدی : " + sale.Id + "اضافه شد.",
                TargetEmail = "mmhlego@gmail.com",
                Type = SmsTypes.Festival
            }).Wait();

            _saleService.UpdateSalePrice(id, newPrice);
			return Ok(StatusResponse.Success);
		}

		[HttpPut("sales/{id}/amount")]
		[Authorize(Roles = "Admin, Seller , StoreKeeper , Owner")]
		public ActionResult<StatusResponse> UpdateSaleAmount(Guid id, int amount)
		{
			_ = Enum.TryParse(User.FindFirstValue(ClaimTypes.Role), out UserTypes role);
			_ = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid UserId);
			var sale = _saleService.GetSaleById(id);
			if (sale == null)
				return Ok(StatusResponse.Failed("پیدا نشد."));
			if (role == UserTypes.Seller)
			{
				if (sale.UserId != UserId)
					return Ok(StatusResponse.Failed("اجازه دسترسی ندارید"));
			}
			_saleService.UpdateSaleAmount(id, amount);
			return Ok(StatusResponse.Success);
		}

		[HttpPost]
		[Route("sales")]
		[Authorize(Roles = "Admin, Seller , StoreKeeper , Owner")]
		public ActionResult<StatusResponse> AddSale(Guid userId, Guid productId, int amount, int initialPrice)
		{
			_ = Enum.TryParse(User.FindFirstValue(ClaimTypes.Role), out UserTypes role);
			_ = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid UserId);
			if (role == UserTypes.Seller)
			{
				if (userId != UserId)
					return Ok(StatusResponse.Failed("اجازه دسترسی ندارید."));
			}
			_saleService.AddSale(userId, productId, amount, initialPrice);
			return Ok(StatusResponse.Success);
		}


		
	}
}