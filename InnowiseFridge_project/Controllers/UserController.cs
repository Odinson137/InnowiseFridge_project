using InnowiseFridge_project.Data;
using InnowiseFridge_project.DTO;
using InnowiseFridge_project.Interfaces.ServiceInterfaces;
using InnowiseFridge_project.Models;
using InnowiseFridge_project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using static Newtonsoft.Json.JsonConvert;
using IUser = InnowiseFridge_project.Interfaces.RepositoryInterfaces.IUser;

namespace InnowiseFridge_project.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUser _user;
    private readonly ITokenService _tokenService;

    public UserController(ILogger<UserController> logger, IUser user, ITokenService tokenService)
    {
        _logger = logger;
        _user = user;
        _tokenService = tokenService;
    }

    [AllowAnonymous]
    [HttpPost("Registration")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<int>> Registration(UserDto userDto)
    {
        _logger.LogInformation("Registration");

        if (await _user.UserNameCheckTakenAsync(userDto.UserName))
        {
            return BadRequest("UserName has already taken");
        }

        var user = new User()
        {
            UserName = userDto.UserName,
            Password = userDto.Password,
            Role = Role.Authorized,
        };

        await _user.AddUserAsync(user);

        await _user.SaveAsync();

        return Ok(user.Id);
    }
    
    [AllowAnonymous]
    [HttpPost("Authorization")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<AuthorizeUser>> Authorization(UserDto userDto)
    {
        _logger.LogInformation("Authorization");

        var user = await _user.GetUserByName(userDto.UserName);

        if (user == null)
        {
            return NotFound("User not found");
        }

        if (user.Password != userDto.Password)
        {
            return NotFound("Passwords are not match");
        }

        var token = _tokenService.GenerateJwtToken(user.Id, user.UserName, user.Role);
        
        return Ok(new AuthorizeUser{ UserId = user.Id, Token = token });
    }
}