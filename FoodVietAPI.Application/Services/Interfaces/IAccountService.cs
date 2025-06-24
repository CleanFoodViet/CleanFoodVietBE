using CleanFoodVietAPI.Application.DTOs.AccountDTOs;
using CleanFoodVietAPI.Application.DTOs.AuthDTOs;
using CleanFoodVietAPI.Data.Paginate;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface IAccountService
    {
        #region Account Functions
        Task<IPaginate<AccountDTO>> GetRetailerAccountList(int page, int size);
        Task<IPaginate<AccountDTO>> GetGardenerAccountList(int page, int size);
        Task<AccountDTO> GetAccountInformation(string id);
        Task UpdateAccountStatus(string accountId, string status);
        Task UpdateProfile(string id, UpdateAccountDTO data);
        #endregion

        #region Authentication Functions
        Task<AuthDTO> Login(LoginDTO loginData);
        Task<AuthDTO> Register(RegisterDTO registerData);
        Task<AuthDTO> GardenerRegister(GardenerRegisterDTO registerData);
        #endregion
    }
}
