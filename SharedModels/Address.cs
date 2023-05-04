using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.API.Models {
    public class address {
        [Key]
        public Guid Id { get; set; }
        public string Address { get; set; }
    }
}