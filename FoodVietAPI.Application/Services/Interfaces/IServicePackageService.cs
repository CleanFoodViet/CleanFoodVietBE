using CleanFoodVietAPI.Application.DTOs.ServicePackageDTOs;
using CleanFoodVietAPI.Data.Paginate;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface IServicePackageService
    {
        Task<IPaginate<ServicePackageDTO>> GetServicePackagesWithFeaturesAsync(
            int page,
            int size,
            string? filterField = null,
            string? filterValue = null,
            string? sortField = null,
            string? sortOrder = "asc",
            string? search = null);

        Task<ServicePackageDTO> GetServicePackageDetailAsync(Ulid packageId);

        Task<ServicePackageDTO> CreateServicePackageAsync(CreateServicePackageDTO createDto);

        Task<ServicePackageDTO> UpdateServicePackageAsync(Ulid packageId, UpdateServicePackageDTO updateDto);

        Task<ServicePackageDTO> DisableServicePackageAsync(Ulid packageId);

        Task<ServicePackageDTO> ActivateServicePackageAsync(Ulid packageId);
    }

}
