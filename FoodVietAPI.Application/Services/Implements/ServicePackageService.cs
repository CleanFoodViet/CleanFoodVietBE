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
            // 1) Build your spec (including any Includes you need)
            var spec = new ServicePackageSpecification(
                filterField, filterValue, sortField, sortOrder, search);

            return await _unitOfWork
                .GetRepository<ServicePackage>()
                .GetPagingListAsync(
                    spec: spec,
                    selector: ServicePackageDTO.Projection,
                    page: page,
                    size: size,
                    include: query => query
                        .Include(sp => sp.ServicePackageFeatures)
                          .ThenInclude(psf => psf.ServiceFeature)
                );
        }


        // View package details with features
        public async Task<ServicePackageDTO> GetServicePackageDetailAsync(string id)
        {
            // Parse & validate
            if (string.IsNullOrWhiteSpace(id) || !Ulid.TryParse(id, out var packageId))
                throw new DomainValidationException($"Invalid ServicePackage id: '{id}'.");

            var repo = _unitOfWork.GetRepository<ServicePackage>();

            // Eager‐load the join → ServicePackageFeatures → ServiceFeature
            var packageEntity = await repo.GetAsync(
                predicate: sp => sp.ServicePackageId == packageId,
                include: q => q
                    .Include(sp => sp.ServicePackageFeatures)
                     .ThenInclude(psf => psf.ServiceFeature)
            );

            if (packageEntity == null)
                throw new NotFoundException($"Service package '{id}' not found.");

            var dto = _mapper.Map<ServicePackageDTO>(packageEntity);
            dto.Features ??= new List<ServiceFeatureDTO>();
            return dto;
        }

        // Create a new service package with features
        public async Task<ServicePackageDTO> CreateServicePackageAsync(CreateServicePackageDTO createDto)
        {
            // 0) Validate incoming DTO
            if (createDto.FeatureIds == null || !createDto.FeatureIds.Any())
                throw new DomainValidationException("At least one feature must be selected.");

            // 1) Load & validate features
            var featureRepo = _unitOfWork.GetRepository<ServiceFeature>();
            var activeFeatures = await featureRepo.GetListAsync(
                predicate: f => createDto.FeatureIds.Contains(f.ServiceFeatureId));

            if (activeFeatures.Count() != createDto.FeatureIds.Count)
                throw new DomainValidationException("Some provided features do not exist.");

            foreach (var f in activeFeatures)
            {
                if (!string.Equals(f.Status, ServiceFeatureStatusEnum.ACTIVE.ToString(), StringComparison.OrdinalIgnoreCase))
                    throw new DomainValidationException($"Feature '{f.ServiceFeatureName}' is not active.");
            }

            // 2) Check duplicate actions
            var dup = activeFeatures
                .GroupBy(f => f.Action)
                .FirstOrDefault(g => g.Count() > 1);
            if (dup != null)
                throw new DomainValidationException($"Duplicate feature action '{dup.Key}' is not allowed.");

            // 3) Build the new package entity
            var now = DateTime.UtcNow;
            var newPackage = _mapper.Map<ServicePackage>(createDto);
            newPackage.ServicePackageId = Ulid.NewUlid();
            newPackage.Status = ServicePackageStatusEnums.ACTIVE.ToString();
            newPackage.CreatedAt = now;
            newPackage.UpdatedAt = now;
            newPackage.ServicePackageFeatures = activeFeatures
                .Select(f => new PackageServiceFeature
                {
                    PackageServiceFeatureId = Ulid.NewUlid(),
                    ServicePackageId = newPackage.ServicePackageId,
                    ServiceFeatureId = f.ServiceFeatureId
                })
                .ToList();

            // 4) Persist
            await _unitOfWork.GetRepository<ServicePackage>().InsertAsync(newPackage);
            var saved = await _unitOfWork.CommitAsync() > 0;
            if (!saved)
                throw new DomainValidationException("Error occurred while creating the Service Package.");

            // 5) Map to DTO, manually wire up the feature list
            var packageDto = _mapper.Map<ServicePackageDTO>(newPackage);

            // Assuming you have a mapping from ServiceFeature → ServiceFeatureDTO
            packageDto.Features = activeFeatures
                .Select(f => _mapper.Map<ServiceFeatureDTO>(f))
                .ToList();

            return packageDto;
        }


        // Update an existing service package (only PackageName and Description allowed)
        public async Task<ServicePackageDTO> UpdateServicePackageAsync(UpdateServicePackageDTO updateDto)
        {
            // 0) Validate & parse the incoming PackageId
            if (string.IsNullOrWhiteSpace(updateDto.PackageId)
                || !Ulid.TryParse(updateDto.PackageId, out var packageId))
            {
                throw new DomainValidationException($"Invalid ServicePackage id: '{updateDto.PackageId}'.");
            }

            var repo = _unitOfWork.GetRepository<ServicePackage>();

            // 1) Load the existing package with its features
            var packageEntity = await repo.GetAsync(
                predicate: sp => sp.ServicePackageId == packageId,
                include: q => q
                    .Include(sp => sp.ServicePackageFeatures)
                     .ThenInclude(psf => psf.ServiceFeature)
            );

            if (packageEntity == null)
                throw new NotFoundException($"Service package '{updateDto.PackageId}' not found.");

            // 2) Update only allowed fields
            if (!string.IsNullOrWhiteSpace(updateDto.PackageName))
                packageEntity.PackageName = updateDto.PackageName;

            // allow description to be set to null
            packageEntity.Description = updateDto.Description;
            packageEntity.UpdatedAt = DateTime.UtcNow;

            // 3) Persist changes
            repo.UpdateAsync(packageEntity);  // schedules the update
            var saved = await _unitOfWork.CommitAsync() > 0;
            if (!saved)
                throw new DomainValidationException("Error occurred while updating the Service Package.");

            // 4) Map to DTO (Features are already loaded)
            var dto = _mapper.Map<ServicePackageDTO>(packageEntity);
            dto.Features ??= new List<ServiceFeatureDTO>();
            return dto;
        }


        // Disable a service package by setting its status to INACTIVE
        public async Task<ServicePackageDTO> DisableServicePackageAsync(string id)
        {
            // Parse & validate
            if (string.IsNullOrWhiteSpace(id) || !Ulid.TryParse(id, out var packageId))
                throw new DomainValidationException($"Invalid ServicePackage id: '{id}'.");

            var repo = _unitOfWork.GetRepository<ServicePackage>();

            // Eager‐load the features
            var packageEntity = await repo.GetAsync(
                predicate: sp => sp.ServicePackageId == packageId,
                include: q => q
                    .Include(sp => sp.ServicePackageFeatures)
                     .ThenInclude(psf => psf.ServiceFeature)
            );

            if (packageEntity == null)
                throw new NotFoundException($"Service package '{id}' not found.");

            // Soft‐delete
            packageEntity.Status = ServicePackageStatusEnums.INACTIVE.ToString();
            packageEntity.UpdatedAt = DateTime.UtcNow;

            repo.UpdateAsync(packageEntity);
            var success = await _unitOfWork.CommitAsync() > 0;
            if (!success)
                throw new DomainValidationException(
                    "Error occurred while disabling the Service Package.");

            return _mapper.Map<ServicePackageDTO>(packageEntity);
        }


        // Activate a service package if all its features are active
        public async Task<ServicePackageDTO> ActivateServicePackageAsync(string id)
        {
            // Parse & validate
            if (string.IsNullOrWhiteSpace(id) || !Ulid.TryParse(id, out var packageId))
                throw new DomainValidationException($"Invalid ServicePackage id: '{id}'.");

            var repo = _unitOfWork.GetRepository<ServicePackage>();

            // Eager‐load the features
            var packageEntity = await repo.GetAsync(
                predicate: sp => sp.ServicePackageId == packageId,
                include: q => q
                    .Include(sp => sp.ServicePackageFeatures)
                     .ThenInclude(psf => psf.ServiceFeature)
            );

            if (packageEntity == null)
                throw new NotFoundException($"Service package '{id}' not found.");

            var features = packageEntity.ServicePackageFeatures;
            if (features == null || !features.Any())
                throw new DomainValidationException(
                    "Cannot activate the package because it has no features assigned.");

            var anyInactive = features.Any(psf =>
                psf.ServiceFeature == null ||
                psf.ServiceFeature.Status != ServiceFeatureStatusEnum.ACTIVE.ToString());

            if (anyInactive)
                throw new DomainValidationException(
                    "Cannot activate the package because at least one feature is not active.");

            // Activate
            packageEntity.Status = ServicePackageStatusEnums.ACTIVE.ToString();
            packageEntity.UpdatedAt = DateTime.UtcNow;

            repo.UpdateAsync(packageEntity);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<ServicePackageDTO>(packageEntity);
        }
    }
}
