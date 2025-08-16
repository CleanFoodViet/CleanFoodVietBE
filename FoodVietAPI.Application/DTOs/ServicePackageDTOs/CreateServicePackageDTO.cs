using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Application.DTOs.ServicePackageDTOs
{
    public class CreateServicePackageDTO
    {
        public string PackageName { get; set; } = null!;
        public string? Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be 1 or greater.")]
        public decimal Price { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Duration must be 1 or greater.")]
        public int Duration { get; set; }

        // List of feature IDs that the admin wants to include.
        public List<Ulid> FeatureIds { get; set; } = new();
    }
}

