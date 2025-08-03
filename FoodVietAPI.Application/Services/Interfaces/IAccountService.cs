using CleanFoodVietAPI.Application.DTOs.AccountDTOs;
using CleanFoodVietAPI.Application.DTOs.AuthDTOs;
using CleanFoodVietAPI.Data.Paginate;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface IAccountService
    {
        #region Account Functions
        Task<IPaginate<AccountDTO>> GetRetailerAccountList(
            int page, int size,
            string? filterField = null,
            string? filterValue = null,
            string? sortField = null,
            string? sortOrder = "asc",
            string? search = null);
        Task<IPaginate<AccountDTO>> GetGardenerAccountList(
            int page, int size,
            string? filterField = null,
            string? filterValue = null,
            string? sortField = null,
            string? sortOrder = "asc",
            string? search = null);
        Task<AccountDTO> GetAccountInformation(string id);
        Task UpdateAccountStatus(string accountId, string status);
        Task UpdateProfile(string id, UpdateAccountDTO data);
        Task ChangePassword(ChangePasswordDto data);
        #endregion

        #region Authentication Functions
        Task<AuthDTO> Login(LoginDTO loginData);
        Task<RegisterResponse> Register(RegisterDTO registerData);
        Task<RegisterResponse> CreateAdmin(RegisterDTO registerData);
        Task<RegisterResponse> GardenerRegister(GardenerRegisterDTO registerData);
        #endregion
    }
}
