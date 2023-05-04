using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Products.API.Models {
    public class Subcategory {
        [Key]
        public Guid Id { get; set; }
        public Guid Category { get; set; }
        public string Title { get; set; }
        [ForeignKey("Category")]
        public Category category { get; set; }
    }
}