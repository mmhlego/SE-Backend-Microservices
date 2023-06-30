using Kavenegar;
using MassTransit;
using SharedModels.Events;

namespace Events.API.Consumers
{
	public class SmsConsumer : IConsumer<SmsEvent>
	{
		public async Task Consume(ConsumeContext<SmsEvent> context)
		{
			var sms = context.Message;

			try
			{
				KavenegarApi api = new KavenegarApi("747A5A544864684F4A4D7035414E6A6A413962702B72556A6A7966534C334149714D496A357141657166593D");
				// var result = await api.Send("2000500666", "09146975491", $"OTP {sms.Code} To {sms.TargetPhone}");

				// Console.WriteLine(result);

				Console.WriteLine($"OTP {sms.Code} To {sms.TargetPhone}");
			}
			catch (Kavenegar.Core.Exceptions.ApiException ex)
			{
				Console.WriteLine("Message : " + ex.Message);
			}
			catch (Kavenegar.Core.Exceptions.HttpException ex)
			{
				Console.WriteLine("Message : " + ex.Message);
			}
		}
	}
}