using Chat.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.API.Models;
using Products.API.Models.Requests;
using Products.API.Services;

namespace Products.API.Controllers
{
	[ApiController]
	[Route("api/[controller]/")]
	public class CategoryController : ControllerBase
	{
		private readonly ICategoryService _categoryService;

		public CategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		[HttpGet]
		[Route("categories")]
		public ActionResult<List<Category>> GetCategories()
		{
			var categories = _categoryService.GetCategories();
			return Ok(categories);
		}

		[HttpGet]
		[Route("subcategories")]
		public ActionResult<List<Subcategory>> GetSubcategories()
		{
			var subcategories = _categoryService.GetSubcategories(default);
			return Ok(subcategories);
		}

		[HttpGet]
		[Route("categories/{categoryId}/subcategories")]
		public ActionResult<List<Subcategory>> GetSubcategories(Guid categoryId)
		{
			var subcategories = _categoryService.GetSubcategories(categoryId);
			return Ok(subcategories);
		}

		[HttpPost]
		[Route("categories")]
		[Authorize(Roles = "Admin , StoreKeeper")]
		public ActionResult<StatusResponse> AddCategory([FromBody] CategoryRequests categoryRequest)
		{
			if (categoryRequest == null)
			{
				return BadRequest(StatusResponse.Failed("اطلاعات دسته بندی را وارد نکردید."));
			}

			_categoryService.AddCategory(categoryRequest.Title, categoryRequest.IconName);
			return Ok(StatusResponse.Success);
		}

		[HttpPost]
		[Route("subcategories")]
		[Authorize(Roles = "Admin , StoreKeeper , Seller")]
		public ActionResult AddSubcategory([FromBody] SubcategoryRequests subcategoryRequests)
		{
			if (_categoryService.GetCategoryById(subcategoryRequests.CategoryId) == null)
				return NotFound(StatusResponse.Failed("دسته بندی مورد نظر پیدا نشد."));
			_categoryService.AddSubcategory(subcategoryRequests.CategoryId, subcategoryRequests.Title);
			return Ok(StatusResponse.Success);
		}

		[HttpPut]
		[Route("categories/{id}")]
		[Authorize(Roles = "Admin , StoreKeeper")]
		public ActionResult UpdateCategory(Guid id, [FromBody] CategoryRequests categoryRequests)
		{
			var category = _categoryService.GetCategoryById(id);
			if (category == null)
				return NotFound(StatusResponse.Failed("دسته بندی مورد نظر پیدا نشد."));
			category.Title = categoryRequests.Title;
			category.IconName = categoryRequests.IconName;
			_categoryService.UpdateCategory(category);
			return Ok(StatusResponse.Success);
		}

		[HttpGet]
		[Route("subcategories/{id}/fields")]
		public ActionResult<List<Field>> GetSubcategoryFields(Guid id)
		{
			var c = _categoryService.GetFields(id);
			if (c == null)
				return Ok(Array.Empty<string>());

			return Ok(c);
		}

		[HttpPost]
		[Route("subcategories/{id}/fields")]
		[Authorize(Roles = "Admin , StoreKeeper , Seller")]
		public ActionResult<StatusResponse> UpdateSubcategoryFields(Guid id, [FromBody] List<string> fields)
		{
			if (id == default)
				return BadRequest(StatusResponse.Failed(" اطلاعات زیردسته را وارد نکردید."));

			if (_categoryService.GetSubcategoryById(id) == null)
				return NotFound(StatusResponse.Failed("زیردسته مورد نظر پیدا نشد."));
			var c = _categoryService.GetFields(id);
			if (c != null)
				_categoryService.DeleteFieldList(c);
			foreach (string p in fields)
				_categoryService.AddField(id, p);

			return Ok(StatusResponse.Success);
		}
	}
}
