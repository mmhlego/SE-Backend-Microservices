using System;
namespace Sales.API.Models.Requests
{
    public class PostSaleRequest
    {
        public Guid userId { get; set; }
        public Guid productId { get; set; }
        public int amount { get; set; }
        public long initialPrice { get; set; }
    }
}

