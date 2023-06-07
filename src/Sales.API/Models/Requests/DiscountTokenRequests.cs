using System;
namespace Sales.API.Models.Requests
{
    public class PostDiscountToken
    {
        public DiscountTokenTypes Type;
        public int Amount { get; set; }
        public int AvailableCount { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}

