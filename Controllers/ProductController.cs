using Microsoft.AspNetCore.Mvc;
using MarketplaceAPI.Data;
using MarketplaceAPI.Models;
using System;
using System.Linq;
using MarketplaceAPI.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MarketplaceAPI.Models.NewFolder;

namespace MarketplaceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductController(DataContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }




        // GET: api/Product
        [HttpGet, Authorize]
        public IActionResult GetProducts()
        {
            var products = _context.Products.ToList();
            return Ok(products);
        }


        [HttpGet("/user/{id}")]
        public async Task<ActionResult<ApplicationUser>> GetUserById(string id)
        {
            // Retrieve the user from the Identity context
            var user = await _userManager.FindByIdAsync(id);

            // Check if user exists
            if (user == null)
            {
                return NotFound(); // Return 404 Not Found if user is not found
            }

            return Ok(user); // Return the user if found
        }


        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Seller)
                .FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // POST: api/Product

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductInput productInput)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Retrieve the seller from the Identity context
            var seller = await _userManager.FindByIdAsync(productInput.SellerId.ToString());

            // Check if seller exists
            if (seller == null)
            {
                return BadRequest("Invalid seller ID.");
            }

            // Retrieve the category from the base context
            var category = await _context.Categories.FindAsync(productInput.CategoryId);
            if (category == null)
            {
                return BadRequest("Invalid category ID.");
            }

            // Map ProductInput to Product entity
            var product = new Product
            {
                Name = productInput.Name,
                Description = productInput.Description,
                Price = productInput.Price,
                SellerId = productInput.SellerId,
                CategoryId = productInput.CategoryId
            };

            // Add the product to the context and save changes
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Map Product entity to ProductOutput DTO if needed
            // var productOutput = new ProductOutput { ... };

            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
        }





        // PUT: api/Product/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                if (!_context.Products.Any(p => p.ProductId == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Product/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
