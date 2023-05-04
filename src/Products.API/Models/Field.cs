using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Products.API.Models {
    public class Field {
        [Key]
        public Guid Id { get; set; }
        public Guid Subcategory { get; set; }
        public string FieldName { get; set; }
    }
}