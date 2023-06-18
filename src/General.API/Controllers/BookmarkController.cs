using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Chat.API.Models;
using General.API.Data;
using General.API.Models;
using General.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace General.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookmarkController : ControllerBase
    {
        private readonly BookmarkService _bookmarkService;

        public BookmarkController(BookmarkService bookmarkService)
        {
            _bookmarkService = bookmarkService;
        }

        [HttpGet]
        [Route("/bookmarks")]
        public ActionResult<List<Bookmark>> GetBookmarks()
        {
            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid UserId);
            List<Bookmark> bookmarks = _bookmarkService.GetBookmarksByUserId(UserId);

            return Ok(bookmarks);
        }

        [HttpPost]
        [Route("/bookmark")]
        public ActionResult<StatusResponse> AddBookmark([FromQuery] Guid productId)
        {
            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid UserId);


            bool success = _bookmarkService.AddBookmark(UserId, productId);

            if (success)
            {
                return Ok(StatusResponse.Success);
            }

            return BadRequest(StatusResponse.Failed("در اضافه کردن نشانک خطایی رخ داد."));
        }

        [HttpDelete]
        [Route("/bookmark")]
        public ActionResult<StatusResponse> DeleteBookmark([FromQuery] Guid productId)
        {
            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid UserId);
            Bookmark bookmark = new Bookmark()
            {
                ProductId = productId,
                UserId = UserId
            };
            bool success = _bookmarkService.DeleteBookmark(bookmark);

            if (success)
            {
                return Ok(StatusResponse.Success);
            }

            return BadRequest(StatusResponse.Failed("در حذف کردن نشانک خطایی رخ داد."));
        }

    }
}
