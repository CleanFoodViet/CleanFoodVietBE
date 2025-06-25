namespace CleanFoodVietAPI.Application.DTOs.AddressDTOs
{
    public record GetAddressDTO
    {
        public Ulid AddressId { get; set; }
        public string AddressLine { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Province { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string Country { get; set; } = null!;
    }
}
