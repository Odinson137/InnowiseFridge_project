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
        
        var token = HttpContext.Session.GetString("AuthToken");
        if (token == null)
        {
            ViewBag.Error = "you are not authorized, man";
            return View(new List<FridgeViewModel>());
        }
        
        var userName = HttpContext.Session.GetString("AuthUserName");

        if (userName == null)
        {
            ViewBag.Error = "user name was not founded";
            return View(new List<FridgeViewModel>());
        }
        
        _client.Authentificating(token);
        
        var response = await _client.GetClient().GetAsync("/api/Fridge");
        
        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "failed";
            return View(new List<FridgeViewModel>());
        }
        
        ViewBag.UserName = userName;
        
        var fridges = await response.Content.ReadFromJsonAsync<List<FridgeViewModel>>();
        
        return View(fridges);
    }
    
    public async Task<IActionResult> Products(string fridgeId)
    {
        _logger.LogInformation("Index");

        var response = await _client.GetClient().GetAsync($"/api/FridgeProduct?fridgeId={fridgeId}");
        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "failed";
            return RedirectToAction("Index");
        }
        
        var products = await response.Content.ReadFromJsonAsync<List<FridgeProductViewModel>>();
        
        return View(products);
    }
    
    public async Task<IActionResult> Add()
    {
        _logger.LogInformation("Add");
        
        var token = HttpContext.Session.GetString("AuthToken");
        if (token == null)
        {
            ViewBag.Error = "you are not authorized, man";
            return View("Index");
        }
        
        _client.Authentificating(token);

        var response = await _client.GetClient().GetAsync($"/api/Product");
        var products = await response.Content.ReadFromJsonAsync<List<FridgeProductViewModel>>();

        int num = 0;
        var fridge = new AddFridgeViewModel
        {
            Products = new List<FridgeStartProductsViewModel>(
                products!.Select(p => new FridgeStartProductsViewModel()
                {
                    Num = num++,
                    ProductId = p.Id,
                    Name = p.Name,
                    Count = 0,
                }).ToList())
        };

        return View(fridge);
    }
    
    [HttpPost]
    public async Task<IActionResult> Add(AddFridgeViewModel fridgeViewModel)
    {
        _logger.LogInformation("Post Add");
        var token = HttpContext.Session.GetString("AuthToken");
        if (token == null)
        {
            ViewBag.Error = "token failed";
            return View("Add");
        }
        
        var userName = HttpContext.Session.GetString("AuthUserName");

        if (userName == null)
        {
            ViewBag.Error = "user name failed";
            return View("Add");
        }

        fridgeViewModel.OwnerName = userName;

        fridgeViewModel.Products = fridgeViewModel.Products?.ToList().Where(c => c.Count > 0).ToList();
        
        _client.Authentificating(token);
        var response = await _client.GetClient().PostAsJsonAsync("/api/Fridge", fridgeViewModel);
        if (response.IsSuccessStatusCode) 
            return RedirectToAction("Index");
        
        ViewBag.Error = "failed";
        return View();

    }
    
    public async Task<IActionResult> Edit(string id)
    {
        _logger.LogInformation("Edit");
                
        var token = HttpContext.Session.GetString("AuthToken");
        if (token == null)
        {
            ViewBag.Error = "token failed";
            return RedirectToAction("Index");
        }
        _client.Authentificating(token);
        
        var response = await _client.GetClient().GetAsync($"/api/Fridge/{id}");
        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "failed with open getting the fridge model";
            return RedirectToAction("Index");
        } 

        var fridge = await response.Content.ReadFromJsonAsync<FridgeViewModel>();
        return View(fridge);
    }

    
    [HttpPost]
    public async Task<IActionResult> Edit(FridgeViewModel fridgeViewModel)
    {
        _logger.LogInformation("Put Edit");
        
        var token = HttpContext.Session.GetString("AuthToken");
        if (token == null)
        {
            ViewBag.Error = "token failed";
            return View(fridgeViewModel.Id);
        }
        
        var userName = HttpContext.Session.GetString("AuthUserName");

        if (userName == null)
        {
            ViewBag.Error = "user name failed";
            return View("Add");
        }

        fridgeViewModel.OwnerName = userName;
        
        _client.Authentificating(token);

        var response = await _client.GetClient().PutAsJsonAsync("/api/Fridge", fridgeViewModel);
        
        if (response.IsSuccessStatusCode) return RedirectToAction("Index");

        ViewBag.Error = "failed with edit";
        return RedirectToAction("Index");
    }
    
    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        _logger.LogInformation("Delete");
        var token = HttpContext.Session.GetString("AuthToken");
        if (token == null)
        {
            ViewBag.Error = "token failed";
            return View(id);
        }
        
        var userName = HttpContext.Session.GetString("AuthUserName");

        if (userName == null)
        {
            ViewBag.Error = "user name failed";
            return View(id);
        }
        
        _client.Authentificating(token);
        
        var response = await _client.GetClient().DeleteAsync($"/api/Fridge/{id}");
        if (response.IsSuccessStatusCode) return RedirectToAction("Index");
        
        ViewBag.Error = "the delete failed";

        return RedirectToAction("Index");
    }
}