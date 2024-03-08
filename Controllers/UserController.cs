using Microsoft.AspNetCore.Mvc;
using MarketplaceAPI.Models.Base;
using MarketplaceAPI.Models.Auth;

using MarketplaceAPI.Data;
using Microsoft.EntityFrameworkCore;
using MarketplaceAPI.Models.Auth;
using MarketplaceAPI.Models.Base;
using System.Text;
using System.Security.Cryptography;

namespace MarketplaceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        // POST: api/user/register
        [HttpPost("register")]
        public IActionResult Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the email is already registered
            if (_context.Users.Any(u => u.Email == model.Email))
            {
                return BadRequest("Email is already registered.");
            }

            // Check if the username is already registered
            if (_context.Users.Any(u => u.Username == model.Username))
            {
                return BadRequest("Username is already taken.");
            }

            // Hash the password
            string hashedPassword = HashPassword(model.Password);

            // For demonstration purposes, let's assume we're adding the user directly to the database
            var newUser = new User
            {
                Username = model.Username,
                Email = model.Email,
                Password = hashedPassword // Save the hashed password
            };
            _context.Users.Add(newUser);
            _context.SaveChanges();

            return Ok("User registered successfully.");
        }

        // Hashing the password using bcrypt
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }


        // POST: api/user/login
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(LoginModel model)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u =>
                (u.Username == model.UsernameOrEmail || u.Email == model.UsernameOrEmail) &&
                u.Password == model.Password);

            if (existingUser == null)
            {
                return NotFound("Invalid username/email or password");
            }

            return Ok(existingUser);
        }


        // GET: api/user/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPassword(string password)
        {
            return !string.IsNullOrEmpty(password) && password.Length >= 8 && password.Distinct().Count() > 1;
        }
    }
}
