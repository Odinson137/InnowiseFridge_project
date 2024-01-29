using System.Collections;
using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using InnowiseFridge_project.Data;
using InnowiseFridge_project.DTO;
using InnowiseFridge_project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace InnowiseFridge_project.Interfaces.RepositoryInterfaces;

public class FridgeRepository : IFridge
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    
    public FridgeRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<Fridge?> GetFridgeModelAsync(string fridgeId)
    {
        return _context.Fridges.Where(f => f.Id == fridgeId).SingleOrDefaultAsync();
    }

    public Task<FridgeDto?> GetFridgeAsync(string fridgeId)
    {
        var fridge = _context.Fridges
            .Where(f => f.Id == fridgeId)
            .ProjectTo<FridgeDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
        return fridge;
    }

    public Task<Product?> GetProductAsync(string productId)
    {
        return _context.Products.Where(p => p.Id == productId).SingleOrDefaultAsync();
    }

    public Task<List<FridgeDto>> GetFridgesAsync()
    {
        var fridges = _context.Fridges.ProjectTo<FridgeDto>(_mapper.ConfigurationProvider).ToListAsync();
        return fridges;
    }

    public Task<string?> GetFridgeModelIdByNameAsync(string name)
    {
        return _context.FridgeModels.Where(c => c.Name == name).Select(c => c.Id).SingleOrDefaultAsync();
    }

    public Task<List<FridgeModelDto>> GetFridgeModelsAsync()
    {
        return _context.FridgeModels.ProjectTo<FridgeModelDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public EntityEntry<Fridge> RemoveFridgeAsync(Fridge fridge)
    {
        return _context.Fridges.Remove(fridge);
    }

    public Task<bool> ExistFridgeNameAsync(string name, string userName)
    {
        return _context.Fridges.Where(f => f.Name == name && f.OwnerName == userName).AnyAsync();
    }
    
    public ValueTask<EntityEntry<Fridge>> AddFridgeAsync(Fridge fridge)
    {
        return _context.Fridges.AddAsync(fridge);
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