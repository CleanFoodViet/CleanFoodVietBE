using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.ProductCategoryDTO;
using CleanFoodVietAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.Mappers
{
    public class ProductCategoryMapper : Profile
    {
        public ProductCategoryMapper()
        {
            CreateMap<CreateProductCategoryDTO, ProductCategory>()
                .ForMember(des => des.ProductCategoryId, opt => opt.MapFrom(src => Ulid.NewUlid()));

            CreateMap<UpdateProductCategoryDTO, ProductCategory>()
                .ForAllMembers(opt => opt.Condition((src, des, srcMember) => srcMember != null));

            CreateMap<ProductCategory, GetProductCategoryDTO>();
        }
    }
}
