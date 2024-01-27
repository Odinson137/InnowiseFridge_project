using InnowiseFridge_project.Controllers;
using InnowiseFridge_project.Data;
using InnowiseFridge_project.DTO;
using InnowiseFridge_project.Interfaces.RepositoryInterfaces;
using InnowiseFridge_project.Interfaces.ServiceInterfaces;
using InnowiseFridge_project.Models;
using InnowiseFridge_project.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace InnowiseFridgeTests;

public class UserControllerTests
{
    private readonly UserController _controller;
    private readonly Mock<IUser> _userMock;
    private readonly Mock<ILogger<UserController>> _loggerMock;
    private readonly Mock<ITokenService> _tokenServiceMock;

    public UserControllerTests()
    {
        _userMock = new Mock<IUser>();
        _loggerMock = new Mock<ILogger<UserController>>();
        _tokenServiceMock = new Mock<ITokenService>();

        _controller = new UserController(_loggerMock.Object, _userMock.Object, _tokenServiceMock.Object);
    }
    
    [Fact]
    public async Task Registration_Successful()
    {
        // Arrange
        var userDto = new UserDto { UserName = "TestUser", Password = "TestPassword123" };

        // Act
        var result = await _controller.Registration(userDto);

        // Assert
        Assert.True(result.Result is OkObjectResult);
    }

    [Fact]
    public async Task Registration_UserNameAlreadyTaken()
    {
        // Arrange
        var existingUserName = "ExistingUser";
        var userDto = new UserDto { UserName = existingUserName, Password = "TestPassword123" };

        _userMock.Setup(u => u.UserNameCheckTakenAsync(existingUserName)).ReturnsAsync(true);

        // Act
        var result = await _controller.Registration(userDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("UserName has already taken", badRequestResult.Value);
    }


    [Fact]
    public async Task Authorization_UserNotFound_ReturnsNotFound()
    {
        // Arrange
        var userDto = new UserDto { UserName = "NonExistentUser", Password = "TestPassword123" };

        _userMock.Setup(u => u.GetUserByName(userDto.UserName)).ReturnsAsync((User)null);

        // Act
        var result = await _controller.Authorization(userDto);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task Authorization_PasswordsNotMatch_ReturnsNotFound()
    {
        // Arrange
        var userDto = new UserDto { UserName = "ExistingUser", Password = "IncorrectPassword" };
        var existingUser = new User { UserName = "ExistingUser", Password = "CorrectPassword", Role = Role.Authorized };

        // Setup mock or actual behavior for the GetUserByName method
        _userMock.Setup(u => u.GetUserByName(userDto.UserName)).ReturnsAsync(existingUser);

        // Act
        var result = await _controller.Authorization(userDto);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);

        // Additional assertions based on your application's logic
    }

    [Fact]
    public async Task Authorization_Successful_ReturnsOkWithToken()
    {
        // Arrange
        var userDto = new UserDto { UserName = "ExistingUser", Password = "CorrectPassword" };
        var existingUser = new User { Id = "1", UserName = "ExistingUser", Password = "CorrectPassword", Role = Role.Authorized };
        var expectedToken = "GeneratedJwtToken";

        // Setup mock or actual behavior for the GetUserByName method
        _userMock.Setup(u => u.GetUserByName(userDto.UserName)).ReturnsAsync(existingUser);

        // Setup mock or actual behavior for the GenerateJwtToken method
        _tokenServiceMock.Setup(ts => ts.GenerateJwtToken(existingUser.Id, existingUser.UserName, existingUser.Role)).Returns(expectedToken);

        // Act
        var result = await _controller.Authorization(userDto);

        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
        var responseData = Assert.IsType<AuthorizeUser>(okObjectResult.Value);

        Assert.Equal(existingUser.Id, responseData.UserId);
        Assert.Equal(expectedToken, responseData.Token);
    }
}