using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.NotificationDTOs;
using CleanFoodVietAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
