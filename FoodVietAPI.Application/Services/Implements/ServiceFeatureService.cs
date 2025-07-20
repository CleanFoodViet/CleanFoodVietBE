using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.ServiceFeatureDTOs;
using CleanFoodVietAPI.Application.Exceptions;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Enums.ServiceFeatureEnums;
using CleanFoodVietAPI.Data.Enums.ServicePackageEnums;
using CleanFoodVietAPI.Data.Exceptions;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;

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

        public async Task<ServiceFeatureDTO> GetServiceFeatureDetailAsync(string id)
        {
            // 0) Validate & parse the incoming id
            if (string.IsNullOrWhiteSpace(id) || !Ulid.TryParse(id, out var featureId))
                throw new DomainValidationException($"Invalid ServiceFeature id: '{id}'.");

            // 1) Load the feature
            var repo = _unitOfWork.GetRepository<ServiceFeature>();
            var entity = await repo.GetAsync(
                predicate: sf => sf.ServiceFeatureId == featureId);

            // 2) Not found?
            if (entity == null)
                throw new NotFoundException($"Service feature '{id}' not found.");

            // 3) Map & return
            return _mapper.Map<ServiceFeatureDTO>(entity);
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
        public async Task<ServiceFeatureDTO> UpdateServiceFeature(
    Ulid id,
    UpdateServiceFeatureDTO dto)
        {
            // 1) load the feature
            var repo = _unitOfWork.GetRepository<ServiceFeature>();
            var existing = await repo.GetAsync(
                selector: x => x,
                predicate: x => x.ServiceFeatureId == id);

            if (existing == null)
                throw new KeyNotFoundException($"Feature '{id}' not found.");

            // 2) forbid disabling if used in any active package
            if (!string.IsNullOrWhiteSpace(dto.Status) &&
                dto.Status.Equals("INACTIVE", StringComparison.OrdinalIgnoreCase))
            {
                var inUse = (await _unitOfWork
                    .GetRepository<PackageServiceFeature>()
                    .GetListAsync(
                        predicate: psf =>
                            psf.ServiceFeatureId == id &&
                            psf.ServicePackage.Status == "ACTIVE"))
                    .Any();

                if (inUse)
                    throw new DeletionRestrictedException(
                        "Cannot disable a feature that is in an active package.");
            }

            // 3) apply allowed updates
            if (!string.IsNullOrWhiteSpace(dto.ServiceFeatureName))
                existing.ServiceFeatureName = dto.ServiceFeatureName;

            if (!string.IsNullOrWhiteSpace(dto.Description))
                existing.Description = dto.Description;

            if (!string.IsNullOrWhiteSpace(dto.Status))
            {
                try
                {
                    var statusEnum = Enum.Parse<ServiceFeatureStatusEnum>(
                        dto.Status, ignoreCase: true);
                    existing.Status = statusEnum.ToString();
                }
                catch
                {
                    throw new UpdateRestrictedException(
                        $"'{dto.Status}' is not a valid status.");
                }
            }

            // 4) tell EF to track the change
            repo.UpdateAsync(existing);

            // 5) commit the transaction
            await _unitOfWork.CommitAsync();

            // 6) return the updated DTO
            return _mapper.Map<ServiceFeatureDTO>(existing);
        }


        /// <summary>
        /// Soft-deletes (disables) a service feature after checking that no active subscription
        /// (i.e. ServicePackage subscription record) is currently using it.
        /// </summary>
        public async Task<ServiceFeatureDTO> SoftDeleteServiceFeature(string id)        
        {
            // 0) Validate & parse the incoming string into a Ulid
            if (string.IsNullOrWhiteSpace(id) || !Ulid.TryParse(id, out var featureId))
                throw new DomainValidationException($"Invalid ServiceFeature id: '{id}'.");

            // 1) Load the feature
            var featureRepo = _unitOfWork.GetRepository<ServiceFeature>();
            var feature = await featureRepo.GetAsync(predicate: f => f.ServiceFeatureId == featureId);

            if (feature == null)
                throw new NotFoundException($"Service feature '{id}' not found.");

            // 2) Find all package–feature links for this feature
            var psfRepo = _unitOfWork.GetRepository<PackageServiceFeature>();
            var packageFeatures = await psfRepo.GetListAsync(
                predicate: psf => psf.ServiceFeatureId == featureId);

            var packageIds = packageFeatures
                .Select(x => x.ServicePackageId)
                .Distinct()
                .ToList();

            // 3) If that feature is part of any package, ensure none are ACTIVE
            if (packageIds.Any())
            {
                var pkgRepo = _unitOfWork.GetRepository<ServicePackage>();
                var activePkgs = await pkgRepo.GetListAsync(
                    predicate: sp =>
                        packageIds.Contains(sp.ServicePackageId)
                        && sp.Status == ServicePackageStatusEnums.ACTIVE.ToString());

                if (activePkgs.Any())
                    throw new DomainValidationException(
                        "Cannot disable feature; it’s part of an active service package.");
            }

            // 4) Soft-delete: flip status → INACTIVE, schedule update, commit
            feature.Status = ServiceFeatureStatusEnum.INACTIVE.ToString();
            featureRepo.UpdateAsync(feature);    // void
            await _unitOfWork.CommitAsync();     // commit

            // 5) Return the updated DTO
            return _mapper.Map<ServiceFeatureDTO>(feature);
        }
    }
}

