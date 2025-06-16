using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Data.Entities
{
    public class Certificate
    {
        [Key]
        public Ulid CertificateId { get; set; }
        public string Name { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string IssuingAuthority { get; set; } = null!;
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Status { get; set; } = null!;
        public Ulid GardenerId { get; set; }

        public virtual Account Account { get; set; } = null!;
    }
}
