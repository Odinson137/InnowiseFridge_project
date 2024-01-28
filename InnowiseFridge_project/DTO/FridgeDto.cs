using System.ComponentModel.DataAnnotations;

namespace InnowiseFridge_project.DTO;

public class FridgeDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = null!;
    public string FridgeModelName { get; set; }
    public string? OwnerName { get; set; }
}

public class AddFridgeDto
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;
    public string FridgeModelName { get; set; } = null!;
    [Required] [MaxLength(50)] public string OwnerName { get; set; } = null!;
}

public class FridgeModelDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int Year { get; set; }
}