using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.ServiceFeatureDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Enums.ServiceFeatureEnums;
using CleanFoodVietAPI.Data.Enums.ServicePackageEnums;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CleanFoodVietAPI.Application.Services.Implements
{
    public class ServiceFeatureService : BaseService<ServiceFeatureService>, IServiceFeatureService
    {
        public ServiceFeatureService(
            IUnitOfWork<CleanFoodVietDbContext> unitOfWork,
            ILogger<ServiceFeatureService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Existing GetServiceFeatureList and CreateServiceFeature are here...
        public async Task<IPaginate<ServiceFeatureDTO>> GetServiceFeatureList(
            int page,
            int size,
            string? filterField = null,
            string? filterValue = null,
            string? sortField = null,
            string? sortOrder = "asc",
            string? search = null)
        {
            var specification = new ServiceFeatureSpecification(filterField, filterValue, sortField, sortOrder, search);

            var featuresPage = await _unitOfWork.GetRepository<ServiceFeature>()
                .GetPagingListAsync(
                    spec: specification,
                    selector: sf => new ServiceFeatureDTO(
                        sf.ServiceFeatureId,
                        sf.ServiceFeatureName,
                        sf.Description,
                        sf.DefaultValue,   // now an int
                        sf.Action,
                        sf.Status),
                    page: page,
                    size: size);

            return featuresPage;
        }

        public async Task<ServiceFeatureDTO> GetServiceFeatureDetailAsync(Ulid id)
        {
            var repository = _unitOfWork.GetRepository<ServiceFeature>();

            // Retrieve the service feature entity by ID.
            var featureEntity = await repository.GetAsync(
                selector: sf => sf,
                predicate: sf => sf.ServiceFeatureId == id);

            if (featureEntity == null)
            {
                throw new Exception("Service feature not found.");
            }

            // Map the entity to a DTO.
            var featureDto = _mapper.Map<ServiceFeatureDTO>(featureEntity);
            return featureDto;
        }

        public async Task<ServiceFeatureDTO> CreateServiceFeature(CreateServiceFeatureDTO createDto)
        {
            var newFeature = _mapper.Map<ServiceFeature>(createDto);
            newFeature.Status = ServiceFeatureStatusEnum.ACTIVE.ToString();
            await _unitOfWork.GetRepository<ServiceFeature>().InsertAsync(newFeature);
            var isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess)
            {
                throw new Exception("Error occurred while creating the Service Feature.");
            }
            var resultDto = _mapper.Map<ServiceFeatureDTO>(newFeature);
            return resultDto;
        }

        // New update method using PATCH
        public async Task<ServiceFeatureDTO> UpdateServiceFeature(Ulid id, UpdateServiceFeatureDTO updateDto)
        {
            // Retrieve the existing ServiceFeature entity by id.
            var repository = _unitOfWork.GetRepository<ServiceFeature>();
            var existingFeature = await repository.GetAsync<ServiceFeature>(
                selector: x => x,
                predicate: x => x.ServiceFeatureId == id);

            if (existingFeature == null)
            {
                throw new Exception("Service Feature not found.");
            }

            // Validate: ensure the Action value remains unchanged.
            if (updateDto.Action.ToString() != existingFeature.Action)
            {
                throw new Exception("Action cannot be updated after creation.");
            }

            // Update allowed fields
            if (!string.IsNullOrEmpty(updateDto.ServiceFeatureName))
            {
                existingFeature.ServiceFeatureName = updateDto.ServiceFeatureName;
            }

            if (!string.IsNullOrEmpty(updateDto.Description))
            {
                existingFeature.Description = updateDto.Description;
            }

            if (!string.IsNullOrEmpty(updateDto.Status))
            {
                // If a soft-delete is desired, we expect "DISABLE" (case-insensitive).
                if (updateDto.Status.Equals("DISABLE", StringComparison.OrdinalIgnoreCase))
                {
                    existingFeature.Status = ServiceFeatureStatusEnum.INACTIVE.ToString();
                }
                else
                {
                    try
                    {
                        existingFeature.Status = Enum.Parse<ServiceFeatureStatusEnum>(updateDto.Status, true).ToString();
                    }
                    catch
                    {
                        throw new Exception("Invalid status value provided.");
                    }
                }
            }

            // Save the updates (remove await if UpdateAsync returns void)
            repository.UpdateAsync(existingFeature);

            var isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess)
            {
                throw new Exception("Error occurred while updating the Service Feature.");
            }

            var updatedDto = _mapper.Map<ServiceFeatureDTO>(existingFeature);
            return updatedDto;
        }

        /// <summary>
        /// Soft-deletes (disables) a service feature after checking that no active subscription
        /// (i.e. ServicePackage subscription record) is currently using it.
        /// </summary>
        public async Task<ServiceFeatureDTO> SoftDeleteServiceFeature(Ulid id)
        {
            // Retrieve the service feature by its ID.
            var repository = _unitOfWork.GetRepository<ServiceFeature>();
            var existingFeature = await repository.GetAsync<ServiceFeature>(
                selector: x => x,
                predicate: x => x.ServiceFeatureId == id);

            if (existingFeature == null)
            {
                throw new Exception("Service feature not found.");
            }

            // Check whether this service feature is used by any active "subscriptions" 
            // (which are actually ServicePackage records linked via SubscriptionContract).
            var psfRepository = _unitOfWork.GetRepository<PackageServiceFeature>();
            var packageFeatures = await psfRepository.GetListAsync(predicate: x => x.ServiceFeatureId == id);

            if (packageFeatures.Any())
            {
                // Extract the distinct ServicePackageIds associated with this feature.
                var packageIds = packageFeatures.Select(x => x.ServicePackageId).Distinct().ToList();

                // Query SubscriptionContract for any active subscription using these ServicePackages.
                var subscriptionRepository = _unitOfWork.GetRepository<SubscriptionContract>();
                var activeSubscriptions = await subscriptionRepository.GetListAsync(
                    predicate: sc => packageIds.Contains(sc.ServicePackageId)
                                      && sc.Status.Equals(ServicePackageStatusEnums.ACTIVE.ToString(), System.StringComparison.OrdinalIgnoreCase));

                if (activeSubscriptions.Any())
                {
                    throw new Exception("Cannot disable service feature because it is currently used by active subscriptions.");
                }
            }

            // No active usage was found – perform soft-delete by updating the status.
            existingFeature.Status = ServiceFeatureStatusEnum.INACTIVE.ToString();
            repository.UpdateAsync(existingFeature);

            var isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess)
            {
                throw new Exception("Error occurred while disabling the service feature.");
            }

            var updatedDto = _mapper.Map<ServiceFeatureDTO>(existingFeature);
            return updatedDto;
        }
    }
}

