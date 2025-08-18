using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Data.Entities
{
    public class ProductCertificate
    {
        [Key]
        public Ulid ProductCertificateId { get; set; }
        public Ulid ProductId { get; set; }
        public string CertificateName { get; set; } = null!;
        public string IssuingOrganization { get; set; } = null!;
        public string CertificateNumber { get; set; } = null!;
        public DateTime IssuedDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string ImageUrl { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Product Product{ get; set; } = null!;
    }
}
