using Chat.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sales.API.Models;
using Sales.API.Models.Requests;
using Sales.API.Services;
using SharedModels;
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

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
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

            if (order == null)
            {
                return Ok(StatusResponse.Failed("سفارشی وجود ندارد."));
            }

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