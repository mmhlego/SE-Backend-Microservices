namespace SharedModels.Events
{
#pragma warning disable CS8618
	public class SmsEvent : EventBase
	{
		public string TargetPhone { get; init; }
		public string? Code { get; set; } = null;
		public SmsTypes Type { get; init; }
	}

	public enum SmsTypes
	{
		Login,
		DoctorPatient,
		ClinicPatient,
		ClinicDoctor,
	}
}