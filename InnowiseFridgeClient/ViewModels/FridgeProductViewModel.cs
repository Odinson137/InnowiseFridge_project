namespace InnowiseFridgeClient.ViewModels;

public class FridgeProductViewModel
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int Quantity { get; set; }
    public string? ImageUrl { get; set; }
}