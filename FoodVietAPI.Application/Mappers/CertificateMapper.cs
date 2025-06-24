using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.AuthDTOs;
using CleanFoodVietAPI.Application.DTOs.CertificateDTOs;
using CleanFoodVietAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.Mappers
{
    public class CertificateMapper : Profile
    {
        public CertificateMapper()
        {
            CreateMap<Certificate, CertificateDTO>();

            CreateMap<GardenerRegisterDTO, Certificate>()
                .ForMember(des => des.CertificateId, opt => opt.MapFrom(src => Ulid.NewUlid()));
        }
    }
}
