using InnowiseFridge_project.DTO;
using InnowiseFridge_project.Interfaces.RepositoryInterfaces;
using InnowiseFridge_project.Models;
using InnowiseFridge_project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnowiseFridge_project.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FridgeProductController : ControllerBase
{
    private readonly ILogger<FridgeProductController> _logger;
    private readonly IFridgeProduct _fridgeProduct;

    public FridgeProductController(ILogger<FridgeProductController> logger, IFridgeProduct fridgeProduct)
    {
        _logger = logger;
        _fridgeProduct = fridgeProduct;
    }
    
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<List<ProductDto>>> GetFridgeProducts(string fridgeId)
    {
        _logger.LogInformation("Get products by fridge id");
        
        var fridgeExist = await _fridgeProduct.FridgeExistAsync(fridgeId);
        if (!fridgeExist)
        {
            return NotFound("The fridge not found");
        }
        
        var fridges = await _fridgeProduct.GetFridgeProductAsync(fridgeId);
        return Ok(fridges);
    }
    
    [Authorize("FridgeOwner")]
    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> AddProductToFridge(string fridgeId, string productId, int quantity)
    {
        _logger.LogInformation("Add product to the fridge");

        if (quantity <= 0)
        {
            return BadRequest("Quantity must be at least one or higher");
        }
        
        var fridge = await _fridgeProduct.GetFridgeWithProductsAsync(fridgeId);
        
        if (fridge == null)
        {
            return NotFound("The fridge not found");
        }

        var userName = User.Identities.First().Claims.ToList()[1].Value;
        if (fridge.OwnerName != userName)
        {
            return Unauthorized("You are not the owner!");
        }

        var product = await _fridgeProduct.GetProductAsync(productId);

        if (product == null)
        {
            return NotFound("The product not found");
        }
        
        if (fridge.Products.Any(p => p.Id == product.Id))
        {
            return BadRequest("Product with this name already exists in the fridge");
        }

        var fridgeProduct = new FridgeProduct()
        {
            Fridge = fridge,
            Product = product,
            Quantity = quantity,
        };

        await _fridgeProduct.AddFridgeProductAsync(fridgeProduct);
        
        await _fridgeProduct.SaveAsync();
        
        return Ok();
    }
    
    [Authorize("FridgeOwner")]
    [HttpDelete]
    [ProducesResponseType(200)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteProductFromFridge(string fridgeId, string productId)
    {
        _logger.LogInformation("Delete fridge's product");

        var fridge = await _fridgeProduct.GetFridgeWithProductsAsync(fridgeId);

        if (fridge == null)
        {
            return NotFound("The fridge not found");
        }
        
        var userName = User.Identities.First().Claims.ToList()[1].Value;
        if (fridge.OwnerName != userName)
        {
            return Unauthorized("You are not the owner!");
        }

        if (fridge.Products.Any(p => p.Id == productId) == false)
        {
            return NotFound("This product not found in the fridge");
        }

        var product = fridge.Products.Single(p => p.Id == productId);

        fridge.Products.Remove(product);

        await _fridgeProduct.SaveAsync();
        
        return Ok();
    }
}