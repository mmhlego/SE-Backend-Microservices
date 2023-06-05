using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using General.API.Data;
using General.API.Models;
using General.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace General.API.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class BookmarkController : ControllerBase {
            private readonly BookmarkService _bookmarkService;

            public BookmarkController(BookmarkService bookmarkService)
            {
                _bookmarkService = bookmarkService;
            }

        [HttpGet]
        [Route("/bookmarks")]
            public ActionResult<List<Bookmark>> GetBookmarks()
        { 
                List<Bookmark> bookmarks = _bookmarkService.GetBookmarksByUserId(GetUserId());

                return Ok(bookmarks);
            }

            [HttpPost]
            public ActionResult AddBookmark([FromQuery] Guid productId)
            {
               

                bool success = _bookmarkService.AddBookmark(GetUserId(), productId);

                if (success)
                {
                    return Ok();
                }

                return BadRequest();
            }

            [HttpDelete]
            public ActionResult DeleteBookmark([FromQuery] Guid productId)
            {
            Bookmark bookmark = new Bookmark()
            {
                ProductId = productId,
                UserId = GetUserId()
            };
                bool success = _bookmarkService.DeleteBookmark(bookmark);

                if (success)
                {
                    return Ok();
                }

                return BadRequest();
            }
        public Guid GetUserId()
        {
            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid UserId);
            return UserId;
        }
    }
    }
