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
        public string Email { get; set; } = null!;
        public string? Gender { get; set; }
        public string? Avatar { get; set; }
    };

    public record GardenerRegisterDTO : RegisterDTO
    {
        //Certificate Data
        public string Name { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string IssuingAuthority { get; set; } = null!;
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Status { get; set; } = null!;

        //Address data
        [Required]
        public List<AddressDTO> Addresses { get; set; } = null!;
    }
}
