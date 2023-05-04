using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Products.API.Models {
    public class ProductRequest {
        [Key]
        public Guid Id { get; set; } 
        public Guid SellerId  { get; set; }
        public Guid RowId { get; set; }
    }
}