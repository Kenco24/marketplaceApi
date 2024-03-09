using Microsoft.AspNetCore.Mvc;
using MarketplaceAPI.Data;
using MarketplaceAPI.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace MarketplaceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TransactionController(DataContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // POST: api/Transaction
        [HttpPost]
        public async Task<IActionResult> CreateTransaction(Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Retrieve the buyer, seller, and product from the database based on their IDs
            var buyer = await _userManager.FindByIdAsync(transaction.BuyerId.ToString());
            var seller = await _userManager.FindByIdAsync(transaction.SellerId.ToString());
            var product = await _context.Products.FindAsync(transaction.ProductId);

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
            await _context.SaveChangesAsync();

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
