using System.ComponentModel.DataAnnotations;

namespace InnowiseFridgeClient.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "User name is required")]
    [Display(Name = "User name")]
    public string UserName { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}