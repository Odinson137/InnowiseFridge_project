using Microsoft.AspNetCore.Mvc;

namespace InnowiseFridge_project.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FridgeController : ControllerBase
{
    private readonly ILogger<FridgeController> _logger;
    
    public FridgeController(ILogger<FridgeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetFridges()
    {
        _logger.LogInformation("Get fridges");
        return NotFound();
    }
    
}