using CleanFoodVietAPI.Application.DTOs.Gardener;
using CleanFoodVietAPI.Application.DTOs.PaymentHistoryDTOs;
using CleanFoodVietAPI.Application.DTOs.SubscriptionContractDTOs;
using CleanFoodVietAPI.Data.Paginate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface ISubscriptionContractService
    {
        Task<IPaginate<SubscriptionContractDTO>> GetContractsAsync(
            int page, int size,
            string? filterField = null, string? filterValue = null,
            string? sortField = null, string? sortOrder = "asc",
            string? search = null);

        Task<SubscriptionContractDTO> GetContractDetailAsync(Ulid subscriptionId);

        Task<IPaginate<ContractHistoryDTO>> GetContractHistoryAsync(string gardenerId, int page, int size);

        Task<SubscriptionContractDetailDTO?> GetActiveContractAsync(string gardenerId);

        Task<CurrentSubscriptionContractBenefitDTO> GetCurrentSubscriptionContractBenefit(string gardenerId);

    }
}
