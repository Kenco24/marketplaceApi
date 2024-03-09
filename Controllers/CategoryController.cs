using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using MarketplaceAPI.Data;
using MarketplaceAPI.Models;
using MarketplaceAPI.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MarketplaceAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly DataContext _context;

        public CategoryController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Category
        [HttpGet]
        public ActionResult<IEnumerable<Category>> GetCategories()
        {
            return _context.Categories.ToList();
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public ActionResult<Category> GetCategory(int id)
        {
            var category = _context.Categories.Find(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        // POST: api/Category
        [HttpPost]
        public ActionResult<Category> PostCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId }, category);
        }

  

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return NoContent();
        }
        [Authorize]
        [HttpGet("currentUserId")]
        public IActionResult GetCurrentUserId()
        {
            // Retrieve the user's ID from the claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check if user ID exists
            if (userId == null)
            {
                return NotFound(); // Or return some appropriate error response
            }

            // Return the user's ID
            return Ok(userId);
        }



    }
}
