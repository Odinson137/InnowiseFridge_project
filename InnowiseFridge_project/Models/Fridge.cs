using System.ComponentModel.DataAnnotations;

namespace InnowiseFridge_project.Models;

public class Fridge
{
    [Key] 
    [MaxLength(50)]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    [MaxLength(50)]
    public string FridgeModelId { get; set; } = null!;
    public FridgeModel FridgeModel { get; set; } = null!;
    [MaxLength(50)]
    public string? OwnerName { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}