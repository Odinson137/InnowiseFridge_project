using InnowiseFridge_project.DTO;
using InnowiseFridge_project.Interfaces.RepositoryInterfaces;
using InnowiseFridge_project.Interfaces.ServiceInterfaces;
using InnowiseFridge_project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnowiseFridge_project.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly IProduct _product;
    private readonly IFileService _fileService;

    public ProductController(ILogger<ProductController> logger, IProduct product, IFileService fileService)
    {
        _logger = logger;
        _product = product;
        _fileService = fileService;
    }
    
    [Authorize("Admin")]
    [HttpPatch]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> PatchProductAsync(string productId, [FromBody] PatchProductDto productDto)
    {
        _logger.LogInformation("Patch product in db");
        
        var product = await _product.GetProductAsync(productId);
        if (product == null)
        {
            return NotFound("The product not found");
        }

        if (productDto.Name != null)
        {
            product.Name = productDto.Name;
        }

        if (productDto.Quantity != null)
        {
            if (productDto.Quantity <= 0)
            {
                return BadRequest("The quantity is not correct");
            }

            product.DefaultQuantity = productDto.Quantity;
        }

        if (productDto.Image != null)
        {
            string nameFile = _fileService.GetUniqueName(productDto.Image);
            await _fileService.CreateFileAsync(productDto.Image, nameFile);
            product.ImageUrl = nameFile;
        }

        await _product.SaveAsync();
        
        return Ok();
    }

}