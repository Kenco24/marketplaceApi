using System.ComponentModel.DataAnnotations;

namespace MarketplaceAPI.Models.Base
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; } // Should be hashed and salted for security

        [Required]
        public string Email { get; set; }

        // Add other user-related properties as needed
    }
}
