using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.AuthDTO;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Application.Utils;
using CleanFoodVietAPI.Data.Entities;
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


        #endregion
    }
}
