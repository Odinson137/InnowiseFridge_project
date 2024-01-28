using InnowiseFridge_project.DTO;
using InnowiseFridge_project.Interfaces.RepositoryInterfaces;
using InnowiseFridge_project.Models;
using Microsoft.AspNetCore.Authorization;
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

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<ActionResult<List<FridgeDto>>> GetFridges()
    {
        _logger.LogInformation("Get fridges");
        var fridges = await _fridge.GetFridgeAsync();
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
            return BadRequest("This name is already taken");
        }

        var modelId = await _fridge.GetFridgeModelIdByNameAsync(fridgeDto.FridgeModelName);
        if (modelId == null)
        {
            return NotFound("FridgeModel id is not found");
        }
        
        var fridge = new Fridge()
        {
            Name = fridgeDto.Name,
            OwnerName = fridgeDto.OwnerName,
            FridgeModelId = modelId,
        };
        
        await _fridge.AddFridgeAsync(fridge);
        await _fridge.SaveAsync();
        return Ok(fridge.Id);
    }
}