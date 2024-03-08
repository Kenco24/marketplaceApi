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
    public int SellerId { get; set; } // Corrected property name

    // Foreign key
    [Required]
    public int CategoryId { get; set; }

    // Navigation properties
    public Category Category { get; set; }
    public User Seller { get; set; }
}


