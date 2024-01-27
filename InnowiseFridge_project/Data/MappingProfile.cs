using AutoMapper;
using InnowiseFridge_project.DTO;
using InnowiseFridge_project.Models;

namespace InnowiseFridge_project.Data;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Fridge, FridgeDto>();
        CreateMap<Product, ProductDto>();
    }
}