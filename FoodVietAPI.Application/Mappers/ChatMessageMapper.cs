using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.ChatMessageDTOs;
using CleanFoodVietAPI.Data.Entities;

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
