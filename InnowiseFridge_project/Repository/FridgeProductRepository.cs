using AutoMapper;
using AutoMapper.QueryableExtensions;
using InnowiseFridge_project.Data;
using InnowiseFridge_project.DTO;
using InnowiseFridge_project.Interfaces.RepositoryInterfaces;
using InnowiseFridge_project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace InnowiseFridge_project.Repository;

public class FridgeProductRepository : IFridgeProduct
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    
    public FridgeProductRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public Task<List<FridgeProductDto>> GetFridgeProductAsync(string fridgeId)
    {
        var products = _context.FridgeProducts
            .Where(f => f.FridgeId == fridgeId)
            .Select(f => new FridgeProductDto()
            {
                Id = f.Product.Id,
                ImageUrl = f.Product.ImageUrl,
                Name = f.Product.Name,
                Quantity = f.Quantity,
            })
            .ToListAsync();

        return products;
    }

    public Task<bool> FridgeExistAsync(string fridgeId)
    {
        return _context.Fridges.Where(f => f.Id == fridgeId).AnyAsync();
    }

    public Task<Fridge?> GetFridgeWithProductsAsync(string fridgeId)
    {
        return _context.Fridges
            .Where(f => f.Id == fridgeId)
            .Include(f => f.Products)
            .SingleOrDefaultAsync();
    }

    public Task<Product?> GetProductAsync(string productId)
    {
        return _context.Products.Where(p => p.Id == productId).SingleOrDefaultAsync();
    }

    public ValueTask<EntityEntry<FridgeProduct>> AddFridgeProductAsync(FridgeProduct fridgeProduct)
    {
        return _context.FridgeProducts.AddAsync(fridgeProduct);
    }

    public Task<int> SaveAsync()
    {
        return _context.SaveChangesAsync();
    }
}