#pragma warning disable CS8618
namespace SharedModels.Events
{
	public class SmsEvent : EventBase
	{
		public string TargetPhone { get; init; }
		public string Code { get; set; }
		public SmsTypes Type { get; init; }
	}

	public enum SmsTypes
	{
		Login, // Send to user
		Festival, // Send to all customers
		CartChange, // Send to customer
	}
}