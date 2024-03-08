using System.ComponentModel.DataAnnotations;

namespace MarketplaceAPI.Models.Auth
{
    public class LoginModel
    {
        [Required]
        public string UsernameOrEmail { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
