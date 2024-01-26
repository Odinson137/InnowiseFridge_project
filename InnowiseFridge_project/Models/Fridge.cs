using System.ComponentModel.DataAnnotations;

namespace InnowiseFridge_project.Models;

public class Fridge
{
    [Key] 
    public string Id { get; set; } = new Guid().ToString();
    [Required]
    public string Name { get; set; } = null!;
    public string FridgeModelId { get; set; } = null!;
    public FridgeModel FridgeModel { get; set; } = null!;
    public string? OwnerName { get; set; }
}