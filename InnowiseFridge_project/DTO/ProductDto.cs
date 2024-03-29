using System.ComponentModel.DataAnnotations;

namespace InnowiseFridge_project.DTO;

public class ProductDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int DefaultQuantity { get; set; }
    public string? ImageUrl { get; set; }
}

public class PatchProductDto
{
    [Required] [MaxLength(50)] public string Id { get; set; } = null!;
    [MaxLength(50)]
    public string? Name { get; set; }
    public int? Quantity { get; set; }
    public string? ImageUrl { get; set; }
}