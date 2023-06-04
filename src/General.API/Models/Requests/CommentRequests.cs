namespace General.API.Models.Requests
{
    public class GetCommentsRequests {
        public Guid ProductId { get; set; }
        public Guid CustomerId { get; set; }
        public int Count { get; set; }
        public DateTime StartDate { get; set; }
    }
    public class PostCommentRequests
    {
        public Guid ProductId { get; set; }
        public string Text { get; set; } = "";
    }
}
