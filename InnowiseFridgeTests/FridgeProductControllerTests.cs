using System.Security.Claims;
using InnowiseFridge_project.Controllers;
using InnowiseFridge_project.DTO;
using InnowiseFridge_project.Interfaces.RepositoryInterfaces;
using InnowiseFridge_project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace InnowiseFridgeTests;

public class FridgeProductControllerTests
{
    private readonly FridgeProductController _controller;
    private readonly Mock<IFridgeProduct> _fridgeProductMock;
    private readonly Mock<ILogger<FridgeProductController>> _loggerMock;

    public FridgeProductControllerTests()
    {
        _fridgeProductMock = new Mock<IFridgeProduct>();
        _loggerMock = new Mock<ILogger<FridgeProductController>>();

        _controller = new FridgeProductController(_loggerMock.Object, _fridgeProductMock.Object);
    }
    
    [Fact]
    public async Task GetFridgeProducts_Successful_ReturnsOkWithProductList()
    {
        // Arrange
        var fridgeId = "existingFridgeId";
        var productDtoList = new List<ProductDto>
        {
            new ProductDto { Id = "1", Name = "Product1" },
            new ProductDto { Id = "2", Name = "Product2" },
        };

        _fridgeProductMock.Setup(fp => fp.FridgeExistAsync(fridgeId)).ReturnsAsync(true);
        _fridgeProductMock.Setup(fp => fp.GetFridgeProductAsync(fridgeId)).ReturnsAsync(productDtoList);

        // Act
        var result = await _controller.GetFridgeProducts(fridgeId);

        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
        var responseData = Assert.IsType<List<ProductDto>>(okObjectResult.Value);

        Assert.Equal(productDtoList.Count, responseData.Count);
        Assert.Equal(productDtoList[0].Id, responseData[0].Id);
        Assert.Equal(productDtoList[0].Name, responseData[0].Name);
    }
    
    [Fact]
    public async Task GetFridgeProducts_FridgeNotFound_ReturnsNotFound()
    {
        // Arrange
        var fridgeId = "nonExistingFridgeId";

        _fridgeProductMock.Setup(fp => fp.FridgeExistAsync(fridgeId)).ReturnsAsync(false);

        // Act
        var result = await _controller.GetFridgeProducts(fridgeId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task AddProductToFridge_FridgeNotFound_ReturnsNotFound()
    {
        // Arrange
        var fridgeId = "nonExistingFridgeId";
        var productId = "existingProductId";
        var quantity = 5;

        _fridgeProductMock.Setup(fp => fp.GetFridgeWithProductsAsync(fridgeId)).ReturnsAsync((Fridge)null);

        // Act
        var result = await _controller.AddProductToFridge(fridgeId, productId, quantity);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task AddProductToFridge_InvalidQuantity_ReturnsBadRequest()
    {
        // Arrange
        var fridgeId = "existingFridgeId";
        var productId = "existingProductId";
        var quantity = -1;

        // Act
        var result = await _controller.AddProductToFridge(fridgeId, productId, quantity);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Quantity must be at least one or higher", badRequestResult.Value);
    }

    
    [Fact]
    public async Task AddProductToFridge_Successful_ReturnsOk()
    {
        // Arrange
        var fridgeId = "existingFridgeId";
        var productId = "existingProductId";
        var quantity = 5;

        var userClaims = new[]
        {
            new Claim("ClaimType1", "ClaimValue1"),
            new Claim("ClaimType2", "TestUser"), 
        };

        var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(userClaims, "TestAuthentication"));

        _fridgeProductMock.Setup(fp => fp.GetFridgeWithProductsAsync(fridgeId))
            .ReturnsAsync(new Fridge { Id = fridgeId, OwnerName = "TestUser", Products = new List<Product>() });
        _fridgeProductMock.Setup(fp => fp.GetProductAsync(productId)).ReturnsAsync(new Product { Id = productId });
        _fridgeProductMock.Setup(fp => fp.AddFridgeProductAsync(It.IsAny<FridgeProduct>()));
        _fridgeProductMock.Setup(fp => fp.SaveAsync());

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = userPrincipal }
        };

        // Act
        var result = await _controller.AddProductToFridge(fridgeId, productId, quantity);

        // Assert
        Assert.IsType<OkResult>(result);
    }
    
    
    [Fact]
    public async Task AddProductToFridge_ProductNotFound_ReturnsNotFound()
    {
        // Arrange
        var fridgeId = "existingFridgeId";
        var productId = "existingProductId";
        var quantity = 5;

        var userClaims = new[]
        {
            new Claim("ClaimType1", "ClaimValue1"),
            new Claim("ClaimType2", "TestUser"), 
        };

        var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(userClaims, "TestAuthentication"));

        _fridgeProductMock.Setup(fp => fp.GetFridgeWithProductsAsync(fridgeId))
            .ReturnsAsync(new Fridge { Id = fridgeId, OwnerName = "TestUser", Products = new List<Product>() });
        _fridgeProductMock.Setup(fp => fp.GetProductAsync(productId)).ReturnsAsync((Product?)null);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = userPrincipal }
        };

        // Act
        var result = await _controller.AddProductToFridge(fridgeId, productId, quantity);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

        
    [Fact]
    public async Task AddProductToFridge_ProductAlreadyExists_ReturnsBadRequest()
    {
        // Arrange
        var fridgeId = "existingFridgeId";
        var productId = "existingProductId";
        var quantity = 5;

        var userClaims = new[]
        {
            new Claim("ClaimType1", "ClaimValue1"),
            new Claim("ClaimType2", "TestUser"), 
        };

        var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(userClaims, "TestAuthentication"));

        _fridgeProductMock.Setup(fp => fp.GetFridgeWithProductsAsync(fridgeId))
            .ReturnsAsync(new Fridge { Id = fridgeId, OwnerName = "TestUser", Products = new List<Product>()
            {
                new ()
                {
                    Id = productId,
                    Name = "Existed"
                }
            } 
        });

        _fridgeProductMock.Setup(fp => fp.GetProductAsync(productId))
            .ReturnsAsync(new Product { Id = productId, Name = "Existed" });
        _fridgeProductMock.Setup(fp => fp.AddFridgeProductAsync(It.IsAny<FridgeProduct>()));
        _fridgeProductMock.Setup(fp => fp.SaveAsync());

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = userPrincipal }
        };

        // Act
        var result = await _controller.AddProductToFridge(fridgeId, productId, quantity);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task AddProductToFridge_UserNotOwner_ReturnsUnauthorized()
    {
        // Arrange
        var fridgeId = "existingFridgeId";
        var productId = "existingProductId";
        var quantity = 5;

        var userClaims = new[]
        {
            new Claim("ClaimType1", "ClaimValue1"),
            new Claim("ClaimType2", "TestUser"), 
        };

        var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(userClaims, "TestAuthentication"));

        _fridgeProductMock.Setup(fp => fp.GetFridgeWithProductsAsync(fridgeId))
            .ReturnsAsync(new Fridge { Id = fridgeId, OwnerName = "NotTestUser", Products = new List<Product>() });
        _fridgeProductMock.Setup(fp => fp.GetProductAsync(productId)).ReturnsAsync((Product?)null);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = userPrincipal }
        };

        // Act
        var result = await _controller.AddProductToFridge(fridgeId, productId, quantity);

        // Assert
        Assert.IsType<UnauthorizedObjectResult>(result);
    }
}