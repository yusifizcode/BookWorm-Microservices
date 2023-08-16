using AutoMapper;
using Catalog.API.DTOs;
using Catalog.API.Entities;

namespace Catalog.API.Mapping;

public class MapProfile : Profile
{
	public MapProfile()
	{
		CreateMap<Course, CourseDto>().ReverseMap();
		CreateMap<Category, CategoryDto>().ReverseMap();
		CreateMap<Feature, FeatureDto>().ReverseMap();

		CreateMap<Course, CourseCreateDto>().ReverseMap();
		CreateMap<Course, CourseUpdateDto>().ReverseMap();
	}
}
