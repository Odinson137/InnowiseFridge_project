namespace InnowiseFridgeClient.ViewModels;

public class ProductViewModel
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int DefaultQuantity { get; set; }
    public string? ImageUrl { get; set; }
}