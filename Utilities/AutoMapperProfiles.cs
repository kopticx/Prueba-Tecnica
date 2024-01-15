using AutoMapper;
using MongoDB.Bson;
using PruebaTecnica.Entities;
using PruebaTecnica.Models;

namespace PruebaTecnica.Utilities;

public class AutoMapperProfiles : Profile
{
  public AutoMapperProfiles()
  {
    CreateMap<Product, ProductDto>();
    CreateMap<ProductDto, Product>()
      .ForMember(x => x.Id, opt => opt.MapFrom(src => ObjectId.GenerateNewId().ToString()));
    CreateMap<Category, CategoryDto>().ReverseMap();
    CreateMap<SubCategory, SubCategoryDto>().ReverseMap();
  }
}