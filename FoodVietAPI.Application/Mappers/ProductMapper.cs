using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.ProductCaertificateDTOs;
using CleanFoodVietAPI.Application.DTOs.ProductDTOs;
using CleanFoodVietAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.Mappers
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<CreateProductDTO, Product>()
                .ForMember(des => des.ProductId, opt => opt.MapFrom(src => Ulid.NewUlid()))
                .ForMember(des => des.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(des => des.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<string, ProductTag>()
                .ForMember(des => des.ProductTagId, opt => opt.MapFrom(src => Ulid.NewUlid()))
                .ForMember(des => des.GardenerId, opt => opt.MapFrom(
                        (src, des, member, context) => context.Items["GardenerId"]
                    ))
                .ForMember(des => des.ProductId, opt => opt.MapFrom(
                        (src, des, member, context) => context.Items["ProductId"]
                    ));

            CreateMap<ProductCertificateDTO, ProductCertificate>()
                .ForMember(des => des.ProductCertificateId, opt => opt.MapFrom(src => Ulid.NewUlid()))
                .ForMember(des => des.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(des => des.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(des => des.ProductId, opt => opt.MapFrom(
                        (src, des, member, context) => context.Items["ProductId"]
                    ));
        }
    }
}
