using General.API.Models;
using SharedModels;
namespace General.API.Services {
    public interface IBookmarkService {
        List<Bookmark> GetBookmarksByUserId(Guid userId);
        List<Bookmark> GetBookmarksByUserIdAndProductId(Guid userId, Guid productId);
        bool AddBookmark(Guid userId, Guid productId);
        bool DeleteBookmark(Bookmark bookmark);
    }
}