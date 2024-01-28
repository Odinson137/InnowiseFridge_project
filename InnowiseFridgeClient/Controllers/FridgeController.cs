using System.Text.Json.Serialization;
using InnowiseFridgeClient.Services;
using InnowiseFridgeClient.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InnowiseFridgeClient.Controllers;

public class FridgeController : Controller
{
    private readonly ILogger<FridgeController> _logger;
    private readonly HttpClientService _client;
    
    public FridgeController(ILogger<FridgeController> logger, HttpClientService client)
    {
        _logger = logger;
        _client = client;
    }
    
    public async Task<IActionResult> Index()
    {
        _logger.LogInformation("Index");
        
        var response = await _client.GetClient().GetAsync("/api/Fridge");
        
        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "failed";
        }

        var userName = HttpContext.Session.GetString("AuthUserName");
        if (userName == null)
        {
            ViewBag.Error = "user name not found"; 
        }
        
        ViewBag.UserName = userName;
        
        var fridges = await response.Content.ReadFromJsonAsync<List<FridgeViewModel>>();
        
        return View(fridges);
    }
    
    public IActionResult Add(string id)
    {
        _logger.LogInformation("Add");
        var fridge = new FridgeViewModel();
        return View(fridge);
    }
    
    [HttpPost]
    public async Task<IActionResult> Add(FridgeViewModel fridgeViewModel)
    {
        _logger.LogInformation("Post Add");
        var token = HttpContext.Session.GetString("AuthToken");
        if (token == null)
        {
            ViewBag.Error = "token failed";
            return View();
        }
        
        var userName = HttpContext.Session.GetString("AuthUserName");

        if (userName == null)
        {
            ViewBag.Error = "user name failed";
            return View();
        }

        fridgeViewModel.OwnerName = userName;
        
        _client.Authentificating(token);
        
        var response = await _client.GetClient().PostAsJsonAsync("/api/Fridge", fridgeViewModel);
        var content = await response.Content.ReadAsStreamAsync();
        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "failed";
            return View();
        }
        
        return RedirectToAction("Index");

    }
    
    public IActionResult Edit(string id)
    {
        _logger.LogInformation("Edit");
        var fridge = new FridgeViewModel();
        return View(fridge);
    }

    
    [HttpPut]
    public IActionResult Edit(string id, FridgeViewModel fridgeViewModel)
    {
        _logger.LogInformation("Put Edit");
        var fridge = new FridgeViewModel();
        return RedirectToAction("Index");
    }
    
    [HttpDelete]
    public IActionResult Delete(string id)
    {
        _logger.LogInformation("Delete");
        return RedirectToAction("Index");
    }
}