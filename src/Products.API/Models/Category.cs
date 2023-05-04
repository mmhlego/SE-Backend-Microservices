using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Products.API.Models
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public List<Subcategory> SubCategory { get; set; }
    }
}