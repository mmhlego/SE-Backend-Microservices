using General.API.Data;
using General.API.Models;

namespace General.API.Services {
    public class BookmarkService : IBookmarkService {
        private GeneralContext _context;

        public BookmarkService(GeneralContext context) {
            _context = context;
        }

        public List<Bookmark> GetBookmarksByUserId(Guid userId) {
            // try {
            //     if (userId == default(Guid)) {
            //         throw new ArgumentNullException("UserId cannot be null");
            //     }
            return _context.Bookmarks.Where(b => b.UserId == userId).ToList();
            // } catch (Exception ex) {
            //     throw new Exception("An error occurred while getting bookmarks by userId", ex);
            // }
        }

        public List<Bookmark> GetBookmarksByUserIdAndProductId(Guid userId, Guid productId) {
            // try {
            //     if (userId == default(Guid) || productId == default(Guid)) {
            //         throw new ArgumentNullException("UserId and ProductId cannot be null");
            //     }
            return _context.Bookmarks.Where(b => b.UserId == userId && b.ProductId == productId).ToList();
            // } catch (Exception ex) {
            //     throw new Exception("An error occurred while getting bookmarks by userId and productId", ex);
            // }
        }

        public bool AddBookmark(Guid userId, Guid productId) {
            // try {
            //     if (userId == default(Guid) || productId == default(Guid)) {
            //         throw new ArgumentNullException("UserId and ProductId cannot be null");
            //     }
            if (!_context.Bookmarks.Any(b => b.UserId == userId && b.ProductId == productId)) {
                _context.Bookmarks.Add(new Bookmark { UserId = userId, ProductId = productId });
                _context.SaveChanges();
                return true;
            } else {
                return false;
            }
            // } catch (Exception ex) {
            //     throw new Exception("An error occurred while adding a bookmark", ex);
            // }
        }

        public bool DeleteBookmark(Bookmark bookmark) {
            // try {
            // if (bookmark == null) {
            //     throw new ArgumentNullException("Bookmark cannot be null");
            // }
            if (!_context.Bookmarks.Any(b => b.Id == bookmark.Id))
                return false;

            // var b = _context.Bookmarks.FirstOrDefault(b => b.Id == bookmark.Id);
            // if (b == null) {
            //     throw new Exception("Bookmark Not Found");
            // }

            _context.Bookmarks.Remove(bookmark);
            _context.SaveChanges();
            return true;

            // } catch (Exception ex) {
            //     throw new Exception("An error occurred while deleting a bookmark", ex);
            // }
        }
    }
}