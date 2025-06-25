using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.SubscriptionContractDTOs;
using CleanFoodVietAPI.Application.DTOs.SubscriptionContractBenefitDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Application.Specifications;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.Services.Implements
{
    public class SubscriptionContractService : ISubscriptionContractService
    {
        private readonly IUnitOfWork<CleanFoodVietDbContext> _uow;
        private readonly IMapper _mapper;

        public SubscriptionContractService(
            IUnitOfWork<CleanFoodVietDbContext> uow,
            IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IPaginate<SubscriptionContractDTO>> GetContractsAsync(
            int page, int size,
            string? filterField, string? filterValue,
            string? sortField, string? sortOrder,
            string? search)
        {
            var spec = new SubscriptionContractSpecification(
                filterField, filterValue, sortField, sortOrder, search);

            var repo = _uow.GetRepository<SubscriptionContract>();
            return await repo.GetPagingListAsync(
                spec: spec,
                selector: sc => new SubscriptionContractDTO
                {
                    SubscriptionId = sc.SubscriptionId,
                    GardenerId = sc.GardenerId,
                    ServicePackageId = sc.ServicePackageId,
                    StartDate = sc.StartDate,
                    EndDate = sc.EndDate,
                    Status = sc.Status,
                    SubscriptionType = sc.SubscriptionType,
                    Benefits = sc.SubscriptionContractBenefits
                                 .Select(b => new SubscriptionContractBenefitDTO
                                 {
                                     SubscriptionContractBenefitId = b.SubscriptionContractBenefitId,
                                     BenefitType = b.BenefitType,
                                     DefaultValue = b.DefaultValue,
                                     RemainingValue = b.RemainingValue,
                                     CreatedAt = b.CreatedAt,
                                     UpdatedAt = b.UpdatedAt
                                 })
                                 .ToList()
                },
                page: page,
                size: size);
        }

        public async Task<SubscriptionContractDTO> GetContractDetailAsync(Ulid subscriptionId)
        {
            var repo = _uow.GetRepository<SubscriptionContract>();

            // Load the contract with its benefits
            var contract = await repo.GetAsync(
                selector: sc => new SubscriptionContractDTO
                {
                    SubscriptionId = sc.SubscriptionId,
                    GardenerId = sc.GardenerId,
                    ServicePackageId = sc.ServicePackageId,
                    StartDate = sc.StartDate,
                    EndDate = sc.EndDate,
                    Status = sc.Status,
                    SubscriptionType = sc.SubscriptionType,
                    CreatedAt = sc.CreatedAt,
                    Benefits = sc.SubscriptionContractBenefits
                                 .Select(b => new SubscriptionContractBenefitDTO
                                 {
                                     SubscriptionContractBenefitId = b.SubscriptionContractBenefitId,
                                     BenefitType = b.BenefitType,
                                     DefaultValue = b.DefaultValue,
                                     RemainingValue = b.RemainingValue,
                                     CreatedAt = b.CreatedAt,
                                     UpdatedAt = b.UpdatedAt
                                 })
                                 .ToList()
                },
                predicate: sc => sc.SubscriptionId == subscriptionId);

            if (contract == null)
                throw new Exception("Subscription contract not found.");

            return contract;
        }

    }
}
