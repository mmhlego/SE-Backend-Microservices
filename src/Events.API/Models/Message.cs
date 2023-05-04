using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SharedModels;
namespace Events.API.Models
{
    public class message
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }        
        public Guid Type { get; set; }
        public string Message { get; set; }
        public DateTime IssueDate { get; set; }
        public bool isRead { get; set; }
        [ForeignKey("UserId")]
        public User user { get; set; }
        [ForeignKey("Type")]
        public messageType type { get; set; }
    }
    public class messageType {
        [Key]
        public Guid Id { get; set; }
        public string Text { get; set; }
    }
}