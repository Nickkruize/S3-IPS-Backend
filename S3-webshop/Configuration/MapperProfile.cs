﻿using AutoMapper;
using DAL.ContextModels;
using Microsoft.AspNetCore.Identity;
using S3_webshop.Resources;

namespace S3_webshop.Configuration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<ProductWithCategoriesResource, Product>();
            CreateMap<NewProductResource, Product>();
            CreateMap<Product, ProductWithCategoriesResource>();
            CreateMap<Product, ProductWithCategoriesResource>()
                .ForMember(dto => dto.Categories, opt => opt.MapFrom(x => x.Categories));
            CreateMap<Product, ProductResource>();
            CreateMap<ProductResource, Product>();

            CreateMap<CategoryResource, Category>();
            CreateMap<NewCategoryResource, Category>();
            CreateMap<Category, CategoryResource>();
            CreateMap<Category, CategoryProductResource>()
                .ForMember(dto => dto.Products, opt => opt.MapFrom(x => x.Products));
            CreateMap<UpdateCategoryResource, Category>();

            CreateMap<IdentityUser, UserResource>();
            CreateMap<UserResource, IdentityUser>();
            CreateMap<IdentityUser, User>();
            CreateMap<NewUserResource, User>();

            CreateMap<Order, OrdersResource>();
            CreateMap<OrderItem, OrderItemResource>();
        }

    }
}
