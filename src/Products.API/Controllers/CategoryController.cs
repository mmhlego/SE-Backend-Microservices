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
			var subcategories = _categoryService.GetSubcategories(default(Guid));
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
		[Route("/categories")]
		[Authorize(Roles = "Admin , StoreKeeper")]
		public ActionResult<StatusResponse> AddCategory([FromBody] CategoryRequests categoryRequest)
		{
			if (categoryRequest == null)
			{
				return BadRequest(StatusResponse.Failed("اطلاعات دسته بندی را وارد نکردید."));
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

			[HttpPut]
			[Route("/subcategories/{id}")]
			[Authorize(Roles = "Admin , StoreKeeper , Seller")]
			public ActionResult<StatusResponse> UpdateSubcategory(Guid id, [FromBody] SubcategoryRequests subcategoryRequests)
			{
				var subcategory = _categoryService.GetSubcategoryById(id);
				[HttpPut]
				[Route("subcategories/{id}")]
				[Authorize(Roles = "Admin , StoreKeeper , Seller")]
				public ActionResult<StatusResponse> UpdateSubcategory(Guid id, [FromBody] SubcategoryRequests subcategoryRequests)
				{
					var subcategory = _categoryService.GetSubcategoryById(id);

					if (subcategory == null)
						return NotFound(StatusResponse.Failed("زیردسته مورد نظر پیدا نشد."));
					subcategory.Title = subcategoryRequests.Title;
					subcategory.CategoryId = subcategoryRequests.CategoryId;
					_categoryService.UpdateSubcategory(subcategory);
					return Ok(StatusResponse.Success);
				}

				[HttpPut]
				[Route("/subcategories/{id}/fields")]
				[Authorize(Roles = "Admin , StoreKeeper , Seller")]
				public ActionResult<StatusResponse> UpdateSubcategoryFields(Guid id, [FromBody] List<Field> fields)
				{
					if (id == default(Guid))
						return BadRequest(StatusResponse.Failed(" اطلاعات زیردسته را وارد نکردید."));
					[HttpPut]
					[Route("subcategories/{id}/fields")]
					[Authorize(Roles = "Admin , StoreKeeper , Seller")]
					public ActionResult<StatusResponse> UpdateSubcategoryFields(Guid id, [FromBody] List<Field> fields)
					{
						if (id == default(Guid))
							return BadRequest(StatusResponse.Failed(" اطلاعات زیردسته را وارد نکردید."));

						if (_categoryService.GetSubcategoryById(id) == null)
							return NotFound(StatusResponse.Failed("زیردسته مورد نظر پیدا نشد."));

						foreach (Field f in fields)
						{
							if (f.SubcategoryId != id)
								return BadRequest(StatusResponse.Failed("خطایی رخ داده."));
							if (_categoryService.GetSubcategoryById(id) == null)
								return NotFound(StatusResponse.Failed("زیردسته مورد نظر پیدا نشد."));

							foreach (Field f in fields)
							{
								if (_categoryService.GetFieldById(f.Id) == null)
									return BadRequest(StatusResponse.Failed("فیلد مورد نظر وجود ندارد."));
								if (f.SubcategoryId != id)
									return BadRequest(StatusResponse.Failed("خطایی رخ داده."));

								_categoryService.UpdateField(f.Id, f.Title);
							}

							return Ok(StatusResponse.Success);
						}

						[HttpPost]
						[Route("/subcategories/{id}/fields")]
						[Authorize(Roles = "Admin , StoreKeeper , Seller")]
						public ActionResult<StatusResponse> AddSubcategoryFields(Guid id, string title)
						{
							if (id == default(Guid))
								return BadRequest(StatusResponse.Failed(" اطلاعات دسته بندی را وارد نکردید."));
							[HttpPost]
							[Route("subcategories/{id}/fields")]
							[Authorize(Roles = "Admin , StoreKeeper , Seller")]
							public ActionResult<StatusResponse> AddSubcategoryFields(Guid id, string title)
							{
								if (id == default(Guid))
									return BadRequest(StatusResponse.Failed(" اطلاعات دسته بندی را وارد نکردید."));

								if (_categoryService.GetSubcategoryById(id) == null)
									return NotFound(StatusResponse.Failed("دسته بندی مورد نظر پیدا نشد."));

								_categoryService.AddField(id, title);
								return Ok(StatusResponse.Success);
							}
						}
					}
