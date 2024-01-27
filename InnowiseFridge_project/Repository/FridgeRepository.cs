using System.Collections;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using InnowiseFridge_project.Data;
using InnowiseFridge_project.DTO;
using Microsoft.EntityFrameworkCore;

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
    
    public Task<List<FridgeDto>> GetFridgeAsync()
    {
        var fridges = _context.Fridges.ProjectTo<FridgeDto>(_mapper.ConfigurationProvider).ToListAsync();
        return fridges;
    }
}