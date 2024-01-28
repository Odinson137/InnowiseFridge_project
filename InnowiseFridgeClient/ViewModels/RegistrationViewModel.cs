using System.ComponentModel.DataAnnotations;

namespace InnowiseFridgeClient.ViewModels;

public class RegistrationViewModel
{
    [Required(ErrorMessage = "User name is required")]
    [Display(Name = "User name")]
    public string UserName { get; set; }
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Passsword")]
    public string Password { get; set; }
    [Display(Name = "Confirm passsword")]
    [Required(ErrorMessage = "Confirm password is required")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Password do not match")]
    public string ConfirmPassword { get; set; }
}