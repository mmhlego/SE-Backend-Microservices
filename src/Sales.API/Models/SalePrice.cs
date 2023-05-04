using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.API.Models {
    public class salePrice {
        [Key]
        public Guid Id { get; set; }
        public Guid SaleId { get; set; }
        public int Price { get; set; }
        public DateTime UpdateDate  { get; set; }
        public List<orderItem> OrderItem { get; set; }
    }
}