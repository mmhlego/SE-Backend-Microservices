using System;
namespace General.API.Models.Requests
{
	public class ReactionCount
	{
		public int Likes { get; set; }
		public int Dislikes { get; set; }
	}

	public class AddRequest
	{
		public Guid TargetId { get; set; }
		public ReactionTypes Type { get; set; }
		public bool Like { get; set; }
	}

	public class DeleteRequest
	{
		public Guid TargetId { get; set; }
		public ReactionTypes Type { get; set; }
	}
}

