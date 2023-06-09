using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SharedModels;
using SharedModels.Events;
#pragma warning disable CS8618
namespace Events.API.Models
{
	public class Message
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();
		public string Content { get; set; }
		public MessageTypes Type { get; set; }
		public DateTime IssueDate { get; set; } = DateTime.Now;
		public bool IsRead { get; set; } = false;

		public Guid UserId { get; set; }

	}
}