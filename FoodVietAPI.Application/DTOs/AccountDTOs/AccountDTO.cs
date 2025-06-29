using CleanFoodVietAPI.Application.DTOs.AddressDTOs;
using CleanFoodVietAPI.Application.DTOs.CertificateDTOs;

namespace CleanFoodVietAPI.Application.DTOs.AccountDTOs
{
    public record AccountDTO
    (
        Ulid AccountId,
        string Name,
        string Email,
        string PhoneNumber,
        string Gender,
        string Avatar,
        string Status,
        bool IsVerified,
        DateTime UpdatedAt,
        string RoleName,
        List<CertificateDTO>? Certificates,
        List<GetAddressDTO>? Addresses
    );
};

