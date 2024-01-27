using InnowiseFridge_project.Data;
using InnowiseFridge_project.Models;
using Microsoft.EntityFrameworkCore;

namespace InnowiseFridge_project.Interfaces.RepositoryInterfaces;

public class ProductRepository : IProduct
{
    private readonly DataContext _context;
    
    public ProductRepository(DataContext context)
    {
        _context = context;
    }
    
    public Task<int> SaveAsync()
    {
        return _context.SaveChangesAsync();
    }

    public Task<Product?> GetProductAsync(string productId)
    {
        return _context.Products.Where(p => p.Id == productId).SingleOrDefaultAsync();
    }
}