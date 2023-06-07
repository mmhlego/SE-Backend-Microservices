using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.API.Models;
using Products.API.Models.Requests;
using Products.API.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace YourNamespace.Controllers
{
   
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Route("/categories")]
        public ActionResult<List<Category>> GetCategories()
        {
            var categories = _categoryService.GetCategories();
            return Ok(categories);
        }

        [HttpGet]
        [Route("/subcategories")]
        public ActionResult<List<Subcategory>> GetSubcategories()
        {
            var subcategories =_categoryService.GetSubcategories(default(Guid));
            return Ok();
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
        public IActionResult AddCategory([FromBody] CategoryRequests categoryRequest)
        {   if(categoryRequest == null)
            {
                return BadRequest();
            }
            _categoryService.AddCategory(categoryRequest.Title, categoryRequest.IconName);
            return Ok();
        }

        [HttpPost]
        [Route("/subcategories")]
        [Authorize(Roles = "Admin , StoreKeeper , Seller")]
        public ActionResult AddSubcategory( [FromBody] SubcategoryRequests subcategoryRequests)
        {  if (_categoryService.GetCategoryById(subcategoryRequests.CategoryId) == null)
                return NotFound();
            _categoryService.AddSubcategory(subcategoryRequests.CategoryId, subcategoryRequests.Title);
            return Ok();
        }

        [HttpPut]
        [Route("/categories/{id}")]
        [Authorize(Roles = "Admin , StoreKeeper")]
        public ActionResult UpdateCategory(Guid id, [FromBody] CategoryRequests categoryRequests)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null)
                return NotFound();
            category.Title = categoryRequests.Title;
            category.IconName = categoryRequests.IconName;
            _categoryService.UpdateCategory(category);
            return Ok();
        }

     

        [HttpPut]
        [Route("/subcategories/{id}")]
        [Authorize(Roles = "Admin , StoreKeeper , Seller")]
        public ActionResult UpdateSubcategory(Guid id, [FromBody] SubcategoryRequests subcategoryRequests)
        {
            var subcategory = _categoryService.GetSubcategoryById(id);

            if (subcategory == null)
                return NotFound();
            subcategory.Title = subcategoryRequests.Title;
            subcategory.CategoryId = subcategoryRequests.CategoryId;
            _categoryService.UpdateSubcategory(subcategory);
            return Ok();
        }

        
        [HttpPut]
        [Route ("/subcategories/{id}/fields")]
        [Authorize(Roles = "Admin , StoreKeeper , Seller")]
        public ActionResult UpdateSubcategoryFields( Guid id, [FromBody] List<Field> fields)
        {   if (id == default(Guid))
                return BadRequest();
   
            if (_categoryService.GetSubcategoryById(id) == null)
                return NotFound();
           
            foreach (Field f in fields)
            {   if(f.SubcategoryId != id)
                    return BadRequest();
                
                _categoryService.UpdateField(f.Id, f.Title);
                
            }

            return Ok();
        }

        [HttpPost]
        [Route("/subcategories/{id}/fields")]
        [Authorize(Roles = "Admin , StoreKeeper , Seller")]
        public ActionResult AddSubcategoryFields(Guid id, string title)
        {
            if (id == default(Guid))
                return BadRequest();

            if (_categoryService.GetSubcategoryById(id) == null)
                return NotFound();

            _categoryService.AddField(id, title);
            return Ok();
        }
    }
}
