namespace CleanFoodVietAPI.Application.DTOs.CertificateDTOs
{
    public record CertificateDTO
    {
        public string Name { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string IssuingAuthority { get; set; } = null!;
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Status { get; set; } = null!;
    }
}
