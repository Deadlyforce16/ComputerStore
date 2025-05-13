using AutoMapper;
using ComputerStore.Domain.Entities;
using ComputerStore.Application.DTOs;

namespace ComputerStore.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Stock, StockDto>().ReverseMap();
        }
    }
}
