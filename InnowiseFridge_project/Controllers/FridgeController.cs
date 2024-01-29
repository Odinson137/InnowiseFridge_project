using InnowiseFridge_project.DTO;
using InnowiseFridge_project.Interfaces.RepositoryInterfaces;
using InnowiseFridge_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace InnowiseFridge_project.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FridgeController : ControllerBase
{
    private readonly ILogger<FridgeController> _logger;
    private readonly IFridge _fridge;
    public FridgeController(ILogger<FridgeController> logger, IFridge fridge)
    {
        _logger = logger;
        _fridge = fridge;
    }

    
    [Authorize]
    [HttpGet("{fridgeId}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<FridgeDto>> GetFridge(string fridgeId)
    {
        _logger.LogInformation("Get fridge");
        var fridge = await _fridge.GetFridgeAsync(fridgeId);
        if (fridge == null)
        {
            return NotFound("The fridge was not founded");
        } 
        return Ok(fridge);
    }

    
    [Authorize]
    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<ActionResult<List<FridgeDto>>> GetFridges()
    {
        _logger.LogInformation("Get fridges");
        var fridges = await _fridge.GetFridgesAsync();
        return Ok(fridges);
    }
 
    [AllowAnonymous]
    [HttpGet("Models")]
    [ProducesResponseType(200)]
    public async Task<ActionResult<List<FridgeModelDto>>> GetFridgeModels()
    {
        _logger.LogInformation("Get fridgeModels");
        var fridgeModels = await _fridge.GetFridgeModelsAsync();
        return Ok(fridgeModels);
    }
    
    [Authorize]
    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<string>> AddFridge(AddFridgeDto fridgeDto)
    {
        _logger.LogInformation("Add fridge");

        if (await _fridge.ExistFridgeNameAsync(fridgeDto.Name, User.Identities.First().Claims.ToList()[1].Value) == true)
        {
            return BadRequest("This name was already taken");
        }

        var modelId = await _fridge.GetFridgeModelIdByNameAsync(fridgeDto.FridgeModelName);
        if (modelId == null)
        {
            return NotFound("FridgeModel id was not found");
        }
        
        var fridge = new Fridge()
        {
            Name = fridgeDto.Name,
            OwnerName = fridgeDto.OwnerName,
            FridgeModelId = modelId,
            Description = fridgeDto.Description,
        };

        await _fridge.AddFridgeAsync(fridge);
        
        if (fridgeDto.Products != null)
        {
            foreach (var product in fridgeDto.Products)
            {
                var dbProduct = await _fridge.GetProductAsync(product.ProductId);
                if (dbProduct == null)
                {
                    return NotFound("The product was not founded");
                }

                var fridgeProduct = new FridgeProduct()
                {
                    Product = dbProduct,
                    FridgeId = fridge.Id,
                    Quantity = product.Count,
                };

                await _fridge.AddFridgeProductAsync(fridgeProduct);
            }
        }
        
        await _fridge.SaveAsync();
        return Ok(fridge.Id);
    }
    
    [Authorize]
    [HttpPut]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<string>> PutFridge(EditFridgeDto fridgeDto)
    {
        _logger.LogInformation("Edit fridge");

        var dbFridge = await _fridge.GetFridgeModelAsync(fridgeDto.Id);
        
        if (dbFridge == null)
        {
            return NotFound("This fridge was not founded");
        }

        if (dbFridge.Name != fridgeDto.Name)
        {
            var fridgeNameExist =
                await _fridge.ExistFridgeNameAsync(fridgeDto.Name, User.Identities.First().Claims.ToList()[1].Value);

            if (fridgeNameExist)
            {
                return BadRequest("This name was already taken");
            }
        }

        var modelId = await _fridge.GetFridgeModelIdByNameAsync(fridgeDto.FridgeModelName);
        if (modelId == null)
        {
            return NotFound("FridgeModel id was not found");
        }
        
        dbFridge.Name = fridgeDto.Name;
        dbFridge.OwnerName = fridgeDto.OwnerName;
        dbFridge.FridgeModelId = modelId;
        dbFridge.Description = fridgeDto.Description;

        await _fridge.SaveAsync();
        return Ok();
    }
    
    [Authorize]
    [HttpDelete("{fridgeId}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<string>> DeleteFridge(string fridgeId)
    {
        _logger.LogInformation("Delete fridge");

        var dbFridge = await _fridge.GetFridgeModelAsync(fridgeId);
        
        if (dbFridge == null)
        {
            return NotFound("This fridge was not founded");
        }

        _fridge.RemoveFridgeAsync(dbFridge);

        await _fridge.SaveAsync();
        return Ok();
    }
}