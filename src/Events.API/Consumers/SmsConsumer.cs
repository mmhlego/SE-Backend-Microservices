using MassTransit;
using SharedModels.Events;

namespace Events.API.Consumers
{
	public class SmsConsumer : IConsumer<SmsEvent>
	{
		public async Task Consume(ConsumeContext<SmsEvent> context)
		{
			var sms = context.Message;

			Console.WriteLine(sms.TargetPhone);
		}
	}
}