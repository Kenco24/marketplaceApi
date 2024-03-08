using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MarketplaceAPI.Models.Base
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        [Required]
        public int BuyerId { get; set; }


        public int SellerId { get; set; }

        // Remove this property as it's causing the multiple cascade path issue
        // public User SellerId { get; set; }

        // Foreign key
        [Required]
        public int ProductId { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        // Navigation properties
        public User Buyer { get; set; }

        // Add this property to reference Seller
        public User Seller { get; set; }

        public Product Product { get; set; }
    }
}
