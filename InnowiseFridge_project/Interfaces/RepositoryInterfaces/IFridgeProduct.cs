using InnowiseFridge_project.DTO;
using InnowiseFridge_project.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace InnowiseFridge_project.Interfaces.RepositoryInterfaces;

public interface IFridgeProduct
{
    Task<List<ProductDto>> GetFridgeProductAsync(string fridgeId);
    Task<bool> FridgeExistAsync(string fridgeId);
    Task<Fridge?> GetFridgeWithProductsAsync(string fridgeId);
    Task<Product?> GetProductAsync(string productId);
    ValueTask<EntityEntry<FridgeProduct>> AddFridgeProductAsync(FridgeProduct fridgeProduct);
    Task<int> SaveAsync();
}