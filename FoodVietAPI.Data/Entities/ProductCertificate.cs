using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string CertificateFileUrl { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


    }
}
