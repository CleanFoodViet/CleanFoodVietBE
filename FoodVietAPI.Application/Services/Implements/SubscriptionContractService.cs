using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.Gardener;
using CleanFoodVietAPI.Application.DTOs.SubscriptionContractBenefitDTOs;
using CleanFoodVietAPI.Application.DTOs.SubscriptionContractDTOs;
using CleanFoodVietAPI.Application.Exceptions;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Application.Specifications;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Enums.SubscriptionContract;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanFoodVietAPI.Application.Services.Implements
{
    public class SubscriptionContractService : BaseService<SubscriptionContractService>, ISubscriptionContractService
    {
        public SubscriptionContractService(IUnitOfWork<CleanFoodVietDbContext> unitOfWork, ILogger<SubscriptionContractService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<IPaginate<SubscriptionContractDTO>> GetContractsAsync(
            int page, int size,
            string? filterField, string? filterValue,
            string? sortField, string? sortOrder,
            string? search)
        {
            var spec = new SubscriptionContractSpecification(
                filterField, filterValue, sortField, sortOrder, search);

            var repo = _unitOfWork.GetRepository<SubscriptionContract>();
            return await repo.GetPagingListAsync(
                spec: spec,
                selector: sc => new SubscriptionContractDTO
                {
                    SubscriptionId = sc.SubscriptionId,
                    GardenerId = sc.GardenerId,

                    // map gardener info
                    GardenerName = sc.Account.Name,
                    GardenerEmail = sc.Account.Email,
                    GardenerPhone = sc.Account.PhoneNumber,

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
                page: page,
                size: size);
        }

        public async Task<SubscriptionContractDTO> GetContractDetailAsync(Ulid subscriptionId)
        {
            var repo = _unitOfWork.GetRepository<SubscriptionContract>();
            var dto = await repo.GetAsync(
                selector: sc => new SubscriptionContractDTO
                {
                    SubscriptionId = sc.SubscriptionId,
                    GardenerId = sc.GardenerId,
                    GardenerName = sc.Account.Name,
                    GardenerEmail = sc.Account.Email,
                    GardenerPhone = sc.Account.PhoneNumber,
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

            if (dto == null)
                throw new KeyNotFoundException("Subscription contract not found.");

            return dto;
        }

        public async Task<IPaginate<ContractHistoryDTO>> GetContractHistoryAsync(string gardenerId, int page, int size)
        {
            if (!Ulid.TryParse(gardenerId, out var gId))
                throw new DomainValidationException($"Invalid Gardener id: '{gardenerId}'.");

            var contracts = await _unitOfWork.GetRepository<SubscriptionContract>()
                .GetPagingListAsync(
                    predicate: c => c.GardenerId == gId,
                    include: q => q
                        .Include(c => c.Account)
                        .Include(c => c.ServicePackage),
                    selector: c => new ContractHistoryDTO(
                        c.SubscriptionId,
                        c.Status,
                        c.SubscriptionType,
                        c.Account.PhoneNumber,
                        c.ServicePackage.PackageName,
                        c.ServicePackage.Price),
                    page: page, size: size
                );

            return contracts;
        }

        public async Task<SubscriptionContractDetailDTO?> GetActiveContractAsync(string gardenerId)
        {
            if (!Ulid.TryParse(gardenerId, out var gId))
                throw new DomainValidationException($"Invalid gardener id: '{gardenerId}'.");

            var now = DateTime.UtcNow;
            var repo = _unitOfWork.GetRepository<SubscriptionContract>();

            var list = await repo.GetListAsync(
                predicate: sc =>
                    sc.GardenerId == gId &&
                    sc.Status == "ACTIVE" &&
                    sc.StartDate <= now &&
                    sc.EndDate > now,

                // <-- Eager-load the package → features → feature
                include: q => q
                    .Include(sc => sc.ServicePackage)
                        .ThenInclude(sp => sp.ServicePackageFeatures)
                            .ThenInclude(psf => psf.ServiceFeature),

                selector: SubscriptionContractDetailDTO.Projection);

            return list.SingleOrDefault();
        }

        public async Task<CurrentSubscriptionContractBenefitDTO> GetCurrentSubscriptionContractBenefit(string gardenerId)
        {
            Ulid gardenerID = Ulid.Parse(gardenerId);
            var gardener = await _unitOfWork.GetRepository<Account>()
                .GetAsync(predicate: acc => acc.AccountId == gardenerID);
            if (gardener == null) throw new BadHttpRequestException("Gardener is not found"); ;

            var subscriptionContract = await _unitOfWork.GetRepository<SubscriptionContract>()
                .GetAsync(
                    include: sc => sc.Include(x => x.SubscriptionContractBenefits),
                    predicate: sc => sc.GardenerId == gardenerID && sc.Status == SubscriptionContractStatusEnum.ACTIVE.ToString()
                );

            var currentBenefit = subscriptionContract.SubscriptionContractBenefits
                .Where(scb => scb.BenefitType == "POST_QUOTA").First();

            return _mapper.Map<CurrentSubscriptionContractBenefitDTO>(currentBenefit);
        }
    }
}
