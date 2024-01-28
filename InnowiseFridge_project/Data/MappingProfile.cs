using AutoMapper;
using InnowiseFridge_project.DTO;
using InnowiseFridge_project.Models;

namespace InnowiseFridge_project.Data;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Fridge, FridgeDto>()
            .ForMember(c => c.FridgeModelName, 
                a => a.MapFrom(c => c.FridgeModel.Name));
        CreateMap<Product, ProductDto>();
        CreateMap<FridgeModel, FridgeModelDto>();
    }
}