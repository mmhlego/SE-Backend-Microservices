using Events.API.Models;
using Events.API.Services;
using MassTransit;
using SharedModels.Events;

namespace Events.API.Consumers
{
	public class MessageConsumer : IConsumer<MessageEvent>
	{
		private readonly IMessageService _messageService;

		public MessageConsumer(IMessageService messageService)
		{
			_messageService = messageService;
		}

		public async Task Consume(ConsumeContext<MessageEvent> context)
		{
			var messageEvent = context.Message;

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