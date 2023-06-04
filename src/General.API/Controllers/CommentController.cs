using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using General.API.Data;
using General.API.Models.Requests;
using General.API.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using SharedModels;
using Microsoft.EntityFrameworkCore;
using General.API.Services;

namespace General.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        CommentService _comments;
        public CommentController(CommentService comments)
        {
            _comments = comments;
        }

        [HttpGet]
        [Route("comments/")]
        public IActionResult GetComments(GetCommentsRequests request)
        {

            List<ProductComment> productComments = _comments.GetComments();

            if (request.ProductId != default(Guid))
            {
                productComments = productComments.Where(c => c.ProductId == request.ProductId).ToList();
            }
            if (request.CustomerId != default(Guid))
            {
                productComments = productComments.Where(c => c.UserId == request.CustomerId).ToList();
            }
            if (request.StartDate != default(DateTime))
            {
                productComments = productComments
                 .Where(c => c.IssueDate < request.StartDate)
                 .OrderByDescending(c => c.IssueDate)
                 .ToList();
            }
            if (request.Count != 0)
            {
                productComments.Take(request.Count);
            }
            return Ok(productComments);
        }

        [HttpPost]
        [Route("comments/")]
        [Authorize(Roles = "Customer")]
        public IActionResult PostComments([FromBody] PostCommentRequests comment)
        {
            bool checkUser = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid UserId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (Enum.TryParse(User.FindFirstValue(ClaimTypes.Role), out UserTypes role))
            {
                _comments.AddComment(UserId, comment.ProductId, comment.Text);
            }
            return Ok();
        }

        [HttpDelete]
        [Route("comments/{id}")]
        public IActionResult DeleteComment(Guid commentId)
        {
            ProductComment? comment = _comments.GetCommentById(commentId);
            if (comment == null)
            {
                return BadRequest();
            }

            if (comment.UserId != GetUserId() && GetAccessLevel() != UserTypes.Admin)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            _comments.DeleteComment(commentId);
            return Ok(comment);

        }
        public Guid GetUserId()
        {
            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid UserId);
            return UserId;
        }
        public UserTypes GetAccessLevel()
        {
            Enum.TryParse(User.FindFirstValue(ClaimTypes.Role), out UserTypes role);
            return role;
        }
    }
}
