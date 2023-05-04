using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace General.API.Models
{
    public class Bookmark
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ProductId  { get; set; }
    }
}