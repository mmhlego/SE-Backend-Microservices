using General.API.Models;
using Microsoft.EntityFrameworkCore;

namespace General.API.Data {
    public class GeneralContext : DbContext
    {
        public GeneralContext(DbContextOptions<GeneralContext> options) : base(options)
        {
        }

        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<Poster> Posters { get; set; }
        public DbSet<ProductComment> ProductComments { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
    }
}