using AutoMapper;
using Catalog.API.DTOs;
using Catalog.API.Entities;

namespace Catalog.API.Mapping;

public class MapProfile : Profile
{
	public MapProfile()
	{
		CreateMap<Product, ProductDto>().ReverseMap();
		CreateMap<Category, CategoryDto>().ReverseMap();

		CreateMap<Product, ProductCreateDto>().ReverseMap();
		CreateMap<Product, ProductUpdateDto>().ReverseMap();
	}
}
