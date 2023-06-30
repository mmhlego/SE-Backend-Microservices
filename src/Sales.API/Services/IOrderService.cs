using Sales.API.Models;
using SharedModels;
using System;
using System.Collections.Generic;

namespace Sales.API.Services
{
    public interface IOrderService
    {
        List<Order> GetUserOrders(Guid userId);
        Order? GetCurrentOrder(Guid userId);
        OrderItem AddOrderItemToCurrentOrder(Guid userId, Guid salePriceId, int amount);
        bool RemoveOrderItemFromCurrentOrder(Guid userId, Guid orderItemId);
        List<Order> GetOrders(OrderStates? states, Guid? userId);
        Order? GetOrderById(Guid id);
        bool DeleteOrder(Guid id);
    }
}
