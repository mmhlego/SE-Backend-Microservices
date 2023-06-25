using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Chat.API.Models;
using Events.API.Models;
using Events.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Events.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize]
	public class MessageController : ControllerBase
	{
		private readonly MessageService _messageService;

		public MessageController(MessageService messageService)
		{
			_messageService = messageService;
		}

		[Route("messages")]
		[HttpGet]
		public ActionResult<List<Message>> GetMessages([FromQuery] DateTime startDate, int count, bool isRead)
		{
			_ = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid UserId);

			List<Message> messages = _messageService.GetUserMessages(UserId).Where(c => c.IsRead == isRead).ToList();
			if (startDate != default)
				messages = messages.Where(c => c.IssueDate < startDate).OrderByDescending(c => c.IssueDate).ToList();

			messages = messages.Take(count).ToList();

			if (messages == null)
				return NotFound(StatusResponse.Failed("پیامی پیدا نشد."));

			return Ok(messages);
		}

		[Route("messages/{id}")]
		[HttpPut]
		public ActionResult<StatusResponse> MarkMessageAsRead([FromQuery] Guid id)
		{
			bool success = _messageService.ReadMessage(id);

			if (!success)
				return BadRequest(StatusResponse.Failed("خطایی رخ داده."));

			return Ok(StatusResponse.Success);
		}
	}
}