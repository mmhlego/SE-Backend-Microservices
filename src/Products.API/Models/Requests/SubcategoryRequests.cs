using System;
namespace Products.API.Models.Requests
{
    public class SubcategoryRequests
    
        {
            public string? Title { get; set; }
        public Guid CategoryId { get; internal set; }
    }
    
}

