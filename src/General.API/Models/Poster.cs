using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace General.API.Models
{
    public class Poster
    {
        [Key]
        public Guid Id { get; set; }
        public Guid Type { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string TargetUrl { get; set; }

    }
}