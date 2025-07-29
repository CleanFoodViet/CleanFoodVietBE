using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.ReportDTOs;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Enums.ReportEnums;

namespace CleanFoodVietAPI.Application.Mappers
{
    public class ReportMapper : Profile
    {
        public ReportMapper()
        {
            CreateMap<CreateReportDTO, Report>()
                .ForMember(des => des.ReportId, opt => opt.MapFrom(src => Ulid.NewUlid()))
                .ForMember(des => des.Status, opt => opt.MapFrom(src => ReportStatusEnum.PENDING.ToString()))
                .ForMember(des => des.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(des => des.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<CreateUserReportDTO, Report>()
               .ForMember(des => des.ReportId, opt => opt.MapFrom(src => Ulid.NewUlid()))
               .ForMember(des => des.Status, opt => opt.MapFrom(src => ReportStatusEnum.PENDING.ToString()))
               .ForMember(des => des.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
               .ForMember(des => des.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<Report, ReportDTO>();
        }
    }
}
