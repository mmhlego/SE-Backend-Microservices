using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Users.API.Models;

namespace Sales.API.Models
{
    public class Order
    {
        public Guid Id  { get; set; }
        public Guid State { get; set; }
        public int TotalPrice { get; set; }
        [ForeignKey("State")]
        public orderStates orderStates { get; set; }
        public List<orderItem> OrderItem { get; set; }
        public Purchase Purchase { get; set; }

    }
}