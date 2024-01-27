using System.ComponentModel.DataAnnotations;

namespace InnowiseFridge_project.DTO;

public class UserDto
{
    [MinLength(1)] 
    [MaxLength(50)] 
    public string UserName { get; set; } = null!;
    [MinLength(8)] 
    [MaxLength(50)] 
    public string Password { get; set; } = null!;
}

public class AuthorizeUser
{
    public required string UserId { get; set; }
    public required string Token { get; set; }
}