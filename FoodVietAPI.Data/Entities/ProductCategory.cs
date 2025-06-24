using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Data.Entities
{
    public class ProductCategory
    {
        [Key]
        public Ulid ProductCategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public Ulid GardenerId { get; set; }

        public virtual Account Gardener { get; set; } = null!;
        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
