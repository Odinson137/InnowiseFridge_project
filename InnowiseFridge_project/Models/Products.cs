using System.ComponentModel.DataAnnotations;

namespace InnowiseFridge_project.Models;

public class Products
{
    [Key] public string Id { get; set; } = new Guid().ToString();
    [Required]
    public string Name { get; set; } = null!;
    public int? DefaultQuantity { get; set; }
}