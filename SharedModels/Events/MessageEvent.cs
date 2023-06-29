namespace SharedModels.Events
{
#pragma warning disable CS8618
	public class MessageEvent : EventBase
	{
		public Guid TargetId { get; init; }
		public string Content { get; init; }
		public MessageTypes Type { get; init; }
	}
}