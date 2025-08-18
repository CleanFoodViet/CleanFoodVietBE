using CleanFoodVietAPI.Data.Entities;
using System.Linq.Expressions;
using CleanFoodVietAPI.Application.DTOs.ServiceFeatureDTOs;

namespace CleanFoodVietAPI.Application.DTOs.ServicePackageDTOs
{
    public class ServicePackageDTO
    {
        public Ulid ServicePackageId { get; set; }
        public string PackageName { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<ServiceFeatureDTO> Features { get; set; }

        public ServicePackageDTO(
            Ulid servicePackageId,
            string packageName,
            string? description,
            decimal price,
            int duration,
            string status,
            DateTime createdAt,
            DateTime updatedAt,
            List<ServiceFeatureDTO> features)
        {
            ServicePackageId = servicePackageId;
            PackageName = packageName;
            Description = description;
            Price = price;
            Duration = duration;
            Status = status;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            Features = features;
        }

        public ServicePackageDTO() { }

        /// <summary>
        /// EF-Core can translate this into SQL whenever you do
        /// .Select(ServicePackageDTO.Projection)
        /// </summary>
        public static Expression<Func<ServicePackage, ServicePackageDTO>> Projection =>
            sp => new ServicePackageDTO(
                sp.ServicePackageId,
                sp.PackageName!,
                sp.Description,
                sp.Price,
                sp.Duration,
                sp.Status!,
                sp.CreatedAt,
                sp.UpdatedAt,
                sp.ServicePackageFeatures
                  .Select(psf => new ServiceFeatureDTO(
                      psf.ServiceFeature.ServiceFeatureId,
                      psf.ServiceFeature.ServiceFeatureName!,
                      psf.ServiceFeature.Description,
                      psf.ServiceFeature.DefaultValue,
                      psf.ServiceFeature.Action!,
                      psf.ServiceFeature.Status!))
                  .ToList());
    }
}
