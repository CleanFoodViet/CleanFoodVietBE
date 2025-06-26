using CleanFoodVietAPI.Application.DTOs.ChatMessageDTOs;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface IChatMessageService
    {
        Task SaveMessage(ChatMessageDTO request);
    }
}
