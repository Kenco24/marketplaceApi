using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MarketplaceAPI.Models.Base;

public class Product
{
    [Key]
    public int ProductId { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Required]
    [ForeignKey("Seller")]
    public int SellerId { get; set; } // Change the data type to string

    public ApplicationUser Seller { get; set; } // Update to ApplicationUser

    // Foreign key
    [Required]
    [ForeignKey("Category")]
    public int CategoryId { get; set; }

    // Navigation properties
    public Category Category { get; set; }
   
}


