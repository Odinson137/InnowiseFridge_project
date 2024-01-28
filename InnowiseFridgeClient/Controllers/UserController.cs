using InnowiseFridgeClient.Models;
using InnowiseFridgeClient.Services;
using InnowiseFridgeClient.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InnowiseFridgeClient.Controllers;

public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;
    private readonly HttpClient _client;
    
    public UserController(ILogger<UserController> logger, HttpClientService client)
    {
        _logger = logger;
        _client = client.GetClient();
    }
    
    public IActionResult Index()
    {
        _logger.LogInformation("Index");
        return View();
    }

    
    public IActionResult Login()
    {
        _logger.LogInformation("Login");
        var response = new LoginViewModel();
        return View(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        _logger.LogInformation("Post login");

        var response = await _client.PostAsJsonAsync("/api/User/Authorization", loginViewModel);
        
        if (response.IsSuccessStatusCode)
        {
            var userInfo = await response.Content.ReadFromJsonAsync<UserInfo>();
            
            HttpContext.Session.SetString("AuthToken", userInfo!.Token);
            HttpContext.Session.SetString("AuthId", userInfo.UserId);
            HttpContext.Session.SetString("AuthUserName", loginViewModel.UserName);
            
            return RedirectToAction("Index", "User");
        }
        
        TempData["Error"] = "Wrong credentials. Please, try again";
        return View(loginViewModel);
    }
    
    public IActionResult Registration()
    {
        _logger.LogInformation("Registration");
        
        var response = new RegistrationViewModel();
        return View(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> Registration(RegistrationViewModel registrationViewModel)
    {
        _logger.LogInformation("Post regist");

        var response = await _client.PostAsJsonAsync("/api/User/Registration", registrationViewModel);
        
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Login");
        }
        
        TempData["Error"] = "Wrong credentials. Please, try again";
        return View(registrationViewModel);
    }


}