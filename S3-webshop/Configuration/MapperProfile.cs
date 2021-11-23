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
            CreateMap<NewProductResource, Product>();
            CreateMap<Product, ProductResource>();
            CreateMap<Product, ProductWithCategoriesResource>()
                .ForMember(dto => dto.Categories, opt => opt.MapFrom(x => x.Categories));

            CreateMap<CategoryResource, Category>();
            CreateMap<Category, CategoryResource>();
            CreateMap<Category, CategoryProductResource>()
                .ForMember(dto => dto.Products, opt => opt.MapFrom(x => x.Products));

            CreateMap<User, UserResource>();
            CreateMap<UserResource, User>();
            CreateMap<NewUserResource, User>();
            CreateMap<LoginResource, User>();
        }

    }
}
