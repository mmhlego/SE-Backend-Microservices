#pragma warning disable CS8618
namespace SharedModels.Events
{
	public class SmsEvent : EventBase
	{
		public Guid? TargetId { get; init; } = null;
		public string? Code { get; set; } = null;
		public SmsTypes Type { get; init; }
	}

	public enum SmsTypes
	{
		Login, // Send to user
		Festival, // Send to all customers
		CartChange, // Send to customer
	}
}