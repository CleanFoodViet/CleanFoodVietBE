namespace CleanFoodVietAPI.Application.DTOs.AddressDTOs
{
    public record AddressDTO
    {
        public Ulid AddressId { get; set; }
        public string AddressLine1 { get; set; } = null!;
        public string? AddressLine2 { get; set; }
        public string City { get; set; } = null!;
        public string Province { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string Country { get; set; } = null!;
    }
}
