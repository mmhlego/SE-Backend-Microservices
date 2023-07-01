using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Chat.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Products.API.Models.Requests;
using Products.API.Services;
using SharedModels;

namespace Products.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly HttpClient _httpClient;
        public ProductController(IProductService productService)
        {
            _productService = productService;
            _httpClient = new HttpClient();
        }

        [HttpGet("products")]
        public ActionResult<List<Product>> GetProducts([FromQuery] string? search, [FromQuery] decimal? priceFrom, [FromQuery] decimal? priceTo, [FromQuery] Guid? subcategoryId)
        {

            if (priceTo != null && priceFrom != null && priceFrom > priceTo)
                return Ok(StatusResponse.Failed("فیلتر قیمت گذاری درست وارد نشده."));
            List<Product> products = new List<Product>();
            if (priceTo != null || priceFrom != null)
                products = _productService.FilterProductsByPrice(priceFrom, priceTo);
            else products = _productService.GetProducts();

            if (products == null)
                return Ok(StatusResponse.Failed("کالایی پیدا نشد."));

            if (subcategoryId != null)
                products = products.Where(c => c.SubcategoryId == subcategoryId).ToList();
            if (search != null)
                products = _productService.SearchProductsByName(search, products);
            if (products == null)
                return Ok(StatusResponse.Failed("کالایی پیدا نشد."));

            return Ok(products);
        }

        [HttpPost("products")]
        [Authorize(Roles = "Admin , StoreKeeper , Seller")]
        public ActionResult<StatusResponse> AddProduct([FromBody] PostProductRequest productToAdd)
        {
            if (productToAdd.Name == null || productToAdd.Description == null)
                return Ok(StatusResponse.Failed("اطلاعات کالا را کامل وارد نکردید."));
            var p = _productService.AddProduct(productToAdd.Name, productToAdd.Description, productToAdd.Subcategory, productToAdd.State);
            if (p == null)
                return Ok(StatusResponse.Failed("زیردسته را درست وارد نکردید."));
            return Ok(StatusResponse.Success);
        }

        [HttpPut("products/{id}")]
        [Authorize(Roles = "Admin , StoreKeeper , Seller")]
        public ActionResult<StatusResponse> UpdateProduct(Guid id, [FromBody] PostProductRequest product)
        {
            var existingProduct = _productService.GetProductById(id);
            if (existingProduct == null)
            {
                return Ok(StatusResponse.Failed("کالای مورد نظر پیدا نشد."));
            }
            if (product.Name != null)
                existingProduct.Name = product.Name;
            if (product.Description != null)
                existingProduct.Description = product.Description;
            existingProduct.SubcategoryId = product.Subcategory;
            existingProduct.State = product.State;

            _productService.UpdateProduct(existingProduct);
            return Ok(StatusResponse.Success);
        }
    }
}