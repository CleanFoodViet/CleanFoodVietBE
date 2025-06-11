using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.ProductPriceDTOs;
using CleanFoodVietAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.Mappers
{
    public class ProductPriceMapper : Profile
    {
        public ProductPriceMapper()
        {
            CreateMap<ProductPrice, ProductPriceDTO>();
        }
    }
}
