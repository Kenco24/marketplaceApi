using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MarketplaceAPI.Models.NewFolder
{
    public class ProductInput
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
        public int SellerId { get; set; } // Change the data type to string

        // Foreign key
        [Required]
        public int CategoryId { get; set; }
    }
}
