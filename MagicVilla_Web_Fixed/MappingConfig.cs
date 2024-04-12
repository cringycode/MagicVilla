using AutoMapper;
using MagicVilla_Web_Fixed.Models.DTO;

namespace MagicVilla_Web_Fixed;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<VillaDTO, VillaCreateDTO>().ReverseMap();
        CreateMap<VillaDTO, VillaUpdateDTO>().ReverseMap();
        
        CreateMap<VillaNumberDTO, VillaCreateDTO>().ReverseMap();
        CreateMap<VillaNumberDTO, VillaNumberUpdateDTO>().ReverseMap();
    }
}