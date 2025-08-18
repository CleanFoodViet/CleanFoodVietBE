namespace CleanFoodVietAPI.Application.DTOs.ProductCaertificateDTOs
{
    public record ProductCertificateDTO
    {
        public string CertificateName { get; set; } = null!;
        public string IssuingOrganization { get; set; } = null!;
        public string CertificateNumber { get; set; } = null!;
        public DateTime IssuedDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string ImageUrl { get; set; } = null!;
    }
}
