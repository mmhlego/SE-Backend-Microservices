using System;
using SharedModels;

namespace Products.API.Models.Requests
{
    public class PostProductRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Guid Subcategory { get; set; }
        public ProductStates State { get; set; }
    }
}
    

