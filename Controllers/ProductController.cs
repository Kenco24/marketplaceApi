using Microsoft.AspNetCore.Mvc;
using MarketplaceAPI.Data;
using MarketplaceAPI.Models;
using System;
using System.Linq;
using MarketplaceAPI.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace MarketplaceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Product
        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = _context.Products.ToList();
            return Ok(products);
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
        // POST: api/Product
        // POST: api/Product
        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Retrieve the seller and category from the database
            var seller = _context.Users.Find(product.SellerId);
            var category = _context.Categories.Find(product.CategoryId);

            // Check if seller and category exist
            if (seller == null)
            {
                return BadRequest("Invalid seller ID.");
            }
            if (category == null)
            {
                return BadRequest("Invalid category ID.");
            }

            // Assign the seller and category to the product
            product.Seller = seller;
            product.Category = category;

            // Add the product to the context and save changes
            _context.Products.Add(product);
            _context.SaveChanges();

            // Reload the product with related entities populated
            var createdProduct = _context.Products
                .Include(p => p.Seller)
                .Include(p => p.Category)
                .FirstOrDefault(p => p.ProductId == product.ProductId);

            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.ProductId }, createdProduct);
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
