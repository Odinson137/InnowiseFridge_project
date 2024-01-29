using AutoMapper;
using AutoMapper.QueryableExtensions;
using InnowiseFridge_project.Data;
using InnowiseFridge_project.DTO;
using InnowiseFridge_project.Interfaces.RepositoryInterfaces;
using InnowiseFridge_project.Models;
using Microsoft.EntityFrameworkCore;

namespace InnowiseFridge_project.Repository;

public class ProductRepository : IProduct
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    
    public ProductRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public Task<int> SaveAsync()
    {
        return _context.SaveChangesAsync();
    }

    public Task<Product?> GetProductAsync(string productId)
    {
        return _context.Products.Where(p => p.Id == productId).SingleOrDefaultAsync();
    }

    public Task<List<ProductDto>?> GetProductsAsync()
    {
        return _context.Products.ProjectTo<ProductDto>(_mapper.ConfigurationProvider).ToListAsync();
    }
}