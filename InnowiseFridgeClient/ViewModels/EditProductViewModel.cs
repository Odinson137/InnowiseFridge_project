using System.ComponentModel.DataAnnotations;

namespace InnowiseFridgeClient.ViewModels;

public class EditProductViewModel
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    [Range(0, int.MaxValue)]
    public int DefaultQuantity { get; set; }
    public IFormFile? Image { get; set; }
}