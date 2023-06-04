namespace General.API.Models.Requests
{
    public class PosterPost
    {
        public PosterTypes Type { get; set; }
        public string Title { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public string TargetUrl { get; set; } = ""; 
    }
}
