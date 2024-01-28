using InnowiseFridgeClient.Services;
using InnowiseFridgeClient.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InnowiseFridgeClient.Controllers;

public class FridgeModelController : Controller
{
    private readonly ILogger<FridgeModelController> _logger;
    private readonly HttpClient _client;
    
    public FridgeModelController(ILogger<FridgeModelController> logger, HttpClientService client)
    {
        _logger = logger;
        _client = client.GetClient();
    }

    public async Task<IActionResult> Index()
    {
        _logger.LogInformation("Index");

        var response = await _client.GetAsync("/api/Fridge/Models");

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "failed";
        }
        
        var fridgeModels = await response.Content.ReadFromJsonAsync<List<FridgeModelViewModel>>();
        return View(fridgeModels);
    }
}