using System.ComponentModel.DataAnnotations;

#pragma warning disable CS8618
namespace General.API.Models {
    public class Poster {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public PosterTypes Type { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string TargetUrl { get; set; } = "";
    }
}