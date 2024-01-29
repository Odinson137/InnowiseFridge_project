using InnowiseFridge_project.Controllers;
using InnowiseFridge_project.DTO;
using InnowiseFridge_project.Interfaces.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace InnowiseFridgeTests;

public class FridgeControllerTests
{
    private readonly FridgeController _controller;
    private readonly Mock<IFridge> _fridgeMock;
    private readonly Mock<ILogger<FridgeController>> _loggerMock;

    public FridgeControllerTests()
    {
        _fridgeMock = new Mock<IFridge>();
        _loggerMock = new Mock<ILogger<FridgeController>>();

        _controller = new FridgeController(_loggerMock.Object, _fridgeMock.Object);
    }
    
    [Fact]
    public async Task GetFridges_Successful_ReturnsOkWithEmptyList()
    {
        // Arrange
        var fridgeList = new List<FridgeDto>();
        _fridgeMock.Setup(f => f.GetFridgesAsync()).ReturnsAsync(fridgeList);

        // Act
        var result = await _controller.GetFridges();

        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
        var responseData = Assert.IsType<List<FridgeDto>>(okObjectResult.Value);
        Assert.Empty(responseData);
    }
    
    [Fact]
    public async Task GetFridges_Successful_ReturnsOkWithFridgeList()
    {
        // Arrange
        var fridgeList = new List<FridgeDto>
        {
            new() { Id = "1", Name = "Fridge1" },
            new() { Id = "2", Name = "Fridge2" },
            // Add more fridges as needed
        };

        _fridgeMock.Setup(f => f.GetFridgesAsync()).ReturnsAsync(fridgeList);

        // Act
        var result = await _controller.GetFridges();

        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
        var responseData = Assert.IsType<List<FridgeDto>>(okObjectResult.Value);

        Assert.Equal(fridgeList.Count, responseData.Count);
        Assert.Equal(fridgeList[0].Id, responseData[0].Id);
        Assert.Equal(fridgeList[0].Name, responseData[0].Name);
    }

}