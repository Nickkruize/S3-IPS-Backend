using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.ContextModels;

namespace S3_webshop.Configuration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<ProductResource, Product>();
            CreateMap<Product, ProductResource>()
                .ForMember(dto => dto.Categories, opt => opt.MapFrom(x => x.ProductCategories.Select(y => y.Category).ToList()));
        }

    }
}
