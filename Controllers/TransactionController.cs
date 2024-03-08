using Microsoft.AspNetCore.Mvc;

using MarketplaceAPI.Data;
using MarketplaceAPI.Models.Base;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MarketplaceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly DataContext _context;

        public TransactionController(DataContext context)
        {
            _context = context;
        }

        // POST: api/Transaction
        [HttpPost]
        public IActionResult CreateTransaction(Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Retrieve the buyer, seller, and product from the database based on their IDs
            var buyer = _context.Users.Find(transaction.BuyerId);
            var seller = _context.Users.Find(transaction.SellerId);
            var product = _context.Products.Find(transaction.ProductId);

            // Check if buyer, seller, and product exist
            if (buyer == null)
            {
                return BadRequest("Invalid buyer ID.");
            }
            if (seller == null)
            {
                return BadRequest("Invalid seller ID.");
            }
            if (product == null)
            {
                return BadRequest("Invalid product ID.");
            }

            // Assign the retrieved buyer, seller, and product objects to the transaction
            transaction.Buyer = buyer;
            transaction.Seller = seller;
            transaction.Product = product;

            // Add the transaction to the context and save changes
            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.TransactionId }, transaction);
        }

        // GET: api/Transaction/{id}
        [HttpGet("{id}")]
        public IActionResult GetTransaction(int id)
        {
            var transaction = _context.Transactions
                .Include(t => t.Buyer)
                .Include(t => t.Seller)
                .Include(t => t.Product)
                    .ThenInclude(p => p.Category)
                .FirstOrDefault(t => t.TransactionId == id);

            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

    }
}
