namespace InnowiseFridge_project.DTO;

public class FridgeDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = null!;
    public string? OwnerName { get; set; }
}