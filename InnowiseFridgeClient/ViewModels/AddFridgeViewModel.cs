using System.ComponentModel.DataAnnotations;

namespace InnowiseFridgeClient.ViewModels;

public class AddFridgeViewModel
{
    public string OwnerName { get; set; }
    [Required]
    public string Name { get; set; }
}