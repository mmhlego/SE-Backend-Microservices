using General.API.Models;
using Microsoft.EntityFrameworkCore;
using SharedModels;
namespace General.API.Data {
    public class GeneralContext : DbContext {
        public GeneralContext(DbContextOptions<GeneralContext> options) : base(options) { }

        public DbSet<Bookmark> Bookmarks { get; set; } = null!;
        public DbSet<Poster> Posters { get; set; } = null!;
        public DbSet<ProductComment> ProductComments { get; set; } = null!;
        public DbSet<Reaction> Reactions { get; set; } = null!;
    }
}