using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace General.API.Models
{
    public class ProductComment
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Content { get; set; }
        public DateTime IssueDate { get; set; }
    }
}