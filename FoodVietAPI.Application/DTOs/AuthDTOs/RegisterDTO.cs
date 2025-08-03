using CleanFoodVietAPI.Application.DTOs.AddressDTOs;
using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Application.DTOs.AuthDTOs
{
    public record RegisterDTO
    {
        [Required]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required] 
        public string? Email { get; set; } = string.Empty;  
        public string? Gender { get; set; }
        public string? Avatar { get; set; }
        public string Name { get; set; } = null!;
        //Address Data
        public string AddressLine { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Province { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string Country { get; set; } = null!;
    };

    public record GardenerRegisterDTO : RegisterDTO
    {
        //Certificate Data
        public string ImageUrl { get; set; } = null!;
        public string IssuingAuthority { get; set; } = null!;
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Status { get; set; } = null!;
    }
}
