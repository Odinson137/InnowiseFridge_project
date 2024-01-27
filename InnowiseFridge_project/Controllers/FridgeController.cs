using InnowiseFridge_project.DTO;
using InnowiseFridge_project.Interfaces.RepositoryInterfaces;
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
    
}