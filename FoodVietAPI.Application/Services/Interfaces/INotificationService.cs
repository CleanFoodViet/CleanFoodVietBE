using CleanFoodVietAPI.Application.DTOs.NotificationDTOs;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface INotificationService
    {
        Task<List<NotificationDTO>> GetNotificationList(string accountId);
    }
}
