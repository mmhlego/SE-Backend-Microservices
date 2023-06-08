using System;
using SharedModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Events.API.Models
{
    public class MessageRequests
    {
        
            public string? Content { get; set; }
            public MessageTypes Type { get; set; }
            public Guid UserId { get; set; }
        
    }
}

