using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using General.API.Models;

namespace General.API.Services
{
    public interface IBookmarkService
    {
        List<Bookmark> GetBookmarksByUserId(Guid userId);
        List<Bookmark> GetBookmarksByUserIdAndProductId(Guid userId, Guid productId);
        bool AddBookmark(Guid userId, Guid productId);
        bool DeleteBookmark(Bookmark bookmark);

    }
}