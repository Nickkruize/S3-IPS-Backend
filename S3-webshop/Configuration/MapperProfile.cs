using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.ContextModels;
using S3_webshop.Resources;

namespace S3_webshop.Configuration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<ProductResource, Product>();
            CreateMap<Product, ProductResource>();
            CreateMap<Product, ProductWithCategoriesResource>()
                .ForMember(dto => dto.Categories, opt => opt.MapFrom(x => x.Categories));

            CreateMap<CategoryResource, Category>();
            CreateMap<Category, CategoryResource>();
            CreateMap<Category, CategoryProductResource>()
                .ForMember(dto => dto.Products, opt => opt.MapFrom(x => x.Products));
        }

    }
}
