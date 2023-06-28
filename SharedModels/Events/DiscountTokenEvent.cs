#pragma warning disable CS8618

namespace SharedModels.Events
{
	public class DiscountTokenEvent : EventBase
	{
		public int Amount { get; set; }
		public DateTime ExpiryDate { get; set; }
		public string TargetPhone { get; init; }
	}
}