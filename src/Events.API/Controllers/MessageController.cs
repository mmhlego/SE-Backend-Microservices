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
	[Route("api/[controller]/")]
	[Authorize]
	public class MessageController : ControllerBase
	{
		private readonly IMessageService _messageService;

		public MessageController(IMessageService messageService)
		{
			_messageService = messageService;
		}

		[Route("messages")]
		[HttpGet]
		public ActionResult<List<Message>> GetMessages([FromQuery] DateTime startDate, int count, bool? isRead = null)
		{
			_ = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid UserId);

			List<Message> messages = _messageService.GetUserMessages(UserId).OrderByDescending(c => c.IssueDate).ToList();

			if (isRead != null)
				messages = messages.Where(c => c.IsRead == isRead).ToList();

			if (startDate != default)
				messages = messages.Where(c => c.IssueDate < startDate).OrderByDescending(c => c.IssueDate).ToList();

			messages = messages.Take(count).ToList();

			if (messages == null)
				return Ok(StatusResponse.Failed("پیامی پیدا نشد."));

			return Ok(messages);
		}

		[Route("messages/{id}")]
		[HttpPut]
		public ActionResult<StatusResponse> MarkMessageAsRead(Guid id)
		{
			bool success = _messageService.ReadMessage(id);

			if (!success)
				return Ok(StatusResponse.Failed("خطایی رخ داده."));

			return Ok(StatusResponse.Success);
		}
	}
}