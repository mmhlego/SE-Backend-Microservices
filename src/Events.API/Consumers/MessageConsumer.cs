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
				// new message for targetId user
			}
			else
			{
				// new message for all users
			}
		}
	}
}