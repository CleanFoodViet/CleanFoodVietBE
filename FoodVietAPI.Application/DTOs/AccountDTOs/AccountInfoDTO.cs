using CleanFoodVietAPI.Data.Entities;
using System.Linq.Expressions;

namespace CleanFoodVietAPI.Application.DTOs.AccountDTOs
{
    public class AccountInfoDTO
    {
        public Ulid AccountId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public AccountInfoDTO() { }

        public AccountInfoDTO(Ulid accountId, string name, string email, string phoneNumber)
        {
            AccountId = accountId;
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public static Expression<Func<Account, AccountInfoDTO>> Projection =>
            acct => new AccountInfoDTO(
                acct.AccountId,
                acct.Name!,
                acct.Email!,
                acct.PhoneNumber!
            );
    }
}
