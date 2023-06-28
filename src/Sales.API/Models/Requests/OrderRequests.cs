using System;
namespace Sales.API.Models.Requests
{
   public class PostOrderItem
    {
        public Guid SalePriceId { get; set; }
        public int Amount  { get; set; }
    }
}

