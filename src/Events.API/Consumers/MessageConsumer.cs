using Events.API.Models;
using Events.API.Services;
using SharedModels.Events;

namespace Events.API.Consumers
{
	public class MessageConsumer
	{
		private readonly IMessageService _messageService;

		public MessageConsumer(IMessageService messageService)
		{
			_messageService = messageService;
		}

		public async Task Consume(MessageEvent messageEvent)
		{
			if (messageEvent.TargetId != null)
			{
				_messageService.AddMessage(messageEvent.TargetId, messageEvent.Content, messageEvent.Type);
			}
			else
			{
				_messageService.addMessageToAllCustomers(messageEvent.Content, messageEvent.Type);
			}
		}
	}
}