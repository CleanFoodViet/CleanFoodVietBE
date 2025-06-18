using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.ServiceFeatureDTOs;
using CleanFoodVietAPI.Application.DTOs.ServicePackageDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using CleanFoodVietAPI.Data.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.Services.Implements
{
    public class ServicePackageService : BaseService<ServicePackageService>, IServicePackageService
    {
        public ServicePackageService(
            IUnitOfWork<CleanFoodVietDbContext> unitOfWork,
            ILogger<ServicePackageService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<IPaginate<ServicePackageDTO>> GetServicePackagesWithFeaturesAsync(
            int page,
            int size,
            string? filterField = null,
            string? filterValue = null,
            string? sortField = null,
            string? sortOrder = "asc",
            string? search = null)
        {
            // Create a specification that supports filtering, search, and sorting.
            var specification = new ServicePackageSpecification(filterField, filterValue, sortField, sortOrder, search);

            var repository = _unitOfWork.GetRepository<ServicePackage>();

            var packagesPage = await repository.GetPagingListAsync(
                spec: specification,
                selector: sp => new ServicePackageDTO(
                    sp.ServicePackageId,
                    sp.PackageName,
                    sp.Description,
                    sp.Price,
                    sp.Duration,
                    sp.Status,
                    sp.CreatedAt,
                    sp.UpdatedAt,
                    sp.ServicePackageFeatures
                      .Select(psf => new ServiceFeatureDTO(
                          psf.ServiceFeature.ServiceFeatureId,
                          psf.ServiceFeature.ServiceFeatureName,
                          psf.ServiceFeature.Description,
                          psf.ServiceFeature.DefaultValue,
                          psf.ServiceFeature.Action,
                          psf.ServiceFeature.Status))
                      .ToList()),
                page: page,
                size: size);

            return packagesPage;
        }

        // View package details with features
        public async Task<ServicePackageDTO> GetServicePackageDetailAsync(Ulid packageId)
        {
            var repository = _unitOfWork.GetRepository<ServicePackage>();

            // Retrieve the service package entity with its related features.
            var packageEntity = await repository.GetAsync(
                selector: sp => sp, // retrieve the full entity (assumes navigation properties are loaded or included)
                predicate: sp => sp.ServicePackageId == packageId);

            if (packageEntity == null)
            {
                throw new Exception("Service Package not found.");
            }

            // Map the entity to a DTO. Your AutoMapper configuration will take care of
            // populating the Features list (using the mapping defined in your ServicePackageMapper).
            var packageDto = _mapper.Map<ServicePackageDTO>(packageEntity);

            // Ensure that the Features property is not null.
            packageDto.Features = packageDto.Features ?? new List<ServiceFeatureDTO>();

            return packageDto;
        }


        // Create a new service package with features
        public async Task<ServicePackageDTO> CreateServicePackageAsync(CreateServicePackageDTO createDto)
        {
            // Validate that at least one feature is provided.
            if (createDto.FeatureIds == null || !createDto.FeatureIds.Any())
            {
                throw new Exception("At least one feature must be selected.");
            }

            // Get the repository for ServiceFeature to validate each provided feature.
            var featureRepo = _unitOfWork.GetRepository<ServiceFeature>();

            // Load all features by provided IDs.
            var activeFeatures = await featureRepo.GetListAsync(
                predicate: f => createDto.FeatureIds.Contains(f.ServiceFeatureId));

            // Validate that we have as many features as provided.
            if (activeFeatures.Count() != createDto.FeatureIds.Count)
            {
                throw new Exception("Some provided features do not exist.");
            }

            // Validate each feature to be ACTIVE.
            foreach (var feature in activeFeatures)
            {
                if (!feature.Status.Equals("ACTIVE", StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception($"Feature '{feature.ServiceFeatureName}' is not active.");
                }
            }

            // Map DTO to entity.
            var newPackage = _mapper.Map<ServicePackage>(createDto);
            newPackage.ServicePackageId = Ulid.NewUlid();
            newPackage.Status = "ACTIVE";
            newPackage.CreatedAt = DateTime.UtcNow;
            newPackage.UpdatedAt = DateTime.UtcNow;

            // Build the join collection:
            newPackage.ServicePackageFeatures = activeFeatures.Select(f => new PackageServiceFeature
            {
                PackageServiceFeatureId = Ulid.NewUlid(),
                ServicePackageId = newPackage.ServicePackageId,
                ServiceFeatureId = f.ServiceFeatureId
            }).ToList();

            // Insert and commit.
            await _unitOfWork.GetRepository<ServicePackage>().InsertAsync(newPackage);
            var isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess)
            {
                throw new Exception("Error occurred while creating the Service Package.");
            }

            var packageDto = _mapper.Map<ServicePackageDTO>(newPackage);
            return packageDto;
        }

    }
}
