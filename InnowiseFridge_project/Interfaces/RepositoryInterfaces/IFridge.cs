using System.Security.Claims;
using InnowiseFridge_project.DTO;
using InnowiseFridge_project.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace InnowiseFridge_project.Interfaces.RepositoryInterfaces;

public interface IFridge
{
    Task<Fridge?> GetFridgeModelAsync(string fridgeId);
    Task<FridgeDto?> GetFridgeAsync(string fridgeId);
    Task<Product?> GetProductAsync(string productId);
    Task<List<FridgeDto>> GetFridgesAsync();
    Task<string?> GetFridgeModelIdByNameAsync(string name);
    Task<List<FridgeModelDto>> GetFridgeModelsAsync();
    ValueTask<EntityEntry<Fridge>> AddFridgeAsync(Fridge fridge);
    ValueTask<EntityEntry<FridgeProduct>> AddFridgeProductAsync(FridgeProduct fridgeProduct);
    Task<int> SaveAsync();
    EntityEntry<Fridge> RemoveFridgeAsync(Fridge fridge);
    Task<bool> ExistFridgeNameAsync(string name, string userName);
}