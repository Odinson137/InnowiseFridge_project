using Microsoft.AspNetCore.Mvc;

namespace InnowiseFridgeClient.Controllers;

public class ProductController : Controller
{
    private readonly ILogger<ProductController> _logger;
    
    public ProductController(ILogger<ProductController> logger)
    {
        _logger = logger;
    }
    
    public IActionResult Index()
    {
        _logger.LogInformation("Index");
        return View();
    }
}