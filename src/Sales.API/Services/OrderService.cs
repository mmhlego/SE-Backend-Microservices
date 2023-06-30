using Microsoft.EntityFrameworkCore;
using Sales.API.Data;
using Sales.API.Models;
using Sales.API.Services;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Sales.API.Services
{
	public class OrderService : IOrderService
	{
		private readonly SalesContext _context;

		public OrderService(SalesContext context)
		{
			_context = context;
		}

		public List<Order> GetUserOrders(Guid userId)
		{
			var orders = _context.Orders
				.Where(o => o.UserId == userId)
				.ToList();
			foreach (Order order in orders)
				order.OrderItems = _context.OrderItems.Where(o => o.OrderId == order.Id).ToList();

			return orders;
		}

		public Order? GetCurrentOrder(Guid userId)
		{
			var order = _context.Orders
				.FirstOrDefault(o => o.UserId == userId && o.State == OrderStates.Filling);
			if (order != null)
				order.OrderItems = _context.OrderItems.Where(o => o.OrderId == order.Id).ToList();
			else
			{
				order = new Order
				{
					Id = Guid.NewGuid(),
					UserId = userId,
				};
			}
			return order;
		}

		public OrderItem AddOrderItemToCurrentOrder(Guid userId, Guid salePriceId, int amount)
		{
			var currentOrder = _context.Orders
				.FirstOrDefault(o => o.UserId == userId && o.State == OrderStates.Filling);

			if (currentOrder == null)
			{

				currentOrder = new Order
				{
					UserId = userId,
					State = OrderStates.Filling,
					Id = Guid.NewGuid(),

				};

				_context.Orders.Add(currentOrder);
			}



			var orderItem = new OrderItem
			{
				OrderId = currentOrder.Id,
				Amount = amount,
				Id = Guid.NewGuid(),
				SalePriceId = salePriceId

			};

			_context.OrderItems.Add(orderItem);
			_context.SaveChanges();

			return orderItem;
		}

		public bool RemoveOrderItemFromCurrentOrder(Guid userId, Guid orderItemId)
		{
			var currentOrder = _context.Orders
				.FirstOrDefault(o => o.UserId == userId && o.State == OrderStates.Filling);

			if (currentOrder == null)
			{
				return false;
			}

			var orderItem = _context.OrderItems
				.FirstOrDefault(oi => oi.OrderId == currentOrder.Id && oi.Id == orderItemId);

			if (orderItem == null)
			{
				return false;
			}

			_context.OrderItems.Remove(orderItem);
			_context.SaveChanges();

			return true;
		}

		public List<Order> GetOrders(OrderStates? state, Guid? userId)
		{
			List<Order> orders = _context.Orders.ToList();
			if (state != null)
				orders = orders.Where(o => o.State == state).ToList();
			if (userId != null)
				orders = orders.Where(c => c.UserId == userId).ToList();
			foreach (Order o in orders)
				o.OrderItems = _context.OrderItems.Where(p => p.OrderId == o.Id).ToList();
			return orders;
		}

		public Order? GetOrderById(Guid id)
		{
			return _context.Orders.FirstOrDefault(o => o.Id == id);
		}

		public bool DeleteOrder(Guid id)
		{
			var order = _context.Orders.FirstOrDefault(o => o.Id == id);
			if (order == null)
				return false;
			order.State = OrderStates.Rejected;
			_context.Orders.Update(order);
			_context.SaveChanges();
			return true;
		}
		public Order? UpdateOrderState(Guid id, OrderStates states)
		{
			var order = _context.Orders.FirstOrDefault(c => c.Id == id);
			if (order == null)
				return null;
			order.State = states;
			_context.Orders.Update(order);
			_context.SaveChanges();
			return order;

		}
	}
}