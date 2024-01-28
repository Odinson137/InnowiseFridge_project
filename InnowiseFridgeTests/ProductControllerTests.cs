using InnowiseFridge_project.Controllers;
using InnowiseFridge_project.DTO;
using InnowiseFridge_project.Interfaces.RepositoryInterfaces;
using InnowiseFridge_project.Interfaces.ServiceInterfaces;
using InnowiseFridge_project.Models;
using InnowiseFridge_project.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace InnowiseFridgeTests;

public class ProductControllerTests
{
    private readonly ProductController _controller;
    private readonly Mock<IProduct> _productMock;
    private readonly Mock<ILogger<ProductController>> _loggerMock;
    private readonly Mock<IFileService> _fileServiceMock;

    public ProductControllerTests()
    {
        _productMock = new Mock<IProduct>();
        _loggerMock = new Mock<ILogger<ProductController>>();
        _fileServiceMock = new Mock<IFileService>();

        _controller = new ProductController(_loggerMock.Object, _productMock.Object, _fileServiceMock.Object);
    }
    
    [Fact]
    public async Task PatchProductAsync_SuccessfulUpdate_ReturnsOk()
    {
        // Arrange
        var productId = "existingProductId";
        var patchProductDto = new PatchProductDto { Name = "NewName", Quantity = 10, Image = null };
        var existingProduct = new Product { Id = productId, Name = "OldName", DefaultQuantity = 5, ImageUrl = "OldImage" };

        _productMock.Setup(p => p.GetProductAsync(productId)).ReturnsAsync(existingProduct);

        // Act
        var result = await _controller.PatchProductAsync(productId, patchProductDto);

        // Assert
        Assert.IsType<OkResult>(result);
    }
    
    [Fact]
    public async Task PatchProductAsync_ProductNotFound_ReturnsNotFound()
    {
        // Arrange
        var nonExistingProductId = "nonExistingProductId";
        var patchProductDto = new PatchProductDto { Name = "NewName", Quantity = 10, Image = null };

        _productMock.Setup(p => p.GetProductAsync(nonExistingProductId)).ReturnsAsync((Product?)null);

        // Act
        var result = await _controller.PatchProductAsync(nonExistingProductId, patchProductDto);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);

        // Additional assertions based on your application's logic
    }
    
    [Fact]
    public async Task PatchProductAsync_InvalidQuantity_ReturnsBadRequest()
    {
        // Arrange
        var productId = "existingProductId";
        var patchProductDto = new PatchProductDto { Quantity = -1 };
        var existingProduct = new Product { Id = productId, Name = "OldName", DefaultQuantity = 5, ImageUrl = "OldImage" };

        _productMock.Setup(p => p.GetProductAsync(productId)).ReturnsAsync(existingProduct);

        // Act
        var result = await _controller.PatchProductAsync(productId, patchProductDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task PatchProductAsync_UpdateName_Successful()
    {
        // Arrange
        var productId = "existingProductId";
        var newProductName = "NewName";
        var patchProductDto = new PatchProductDto { Name = newProductName };
        var existingProduct = new Product { Id = productId, Name = "OldName", DefaultQuantity = 5, ImageUrl = "OldImage" };

        _productMock.Setup(p => p.GetProductAsync(productId)).ReturnsAsync(existingProduct);

        // Act
        await _controller.PatchProductAsync(productId, patchProductDto);

        // Assert
        Assert.Equal(newProductName, existingProduct.Name);
    }

    [Fact]
    public async Task PatchProductAsync_UpdateQuantity_Successful()
    {
        // Arrange
        var productId = "existingProductId";
        var newQuantity = 10;
        var patchProductDto = new PatchProductDto { Quantity = newQuantity };
        var existingProduct = new Product { Id = productId, Name = "OldName", DefaultQuantity = 5, ImageUrl = "OldImage" };

        _productMock.Setup(p => p.GetProductAsync(productId)).ReturnsAsync(existingProduct);

        // Act
        await _controller.PatchProductAsync(productId, patchProductDto);

        // Assert
        Assert.Equal(newQuantity, existingProduct.DefaultQuantity);
    }
    
    [Fact]
    public async Task PatchProductAsync_UpdateImage_Successful()
    {
        // Arrange
        var productId = "existingProductId";
        var newImage = "NewImage";
        var imageMock = new Mock<IFormFile>();
        var patchProductDto = new PatchProductDto { Image = imageMock.Object };
        var existingProduct = new Product { Id = productId, Name = "OldName", DefaultQuantity = 5, ImageUrl = "OldImage" };

        _fileServiceMock.Setup(p => p.GetUniqueName(It.IsAny<IFormFile>())).Returns(newImage);
        _fileServiceMock.Setup(p => p.CreateFileAsync(It.IsAny<IFormFile>(), newImage));
        _productMock.Setup(p => p.GetProductAsync(productId)).ReturnsAsync(existingProduct);

        // Act
        await _controller.PatchProductAsync(productId, patchProductDto);

        // Assert
        Assert.Equal(newImage, existingProduct.ImageUrl);
    }


}