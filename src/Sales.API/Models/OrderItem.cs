using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.API.Models
{
    public class orderItem
    {
        [Key]
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid PriceId { get; set; }
        public int Amount  { get; set; }
        [ForeignKey("OrderId")]
        public Order order { get; set; }
        [ForeignKey("PriceId")]
        public salePrice SalePrice { get; set; }
    }
}