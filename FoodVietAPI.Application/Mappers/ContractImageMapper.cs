using AutoMapper;
using CleanFoodVietAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.Mappers
{
    public class ContractImageMapper : Profile
    {
        public ContractImageMapper()
        {
            CreateMap<string, ContractImage>()
                .ForMember(des => des.ContractImageId, opt => opt.MapFrom(src => Ulid.NewUlid()))
                .ForMember(des => des.OrderId, opt => opt.MapFrom(
                        (src, des, member, context) => context.Items["OrderId"]
                    ));
        }
    }
}
