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

    public ProductController(ILogger<ProductController> logger, IProduct product)
    {
        _logger = logger;
        _product = product;
    }
    
    [Authorize]
    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<ActionResult> GetProductsAsync()
    {
        _logger.LogInformation("Get products from db");
        var products = await _product.GetProductsAsync();
        return Ok(products);
    }
    
    [Authorize]
    [HttpGet("{productId}")]
    [ProducesResponseType(200)]
    public async Task<ActionResult> GetProductAsync(string productId)
    {
        _logger.LogInformation("Get product from db");
        var product = await _product.GetProductAsync(productId);
        return Ok(product);
    }
    
    [Authorize("Admin")]
    [HttpPatch]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> PatchProductAsync([FromBody] PatchProductDto productDto)
    {
        _logger.LogInformation("Patch product in db");
        
        var product = await _product.GetProductAsync(productDto.Id);
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

        if (productDto.ImageUrl != null)
        {
            product.ImageUrl = productDto.ImageUrl;
        }
        
        await _product.SaveAsync();
        
        return Ok();
    }

}