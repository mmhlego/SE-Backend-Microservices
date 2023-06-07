using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Events.API.Models;
using Events.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Events.API.Controllers {
    [ApiController]
    [Route("messages")]
    public class MessageController : ControllerBase {
        private readonly MessageService _messageService;

        public MessageController(MessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet]
        public ActionResult<List<Message>> GetMessages([FromQuery] DateTime startDate, int count, bool isRead)
        {
            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid UserId);
            if (UserId == Guid.Empty)
                return Unauthorized();
            List<Message> messages = _messageService.GetUserMessages(UserId).Where(c => c.IsRead == isRead).ToList();
            if (startDate != default(DateTime))
               messages = messages.Where(c => c.IssueDate < startDate).OrderByDescending(c => c.IssueDate).ToList();
           messages = messages.Take(count).ToList();
            if (messages == null)
                return NotFound();
            return Ok(messages);
        }
        [HttpPost]
        [Authorize(Roles = "Owner, Admin, StoreKeeper,Seller,")]
        public IActionResult PostMessages(MessageRequests message)
        { 
            _messageService.AddMessage(message.UserId, message.Content, message.Type);
            return Ok();

        }

        [HttpPut("{id}")]
        public ActionResult MarkMessageAsRead(Guid id)
        { 
            bool success = _messageService.ReadMessage(id);

            if (success)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}