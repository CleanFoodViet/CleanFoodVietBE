using CleanFoodVietAPI.Application.DTOs.SubscriptionContractDTOs;
using CleanFoodVietAPI.Data.Paginate;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface ISubscriptionService
    {
        Task<IPaginate<SubscriptionContractListDTO>> GetSubscriptionContractList(int page, int size);
        Task<SubscriptionContractDTO> GetSubscriptionContractInformation(string id);
    }
}
