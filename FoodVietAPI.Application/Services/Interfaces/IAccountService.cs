using CleanFoodVietAPI.Application.DTOs.AccountDTO;
using CleanFoodVietAPI.Application.DTOs.AuthDTO;
using CleanFoodVietAPI.Data.Paginate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface IAccountService
    {
        #region Account Functions
        Task<IPaginate<AccountDTO>> GetAccountList(int page, int size);
        Task<AccountDTO> GetAccountInformation(string accountId);
        Task UpdateAccountStatus(string accountId, string status);
        #endregion

        #region Authentication Functions
        Task<AuthDTO> Login(LoginDTO loginData);
        Task<AuthDTO> Register(RegisterDTO registerData);
        #endregion
    }
}
