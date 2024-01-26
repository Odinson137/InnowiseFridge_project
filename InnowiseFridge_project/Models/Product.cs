using System.ComponentModel.DataAnnotations;

namespace InnowiseFridge_project.Models;

public class Product
{
    [MaxLength(50)]
    [Key] public string Id { get; set; } = Guid.NewGuid().ToString();
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    public int? DefaultQuantity { get; set; }
    public ICollection<Fridge> Fridges { get; set; } = new List<Fridge>();
}