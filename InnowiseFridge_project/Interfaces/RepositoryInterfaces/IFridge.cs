using System.Security.Claims;
using InnowiseFridge_project.DTO;
using InnowiseFridge_project.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace InnowiseFridge_project.Interfaces.RepositoryInterfaces;

public interface IFridge
{
    Task<List<FridgeDto>> GetFridgeAsync();
    Task<string?> GetFridgeModelIdByNameAsync(string name);
    Task<List<FridgeModelDto>> GetFridgeModelsAsync();
    ValueTask<EntityEntry<Fridge>> AddFridgeAsync(Fridge fridge);
    Task<int> SaveAsync();
    Task<bool> ExistFridgeNameAsync(string name, string userName);
}