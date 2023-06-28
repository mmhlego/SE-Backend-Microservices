namespace SharedModels.Events
{
#pragma warning disable CS8618
	public class MessageEvent : EventBase
	{
		public Guid UserId { get; init; }
		public string Message { get; init; }
		// public MessageTypes Type { get; init; } = MessageTypes.PlainText;
		public Guid TargetId { get; init; } = Guid.Empty;
	}
}