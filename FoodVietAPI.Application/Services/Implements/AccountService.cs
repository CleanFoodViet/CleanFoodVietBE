using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.AuthDTO;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Application.Utils;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Enums.AccountEnums;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
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
        public async Task GetAccountList()
        {
            var accounts = await _unitOfWork.GetRepository<Account>().GetListAsync();
        }

        public async Task GetAccountInformation(string accountId)
        {
            var account = await _unitOfWork.GetRepository<Account>()
                .GetAsync(predicate: acc => acc.AccountId.ToString() == accountId);

            if (account == null) throw new BadHttpRequestException("Account is not found");
        }

        public async Task UpdateAccountStatus(string accountId, string status)
        {
            var account = await _unitOfWork.GetRepository<Account>()
                .GetAsync(predicate: acc => acc.AccountId.ToString() == accountId);

            if (account == null) throw new BadHttpRequestException("Account is not found");


            if (Enum.TryParse<AccountStatusEnum>(status.ToUpper(), out var result))
            {
                account.Status = result.ToString();
            }
            else
            {
                account.Status = AccountStatusEnum.INACTIVE.ToString();
            }
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
            await _unitOfWork.CommitAsync();

            var token = JwtUtil.GenerateJwtToken(newAccount);

            return new AuthDTO(token, newAccount.AccountId.ToString());
        }
        #endregion
    }
}
