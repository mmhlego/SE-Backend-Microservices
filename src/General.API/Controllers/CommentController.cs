using Microsoft.AspNetCore.Mvc;
using General.API.Models.Requests;
using General.API.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using SharedModels;
using General.API.Services;
using Chat.API.Models;

namespace General.API.Controllers
{
	[ApiController]
	[Route("api/[controller]/")]
	public class CommentController : ControllerBase
	{
		private readonly CommentService _comments;
		public CommentController(CommentService comments)
		{
			_comments = comments;
		}

		[HttpGet]
		[Route("comments")]
		public ActionResult<List<ProductComment>> GetComments([FromQuery] Guid ProductId, [FromQuery] Guid CustomerId, [FromQuery] int Count, [FromQuery] DateTime StartDate)
		{
			List<ProductComment> productComments = _comments.GetComments();

			if (ProductId != default)
			{
				productComments = productComments.Where(c => c.ProductId == ProductId).ToList();
			}
			if (CustomerId != default)
			{
				productComments = productComments.Where(c => c.UserId == CustomerId).ToList();
			}
			if (StartDate != default)
			{
				productComments = productComments
				 .Where(c => c.IssueDate < StartDate)
				 .OrderByDescending(c => c.IssueDate)
				 .ToList();
			}
			if (Count != 0)
			{
				productComments = productComments.Take(Count).ToList();
			}
			return Ok(productComments);
		}

		[HttpGet]
		[Route("comments/self")]
		[Authorize(Roles = "Customer")]
		public ActionResult<List<ProductComment>> GetSelfComments()
		{
			List<ProductComment> productComments = _comments.GetComments();
			_ = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid UserId);

			productComments = productComments.Where(c => c.UserId == UserId).ToList();

			return Ok(productComments);
		}

		[HttpPost]
		[Route("comments")]
		[Authorize(Roles = "Customer")]
		public ActionResult<StatusResponse> PostComments([FromBody] PostCommentRequests comment)
		{
			_ = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid UserId);

			if (!ModelState.IsValid)
			{
				return BadRequest(StatusResponse.Failed("در اضافه کردن کامنت خطایی رخ داده."));
			}

			_comments.AddComment(UserId, comment.ProductId, comment.Text);
			return Ok(StatusResponse.Success);
		}

		[HttpDelete]
		[Route("comments/{id}")]
		public ActionResult<ProductComment> DeleteComment(Guid commentId)
		{
			_ = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid UserId);
			_ = Enum.TryParse(User.FindFirstValue(ClaimTypes.Role), out UserTypes role);
			ProductComment? comment = _comments.GetCommentById(commentId);
			if (comment == null)
			{
				return NotFound(StatusResponse.Failed("کامنت مورد نظر پیدا نشد."));
			}

			if (comment.UserId != UserId && role != UserTypes.Admin)
			{
				return Unauthorized(StatusResponse.Failed("اجازه دسترسی ندارید."));
			}
			_comments.DeleteComment(commentId);
			return Ok(comment);
		}
	}
}
