using System.ComponentModel.DataAnnotations;

namespace MarketplaceAPI.Models.Base;
public class Category
{
    [Key]
    public int CategoryId { get; set; }

    [Required]
    public string Name { get; set; }

}