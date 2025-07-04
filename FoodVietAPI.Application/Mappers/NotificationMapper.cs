using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.NotificationDTOs;
using CleanFoodVietAPI.Data.Entities;

namespace CleanFoodVietAPI.Application.Mappers
{
    public class NotificationMapper : Profile
    {
        public NotificationMapper()
        {
            CreateMap<CreateNotificationDTO, Notification>()
                .ForMember(des => des.NotificationId, opt => opt.MapFrom(src => Ulid.NewUlid()))
                .ForMember(des => des.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}
