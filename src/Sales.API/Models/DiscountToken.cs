using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Users.API.Models;

namespace Sales.API.Models
{
    public class discountToken
    {
        [Key]
        public Guid Id { get; set; }
        public string Token { get; set; }
        public string[] Type = { "Percent", "Price" };
        public int Amount { get; set; }
        public DateTime ExpireDate { get; set; }
        public List<Purchase> Purchase { get; set; }

    }
}