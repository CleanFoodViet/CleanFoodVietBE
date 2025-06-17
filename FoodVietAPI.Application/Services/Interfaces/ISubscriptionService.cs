using CleanFoodVietAPI.Application.DTOs.SubscriptionContractDTOs;
using CleanFoodVietAPI.Data.Paginate;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface ISubscriptionService
    {
        Task<IPaginate<SubscriptionContractDTO>> GetSubscriptionContractList(int page, int size);
    }
}
