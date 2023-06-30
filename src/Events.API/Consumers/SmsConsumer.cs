using MailKit.Security;
using MassTransit;
using MimeKit;
using MimeKit.Text;
using SharedModels.Events;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Events.API.Consumers
{
	public class SmsConsumer : IConsumer<EmailEvent>
	{
		public async Task Consume(ConsumeContext<EmailEvent> context)
		{
            var message = context.Message;

            //try
            //{
            //	KavenegarApi api = new KavenegarApi("747A5A544864684F4A4D7035414E6A6A413962702B72556A6A7966534C334149714D496A357141657166593D");
            //	// var result = await api.Send("2000500666", "09146975491", $"OTP {sms.Code} To {sms.TargetPhone}");

            //	// Console.WriteLine(result);

            //	Console.WriteLine($"OTP {sms.Code} To {sms.TargetPhone}");
            //}
            //catch (Kavenegar.Core.Exceptions.ApiException ex)
            //{
            //	Console.WriteLine("Message : " + ex.Message);
            //}
            //catch (Kavenegar.Core.Exceptions.HttpException ex)
            //{
            //	Console.WriteLine("Message : " + ex.Message);
            //}
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse("erfanzadsoltani1@gmail.com"));

            email.To.Add(MailboxAddress.Parse(message.TargetEmail));
            email.Subject = "Seven Shop7";
            email.Body = new TextPart(TextFormat.Html) { Text = "code: " + message.Code };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 25, SecureSocketOptions.StartTls);
            smtp.Authenticate("erfanzadsoltani1@gmail.com", "limgxytipeiqopeo");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
	}
}