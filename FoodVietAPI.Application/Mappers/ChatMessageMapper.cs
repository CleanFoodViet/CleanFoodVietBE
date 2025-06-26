using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.ChatMessageDTOs;
using CleanFoodVietAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.Mappers
{
    public class ChatMessageMapper : Profile
    {
        public ChatMessageMapper()
        {
            CreateMap<ChatMessageDTO, ChatMessage>()
                .ForMember(des => des.ChatMessageId, opt => opt.MapFrom(src => Ulid.NewUlid()));
        }
    }
}
