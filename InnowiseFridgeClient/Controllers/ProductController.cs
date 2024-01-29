using InnowiseFridge_project.Services;
using InnowiseFridgeClient.Services;
using InnowiseFridgeClient.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InnowiseFridgeClient.Controllers;

public class ProductController : Controller
{
    private readonly ILogger<ProductController> _logger;
    private readonly HttpClientService _client;
    private readonly FileService _fileService;
    
    public ProductController(ILogger<ProductController> logger, HttpClientService client, FileService fileService)
    {
        _logger = logger;
        _client = client;
        _fileService = fileService;
    }
    
    public async Task<IActionResult> Index()
    {
        _logger.LogInformation("Index");
        
        var token = HttpContext.Session.GetString("AuthToken");
        if (token == null)
        {
            ViewBag.Error = "token failed";
            return View(new List<ProductViewModel>());
        }
        
        var userName = HttpContext.Session.GetString("AuthUserName");

        if (userName == null)
        {
            ViewBag.Error = "user name failed";
            return View(new List<ProductViewModel>());
        }
        
        _client.Authentificating(token);
        var response = await _client.GetClient().GetAsync("/api/Product");
        if (!response.IsSuccessStatusCode)
        {
            var content = response.Content.ReadAsStreamAsync();
            ViewBag.Error = $"response failed: {content}";
            return View(new List<ProductViewModel>());
        }
        var contentData = await response.Content.ReadFromJsonAsync<List<ProductViewModel>>();
        
        return View(contentData);
    }

    public async Task<IActionResult> Edit(string productId)
    {
        var token = HttpContext.Session.GetString("AuthToken");
        if (token == null)
        {
            ViewBag.Error = "token failed";
            return View(new EditProductViewModel());
        }
        
        var userName = HttpContext.Session.GetString("AuthUserName");

        if (userName == null)
        {
            ViewBag.Error = "user name failed";
            return View(new EditProductViewModel());
            // return RedirectToAction("Index");
        }
        
        _client.Authentificating(token);
        var response = await _client.GetClient().GetAsync($"/api/Product/{productId}");
        if (!response.IsSuccessStatusCode)
        {
            var content = response.Content.ReadAsStreamAsync();
            ViewBag.Error = $"response failed: {content}";
            return View(new EditProductViewModel());
            // return RedirectToAction("Index");
        }
        
        var contentData = await response.Content.ReadFromJsonAsync<ProductViewModel>();
        var product = new EditProductViewModel()
        {
            Id = contentData!.Id,
            Name = contentData.Name,
            DefaultQuantity = contentData.DefaultQuantity,
        };
        
        return View(product);
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(EditProductViewModel productViewModel)
    {
        var token = HttpContext.Session.GetString("AuthToken");
        if (token == null)
        {
            ViewBag.Error = "token failed";
            return View(productViewModel);
        }
        
        var userName = HttpContext.Session.GetString("AuthUserName");

        if (userName == null)
        {
            ViewBag.Error = "user name failed";
            return View(productViewModel);
        }
        
        _client.Authentificating(token);
        
        string nameFile = _fileService.GetUniqueName(productViewModel.Image);
        
        var product = new ProductViewModel()
        {
            Id = productViewModel.Id,
            Name = productViewModel.Name,
            DefaultQuantity = productViewModel.DefaultQuantity,
            ImageUrl = nameFile,
        };
        
        var response = await _client.GetClient().PatchAsJsonAsync($"/api/Product", product);
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStreamAsync();
            ViewBag.Error = $"You don't have any rights";
            return View(new EditProductViewModel());
        }

        _logger.LogInformation("Создание файла");
        await _fileService.CreateFileAsync(productViewModel.Image, nameFile);
        
        return RedirectToAction("Index");
    }
}