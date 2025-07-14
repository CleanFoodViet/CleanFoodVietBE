using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.ServiceFeatureDTOs;
using CleanFoodVietAPI.Application.DTOs.ServicePackageDTOs;
using CleanFoodVietAPI.Application.Exceptions;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using CleanFoodVietAPI.Data.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CleanFoodVietAPI.Data.Enums.ServiceFeatureEnums;
using CleanFoodVietAPI.Data.Enums.ServicePackageEnums;

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
        var repo = _unitOfWork.GetRepository<ServicePackage>();

        // call the GetAsync overload with include:
        var packageEntity = await repo.GetAsync(
            predicate: sp => sp.ServicePackageId == packageId,
            include: q => q
                .Include(sp => sp.ServicePackageFeatures)
                 .ThenInclude(psf => psf.ServiceFeature));

        if (packageEntity == null)
            throw new NotFoundException("Service package not found.");

        var dto = _mapper.Map<ServicePackageDTO>(packageEntity);
        dto.Features ??= new List<ServiceFeatureDTO>();
        return dto;
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

            // Load all features by the provided IDs.
            var activeFeatures = await featureRepo.GetListAsync(
                predicate: f => createDto.FeatureIds.Contains(f.ServiceFeatureId)
            );

            // Validate that we found as many features as provided.
            if (activeFeatures.Count() != createDto.FeatureIds.Count)
            {
                throw new Exception("Some provided features do not exist.");
            }

            // Validate that each feature is ACTIVE.
            foreach (var feature in activeFeatures)
            {
                if (!feature.Status.Equals("ACTIVE", StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception($"Feature '{feature.ServiceFeatureName}' is not active.");
                }
            }

            // Validate that no duplicate feature actions exist.
            var duplicateActionGroup = activeFeatures
                .GroupBy(f => f.Action)
                .FirstOrDefault(g => g.Count() > 1);

            if (duplicateActionGroup != null)
            {
                throw new Exception($"Duplicate feature action '{duplicateActionGroup.Key}' is not allowed in a service package.");
            }

            // Map the create DTO to the ServicePackage entity.
            var newPackage = _mapper.Map<ServicePackage>(createDto);
            newPackage.ServicePackageId = Ulid.NewUlid();
            newPackage.Status = "ACTIVE";
            newPackage.CreatedAt = DateTime.UtcNow;
            newPackage.UpdatedAt = DateTime.UtcNow;

            // Build the join collection from the active features.
            newPackage.ServicePackageFeatures = activeFeatures.Select(f => new PackageServiceFeature
            {
                PackageServiceFeatureId = Ulid.NewUlid(),
                ServicePackageId = newPackage.ServicePackageId,
                ServiceFeatureId = f.ServiceFeatureId
            }).ToList();

            // Insert the new package and commit changes.
            await _unitOfWork.GetRepository<ServicePackage>().InsertAsync(newPackage);
            var isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess)
            {
                throw new Exception("Error occurred while creating the Service Package.");
            }

            var packageDto = _mapper.Map<ServicePackageDTO>(newPackage);
            return packageDto;
        }

        // Update an existing service package (only PackageName and Description allowed)
        public async Task<ServicePackageDTO> UpdateServicePackageAsync(Ulid packageId, UpdateServicePackageDTO updateDto)
        {
            var repository = _unitOfWork.GetRepository<ServicePackage>();

            // Retrieve the existing package.
            var packageEntity = await repository.GetAsync<ServicePackage>(
                selector: sp => sp,
                predicate: sp => sp.ServicePackageId == packageId);

            if (packageEntity == null)
            {
                throw new Exception("Service Package not found.");
            }

            // Only update allowed fields.
            if (!string.IsNullOrWhiteSpace(updateDto.PackageName))
            {
                packageEntity.PackageName = updateDto.PackageName;
            }
            packageEntity.Description = updateDto.Description; // allow update to null if needed

            // Update the UpdatedAt timestamp.
            packageEntity.UpdatedAt = DateTime.UtcNow;

            // Do the update (remove await if repository.UpdateAsync returns void)
            repository.UpdateAsync(packageEntity);

            var isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess)
            {
                throw new Exception("Error occurred while updating the Service Package.");
            }

            return _mapper.Map<ServicePackageDTO>(packageEntity);
        }


        // Disable a service package by setting its status to INACTIVE
        public async Task<ServicePackageDTO> DisableServicePackageAsync(Ulid packageId)
        {
            var repo = _unitOfWork.GetRepository<ServicePackage>();

            // 1) Eagerly load the join table + child feature
            var packageEntity = await repo.GetAsync(
                predicate: sp => sp.ServicePackageId == packageId,
                include: q => q
                    .Include(sp => sp.ServicePackageFeatures)
                     .ThenInclude(psf => psf.ServiceFeature)
            );

            // 2) Not found → domain exception
            if (packageEntity == null)
                throw new NotFoundException($"Service package {packageId} not found.");

            // 3) Soft‐delete
            packageEntity.Status = ServicePackageStatusEnums.INACTIVE.ToString();
            packageEntity.UpdatedAt = DateTime.UtcNow;

            repo.UpdateAsync(packageEntity);                      // void
            var success = await _unitOfWork.CommitAsync() > 0;
            if (!success)
                throw new DomainValidationException(
                    "Error occurred while disabling the Service Package.");

            // 4) Map & return (DTO.Features is now populated)
            return _mapper.Map<ServicePackageDTO>(packageEntity);
        }


        // Activate a service package if all its features are active
        public async Task<ServicePackageDTO> ActivateServicePackageAsync(Ulid packageId)
        {
            var repo = _unitOfWork.GetRepository<ServicePackage>();

            // 1) Load the package AND its features
            var packageEntity = await repo.GetAsync(
                predicate: sp => sp.ServicePackageId == packageId,
                include: q => q
                    .Include(sp => sp.ServicePackageFeatures)
                     .ThenInclude(psf => psf.ServiceFeature)
            );

            if (packageEntity == null)
                throw new NotFoundException($"Service package {packageId} not found.");

            // 2) Validate that there *are* features
            var features = packageEntity.ServicePackageFeatures;
            if (features == null || !features.Any())
                throw new DomainValidationException("Cannot activate the package because it has no features assigned.");

            // 3) Validate all features are ACTIVE
            var allActive = features.All(psf =>
                psf.ServiceFeature != null &&
                psf.ServiceFeature.Status == ServiceFeatureStatusEnum.ACTIVE.ToString());

            if (!allActive)
                throw new DomainValidationException(
                    "Cannot activate the package because at least one feature is not active.");

            // 4) Flip status, stamp time, update & commit
            packageEntity.Status = ServicePackageStatusEnums.ACTIVE.ToString();
            packageEntity.UpdatedAt = DateTime.UtcNow;

            repo.UpdateAsync(packageEntity);    // void call
            await _unitOfWork.CommitAsync();    // this you await

            // 5) Map & return
            return _mapper.Map<ServicePackageDTO>(packageEntity);
        }

    }
}
