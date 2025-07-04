using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.AppointmentDTOs;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Enums.AppointmentEnums;

namespace CleanFoodVietAPI.Application.Mappers
{
    public class AppointmentMapper : Profile
    {
        public AppointmentMapper()
        {
            CreateMap<CreateAppointmentDTO, Appointment>()
                .ForMember(des => des.AppointmentId, opt => opt.MapFrom(src => Ulid.NewUlid()))
                .ForMember(des => des.Status, opt => opt.MapFrom(src => AppointmentStatusEnum.ACTIVE.ToString()))
                .ForMember(des => des.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(des => des.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<UpdateAppointmentDTO, Appointment>()
                .ForMember(des => des.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}
