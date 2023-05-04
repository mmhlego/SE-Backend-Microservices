using System.ComponentModel.DataAnnotations;

namespace General.API.Models {
    public class like {
        [Key]
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public bool Like { get; set; }

    }
}