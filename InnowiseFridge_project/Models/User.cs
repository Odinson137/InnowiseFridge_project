using System.ComponentModel.DataAnnotations;
using InnowiseFridge_project.Data;

namespace InnowiseFridge_project.Models;

public class User
{
    [Key] 
    [MaxLength(50)] 
    public string Id { get; set; } = Guid.NewGuid().ToString();
    [MinLength(1)] 
    [MaxLength(50)] 
    public string UserName { get; set; } = null!;
    [MinLength(8)] 
    [MaxLength(50)] 
    public string Password { get; set; } = null!;
    public Role Role { get; set; }
}