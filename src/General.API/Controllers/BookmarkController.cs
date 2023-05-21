using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using General.API.Data;
using General.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace General.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookmarkController : ControllerBase
    {
        private GeneralContext _context;

        public BookmarkController(GeneralContext context)
        {
            _context = context;
        }
        [HttpDelete]
        [Route("/test")]
        public bool DeleteBookmark(Bookmark bookmark)
        {
            try
            {
                if (bookmark == null)
                {
                    throw new ArgumentNullException("Bookmark cannot be null");
                }
                _context.Bookmarks.Remove(bookmark);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while deleting a bookmark", ex);
            }
        }
    }
}