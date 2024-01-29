namespace InnowiseFridge_project.DTO;

public class FridgeProductDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int Quantity { get; set; }
    public string? ImageUrl { get; set; }
}