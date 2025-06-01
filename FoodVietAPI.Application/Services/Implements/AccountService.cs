using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.AccountDTO;
using CleanFoodVietAPI.Application.DTOs.AuthDTO;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Application.Utils;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Enums.AccountEnums;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.Services.Implements
{
    public class AccountService : BaseService<AccountService>, IAccountService
    {
        public AccountService(IUnitOfWork<CleanFoodVietDbContext> unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, mapper, httpContextAccessor)
        {
        }

        #region Account Functions
        public async Task<IPaginate<GetAccountDTO>> GetAccountList(int page, int size)
        {
            var accountList = await _unitOfWork.GetRepository<Account>().GetPagingListAsync(
                include: acc => acc.Include(x => x.Role),
                selector: acc => new GetAccountDTO
                {
                    AccountId = acc.AccountId.ToString(),
                    PhoneNumber = acc.PhoneNumber,
                    Avatar = acc.Avatar,
                    Email = acc.Email,
                    Gender = acc.Gender,
                    IsVerified = acc.IsVerified,
                    Status = acc.Status,
                    UpdatedAt = acc.UpdatedAt,
                    RoleName = acc.Role.Name
                },
                page: page,
                size: size
                );

            return accountList;
        }

        public async Task<GetAccountDTO> GetAccountInformation(string accountId)
        {
            Ulid idValue = Ulid.Parse(accountId);
            var account = await _unitOfWork.GetRepository<Account>()
                .GetAsync(predicate: acc => acc.AccountId == idValue,
                          include: acc => acc.Include(x => x.Role),
                          selector: acc => new GetAccountDTO
                          {
                              AccountId = acc.AccountId.ToString(),
                              PhoneNumber = acc.PhoneNumber,
                              Avatar = acc.Avatar,
                              Email = acc.Email,
                              Gender = acc.Gender,
                              IsVerified = acc.IsVerified,
                              Status = acc.Status,
                              UpdatedAt = acc.UpdatedAt,
                              RoleName = acc.Role.Name
                          });

            if (account == null) throw new BadHttpRequestException("Account is not found");

            return account!;
        }

        public async Task UpdateAccountStatus(string accountId, string status)
        {
            Ulid idValue = Ulid.Parse(accountId);
            var account = await _unitOfWork.GetRepository<Account>()
                .GetAsync(predicate: acc => acc.AccountId == idValue);

            if (account == null) throw new BadHttpRequestException("Account is not found");


            if (Enum.TryParse<AccountStatusEnum>(status.ToUpper(), out var result))
            {
                account.Status = result.ToString();
            }
            else
            {
                account.Status = AccountStatusEnum.INACTIVE.ToString();
            }

            _unitOfWork.GetRepository<Account>().UpdateAsync(account);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occurs when updating account status");
        }
        #endregion

        #region Authentication Functions
        public async Task<AuthDTO> Login(LoginDTO loginData)
        {
            string hashedPassword = HashUtil.PasswordHash(loginData.Password);
            Account account = await _unitOfWork.GetRepository<Account>()
                                        .GetAsync(predicate: a => a.PhoneNumber == loginData.PhoneNumber &&
                                                                  a.Password == hashedPassword);
            if (account == null) throw new BadHttpRequestException("Incorrect Phone Number or Password");

            var token = JwtUtil.GenerateJwtToken(account);

            return new AuthDTO(token, account.AccountId.ToString());
        }

        public async Task<AuthDTO> Register(RegisterDTO registerData)
        {
            Account account = await _unitOfWork.GetRepository<Account>()
                                       .GetAsync(predicate: a => a.PhoneNumber == registerData.PhoneNumber ||
                                                                 a.Email == registerData.Email);
            if (account != null) throw new BadHttpRequestException("Phone Number or Email have already been used");

            Role role = await _unitOfWork.GetRepository<Role>().GetAsync(predicate: r => r.Name == AccountRoleEnum.RETAILER.ToString());

            var newAccount = _mapper.Map<Account>(registerData);
            newAccount.RoleId = role.RoleId;

            await _unitOfWork.GetRepository<Account>().InsertAsync(newAccount);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occurs when registering");

            var token = JwtUtil.GenerateJwtToken(newAccount);

            return new AuthDTO(token, newAccount.AccountId.ToString());
        }
        #endregion
    }
}
