using Products.API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.API.Models
{
    public class Sale
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Amount { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; } 
    }
}