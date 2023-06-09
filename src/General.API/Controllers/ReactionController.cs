using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Chat.API.Models;
using General.API.Models;
using General.API.Models.Requests;
using General.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace General.API.Controllers
{
	[ApiController]
	[Route("api/[controller]/")]
	public class ReactionController : ControllerBase
	{
		private readonly IReactionService _reactionService;

		public ReactionController(IReactionService reactionService)
		{
			_reactionService = reactionService;
		}

		[HttpGet("likes")]
		public ActionResult<ReactionCount> GetLikes([FromQuery] Guid targetId, [FromQuery] ReactionTypes type)
		{
			var likes = _reactionService.GetLikes(targetId, type);
			var dislikes = _reactionService.GetDislikes(targetId, type);

			var reactionCount = new ReactionCount
			{
				Likes = likes,
				Dislikes = dislikes
			};

			return Ok(reactionCount);
		}

		[HttpPost("likes")]
		[Authorize(Roles = "Customer")]
		public ActionResult<StatusResponse> AddReaction(AddRequest request)
		{
			_ = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid UserId);
			bool success = _reactionService.AddReaction(UserId, request.TargetId, request.Type, request.Like);
			if (success)
			{
				return Ok(StatusResponse.Success);
			}

			return Ok(StatusResponse.Failed("واکنش مشتری از قبل ثبت شده."));
		}

		[HttpDelete("likes")]
		[Authorize(Roles = "Customer")]
		public ActionResult<StatusResponse> DeleteReaction([FromBody] DeleteRequest request)
		{
			_ = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid UserId);
			bool success = _reactionService.DeleteReaction(UserId, request.TargetId, request.Type);
			if (success)
			{
				return Ok(StatusResponse.Success);
			}

			return Ok(StatusResponse.Failed("واکنش مورد نظر پیدا نشد."));
		}
	}
}