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
		public async Task<ActionResult<List<Product>>> GetProducts(string? search, decimal? priceFrom, decimal? priceTo, Guid? subcategoryId)
		{
			if (priceTo != null && priceFrom != null && priceFrom > priceTo)
				return Ok(StatusResponse.Failed("فیلتر قیمت گذاری درست وارد نشده."));
			List<Product> products = new List<Product>();
			if (priceTo != null || priceFrom != null)
			{
				var salePricesEndpoint = $"http://localhost:6004/api/sale/getproductIds?priceFrom={priceFrom}&priceTo={priceTo}"; //TODO: Change
				var response = await _httpClient.GetAsync(salePricesEndpoint);

				// Check the response status code
				if (response.IsSuccessStatusCode == false)
					return Ok(StatusResponse.Failed("خطایی رخ داده."));

				var responseContent = await response.Content.ReadAsStringAsync();
				var productIds = JsonConvert.DeserializeObject<List<Guid>>(responseContent);
				if (productIds == null)
					return Ok(StatusResponse.Failed("کالایی پیدا نشد."));
				productIds = productIds.Distinct().ToList();

				List<Product?> p = productIds.Select(c => _productService.GetProductById(c)).ToList();
				p.RemoveAll(m => m == null);

				products = p;
			}
			else products = _productService.GetProducts();
			if (subcategoryId != Guid.Empty)
				products = products.Where(c => c.SubcategoryId == subcategoryId).ToList();
			if (search != null)
				products = _productService.SearchProductsByName(search, products);
			if (products == null)
				return Ok(StatusResponse.Failed("کالایی پیدا نشد."));

			return Ok(products);
		}

		[HttpPost("products")]
		[Authorize(Roles = "Admin , StoreKeeper , Seller")]
		public ActionResult<StatusResponse> AddProduct(PostProductRequest productToAdd)
		{
			if (productToAdd.Name == null || productToAdd.Description == null)
				return Ok(StatusResponse.Failed("اطلاعات کالا را کامل وارد نکردید."));
			_productService.AddProduct(productToAdd.Name, productToAdd.Description, productToAdd.Subcategory, productToAdd.State);
			return Ok(StatusResponse.Success);
		}

		[HttpPut("products/{id}")]
		public ActionResult<StatusResponse> UpdateProduct(Guid id, PostProductRequest product)
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