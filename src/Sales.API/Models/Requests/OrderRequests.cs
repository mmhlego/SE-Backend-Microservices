using System;
namespace Sales.API.Models.Requests
{
    public class PostOrderItem
    {
        public Guid SalePriceId { get; set; }
        public int Amount { get; set; }
    }
    public class CalculateCart
    {
        public Decimal Sum { get; set; }
        public Decimal Discount { get; set; }
        public decimal Price { get; set; }
    }
}

