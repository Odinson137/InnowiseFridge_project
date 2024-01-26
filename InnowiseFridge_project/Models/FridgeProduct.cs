using System.ComponentModel.DataAnnotations;

namespace InnowiseFridge_project.Models;

public class FridgeProduct
{
    public string ProductId { get; set; } = null!;
    public Product Product { get; set; } = null!;
    public string FridgeId { get; set; } = null!;
    public Fridge Fridge { get; set; } = null!;
    public int? Quantity { get; set; }
}