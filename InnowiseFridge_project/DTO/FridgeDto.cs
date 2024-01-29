using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace InnowiseFridge_project.DTO;

public class FridgeDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = null!;
    public string FridgeModelName { get; set; }
    public string? OwnerName { get; set; }
    public string? Description { get; set; }
}

public class AddFridgeDto
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;
    public string FridgeModelName { get; set; } = null!;
    [MaxLength(500)]
    public string? Description { get; set; }
    [Required] [MaxLength(50)] public string OwnerName { get; set; } = null!;
    public ICollection<FridgeStartProductDto>? Products { get; set; }
}

public class FridgeStartProductDto
{
    [MaxLength(50)] public string ProductId { get; set; } = null!;
    [Range(1, int.MaxValue)]
    public int Count { get; set; }
}

public class EditFridgeDto
{
    [Required] [MaxLength(50)] 
    public string Id { get; set; } = null!;
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;
    public string FridgeModelName { get; set; } = null!;
    [MaxLength(500)]
    public string? Description { get; set; }
    [Required] [MaxLength(50)] public string OwnerName { get; set; } = null!;
}

public class FridgeModelDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int Year { get; set; }
}