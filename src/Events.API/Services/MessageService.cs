using Events.API.Data;
using Events.API.Models;
using SharedModels;
using SharedModels.Events;

namespace Events.API.Services
{
	public class MessageService : IMessageService
	{
		private readonly EventsContext _context;
		public MessageService(EventsContext context)
		{
			_context = context;
		}
		public List<Message> GetUserMessages(Guid userId)
		{
			// if (userId == Guid.Empty)
			// throw new ArgumentNullException("Invalid user id");
			// return new List<Message>();

			return _context.Messages.Where(x => x.UserId == userId).ToList();
		}

		public void AddMessage(Guid userId, string content, MessageTypes type)
		{
			// if (userId == Guid.Empty)
			//     throw new ArgumentNullException("Invalid user id");

			// if (string.IsNullOrWhiteSpace(content))
			//     throw new ArgumentNullException("Content cannot be empty");

			var message = new Message
			{
				UserId = userId,
				Content = content,
				Type = type
			};

			_context.Messages.Add(message);
			_context.SaveChanges();
		}

		public bool ReadMessage(Guid id)
		{
			// if (Id == Guid.Empty)
			//     throw new ArgumentNullException("Invalid message id");

			var message = _context.Messages.FirstOrDefault(x => x.Id == id);
			if (message == null)
				return false;
			// throw new ArgumentNullException("Message doesn't exist");

			message.IsRead = true;
			_context.Messages.Update(message);
			_context.SaveChanges();
			return true;
		}

		public void addMessageToAllCustomers(string content, MessageTypes type)
		{
			List<Bookmark> bookmarks = _context.bookmarks.ToList();
			List<Sales.API.Models.Sale> sales = _context.Sales.ToList();
			List<Customer> customers = _context.Customers.ToList();
			List<Guid> sellersId = _context.bookmarks
					  .Where(bookmark => _context.Sales.Any(sale => sale.ProductId == bookmark.ProductId))
					  .Select(bookmark => _context.Sales.First(sale => sale.ProductId == bookmark.ProductId).UserId)
					  .ToList();

			if (type == MessageTypes.Festival)
			{
				foreach (Customer customer in customers)
				{
					AddMessage(customer.UserId, content, type);
				}
			}
			else if (type == MessageTypes.ProductFinished || type == MessageTypes.SaleAvailable || type == MessageTypes.ProductAvailable)
			{
				foreach (Bookmark bookmark in bookmarks)
				{
					AddMessage(bookmark.UserId, content, type);
				}

				if (type != MessageTypes.ProductAvailable)
				{
					foreach (var sellerId in sellersId)
					{
						AddMessage(sellerId, content, type);
					}
				}
			}
			else if (type == MessageTypes.SaleFinished)
			{
				foreach (var sellerId in sellersId)
				{
					AddMessage(sellerId, content, type);
				}
			}
		}
	}
}