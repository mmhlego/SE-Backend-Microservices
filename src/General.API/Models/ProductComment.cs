using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SharedModels;

#pragma warning disable CS8618
namespace General.API.Models {
    public class ProductComment {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Content { get; set; }
        public DateTime IssueDate { get; set; } = DateTime.Now;

        public Guid UserId { get; set; }
        

        public Guid ProductId { get; set; }
      
    }
}