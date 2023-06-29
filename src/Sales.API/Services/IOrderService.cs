using Sales.API.Models;

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
		// Order UpdateOrder(Guid id, UpdateOrderRequest request);
		bool DeleteOrder(Guid id);
	}
}
