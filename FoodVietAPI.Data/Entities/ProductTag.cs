using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Data.Entities
{
    public class ProductTag
    {
        [Key]
        public Ulid ProductTagId { get; set; }
        public Ulid ProductId { get; set; }
        public Ulid GardenerId { get; set; }
        public string TagName { get; set; } = null!;

        public virtual Product Product { get; set; } = null!;
        public virtual Account Gardener { get; set; } = null!;
    }
}
