using Chat.API.Models;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sales.API.Models;
using Sales.API.Models.Requests;
using Sales.API.Services;
using SharedModels;
using SharedModels.Events;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Sales.API.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderController(IOrderService orderService, IPaymentService paymentService, IPublishEndpoint publishEndpoint)

        {
            _orderService = orderService;
            _paymentService = paymentService;
            _publishEndpoint = publishEndpoint;
        }


        [HttpGet("myOrders")]
        [Authorize(Roles = "Customer")]
        public ActionResult<List<Order>> GetMyOrders([FromQuery] int page, [FromQuery] int perPage)
        {
            Guid userId = (Guid)GetUserIdFromToken()!;

            var orders = _orderService.GetUserOrders(userId);
            orders = Pagination<Order>.Paginate(orders, perPage, page).Data;
            return Ok(orders);
        }


        [HttpGet("myOrders/current")]
        [Authorize(Roles = "Customer")]
        public ActionResult<Order> GetCurrentUserOrder()
        {
            Guid userId = (Guid)GetUserIdFromToken()!;
            var order = _orderService.GetCurrentOrder(userId);

            return Ok(order);
        }

        [HttpPost("myOrders/current")]
        [Authorize(Roles = "Customer")]
        public ActionResult<Order> AddOrderItemToCurrentOrder([FromBody] PostOrderItem item)
        {
            Guid userId = (Guid)GetUserIdFromToken()!;
            var orderItem = _orderService.AddOrderItemToCurrentOrder(userId, item.SalePriceId, item.Amount);
            return Ok(orderItem);
        }

        [HttpPost("myOrders/current/calculate")]
        [Authorize(Roles = "Customer")]
        public ActionResult<CalculateCart> CalculateCart(string Token)
        {
            Guid UserId = (Guid)GetUserIdFromToken()!;
            Order currentOrder = _orderService.GetCurrentOrder(UserId)!;
            decimal discount = _paymentService.EvaluateDiscount(Token);
            decimal sum = currentOrder.TotalPrice;
            decimal totalPrice = sum - discount;
            CalculateCart output = new CalculateCart
            {
                Sum = sum,
                Discount = discount,
                Price = totalPrice
            };
            return Ok(output);
        }

        [HttpPost("myOrders/current/checkout")]
        [Authorize(Roles = "Customer")]
        public ActionResult<Purchase> CheckoutCart(string Description, Guid AddressId, string? Token)
        {
            Guid UserId = (Guid)GetUserIdFromToken()!;
            Order currentOrder = _orderService.GetCurrentOrder(UserId)!;
            decimal totalPrice = currentOrder.TotalPrice - _paymentService.EvaluateDiscount(Token);
            Purchase purchase = new Purchase
            {
                Id = Guid.NewGuid(),
                CustomerAddressId = AddressId,
                Description = Description,
                DiscountToken = Token,
                Order = currentOrder,
                OrderId = currentOrder.Id,
                PurchaseDate = DateTime.Now,
                TotalPrice = totalPrice
            };
            _orderService.DeleteOrder(currentOrder.Id);
            currentOrder.OrderItems.ForEach(item =>
            {
                item.SalePrice.Sale.Amount -= item.Amount;
                if (item.SalePrice.Sale.Amount == 0) {
                    _publishEndpoint.Publish(new MessageEvent
                    {
                        TargetId = UserId,
                        Content = "محصول با آیدی : " + item.SalePrice.Sale.ProductId + "تمام شد.",
                        Type = MessageTypes.ProductFinished
                    }).Wait();
                    _publishEndpoint.Publish(new EmailEvent
                    {
                        Code = "محصول با آیدی : " + item.SalePrice.Sale.ProductId + "تمام شد.",
                        TargetEmail = "mmhlego@gmail.com",
                        Type = SmsTypes.CartChange
                    }).Wait();
                }
            });
            return Ok(purchase);
        }

        [HttpDelete("myOrders/current")]
        [Authorize(Roles = "Customer")]
        public ActionResult<StatusResponse> RemoveProductFromCurrentOrder(Guid orderItemId)
        {
            Guid userId = (Guid)GetUserIdFromToken()!;

            var isRemoved = _orderService.RemoveOrderItemFromCurrentOrder(userId, orderItemId);

            if (!isRemoved)
            {
                return Ok(StatusResponse.Failed("کالا در سبد خرید شما موجود نمیباشد."));
            }

            return Ok(StatusResponse.Success);
        }


        [HttpGet("orders")]
        [Authorize(Roles = "Storekeeper, Admin")]
        public ActionResult<List<Order>> GetOrdersByStatus([FromQuery] OrderStates? states, [FromQuery] int page, [FromQuery] int perPage, [FromQuery] Guid? userId)
        {
            var orders = _orderService.GetOrders(states, userId);
            if (orders == null)
                return Ok(StatusResponse.Failed("هیچ سبد کالایی پیدا نشد."));
            var paginatedOrders = Pagination<Order>.Paginate(orders, perPage, page);
            return Ok(paginatedOrders.Data);
        }

        [HttpGet("orders/{id}")]
        [Authorize(Roles = "Storekeeper, Admin")]
        public ActionResult<Order> GetOrderById(Guid id)
        {
            var order = _orderService.GetOrderById(id);

            if (order == null)
            {
                return Ok(StatusResponse.Failed("هیچ سبد کالایی پیدا نشد."));
            }

            return Ok(order);
        }

        [HttpDelete("orders/{id}")]
        [Authorize(Roles = "Storekeeper, Admin")]
        public ActionResult<StatusResponse> DeleteOrder(Guid id)
        {
            var isDeleted = _orderService.DeleteOrder(id);

            if (!isDeleted)
            {
                return Ok(StatusResponse.Failed("سبد کالای مورد نظر پیدا نشد."));
            }

            return Ok(StatusResponse.Success);
        }


        [HttpPut("orders/{id}")]
        [Authorize(Roles = "Storekeeper, Admin")]
        public ActionResult<Order> ChangeOrderStates(Guid id, [FromBody] OrderStates states)
        {
            var order = _orderService.UpdateOrderState(id, states);
            if (order == null)
                return Ok(StatusResponse.Failed("سبد کالای مورد نظر پیدا نشد."));

            _publishEndpoint.Publish(new MessageEvent
            {
                TargetId = order.UserId,
                Content = "وضعیت سبد خرید شما تغییر یافت",
                Type = MessageTypes.Personal
            }).Wait();
            _publishEndpoint.Publish(new EmailEvent
            {
                Code = "وضعیت سبد خرید شما تغییر یافت",
                TargetEmail = "mmhlego@gmail.com",
                Type = SmsTypes.CartChange
            }).Wait();
            return Ok(order);
        }
        private Guid? GetUserIdFromToken()
        {
            //var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            _ = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid UserId);

            //Console.WriteLine(Guid.Parse(userId));
            return UserId;


        }
    }
}