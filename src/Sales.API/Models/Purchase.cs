using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.API.Models
{
    public class Purchase
    {
        [Key]
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid DiscountToken { get; set; }
        public int TotalPrice { get; set; }
        public string Description   { get; set; }
        public DateTime PurchaseDate { get; set; }

        [ForeignKey("OrderId")]
        public Order order { get; set; }
        [ForeignKey("DiscountToken")]
        public discountToken discountToken { get; set; }
    }
}