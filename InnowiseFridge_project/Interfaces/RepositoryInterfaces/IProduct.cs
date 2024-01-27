using InnowiseFridge_project.Models;

namespace InnowiseFridge_project.Interfaces.RepositoryInterfaces;

public interface IProduct
{
    Task<int> SaveAsync();
    Task<Product?> GetProductAsync(string productId);
}