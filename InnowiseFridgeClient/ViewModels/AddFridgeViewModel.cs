using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InnowiseFridgeClient.ViewModels;

public class AddFridgeViewModel
{
    public string OwnerName { get; set; } = null!;
    [Required] public string Name { get; set; } = null!;
    [MaxLength(500)] public string? Description { get; set; }
    [MaxLength(50)] public string FridgeModelName { get; set; } = null!;
    public List<FridgeStartProductsViewModel>? Products { get; set; }
}

public class FridgeStartProductsViewModel
{
    public string ProductId { get; set; } = null!;
    [JsonIgnore]
    public string Name { get; set; } = null!;
    [Range(0, int.MaxValue)]
    public int Count { get; set; } = 0;
    [JsonIgnore]
    public int Num { get; set; }
}

