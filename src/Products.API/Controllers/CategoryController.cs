using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Products.API.Data;
using Products.API.Models;

namespace Products.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ProductsContext _context;

        public CategoryController(ProductsContext context)
        {
            _context = context;
        }
        //[HttpPut]
        //[Route("/test")]
        //public OkObjectResult UpdateSubcategory(Subcategory subcategory)
        //{
        //    if (subcategory == null)
        //        throw new ArgumentNullException("Subcategory is required to update.");
        //    var sub = _context.Subcategories.SingleOrDefault(c => c.Id == subcategory.Id);
        //    if (sub == null)
        //        throw new ArgumentNullException("SubcategoryId is wrong.");
        //    var catcheck = _context.Categories.SingleOrDefault(c => c.Id == subcategory.CategoryId);
        //    if (catcheck == null)
        //        throw new ArgumentNullException("CategoryId is wrong.");
        //    var category = _context.Categories.SingleOrDefault(c => c.Id == subcategory.CategoryId);
        //    if (category == null)
        //        throw new ArgumentNullException("Category Is Deleted.");
           
        //    sub.CategoryId = subcategory.Id;
        //    sub.Title = subcategory.Title;
        //    _context.Update(sub);
        //    _context.SaveChanges();}
        
    }
    }
