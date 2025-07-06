using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.AccountDTOs;
using CleanFoodVietAPI.Application.DTOs.AddressDTOs;
using CleanFoodVietAPI.Application.DTOs.AuthDTOs;
using CleanFoodVietAPI.Application.DTOs.CertificateDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Application.Specifications;
using CleanFoodVietAPI.Application.Utils;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Enums.AccountEnums;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanFoodVietAPI.Application.Services.Implements
{
    public class AccountService : BaseService<AccountService>, IAccountService
    {
        public AccountService(IUnitOfWork<CleanFoodVietDbContext> unitOfWork, ILogger<AccountService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        #region Account Functions
        public async Task<IPaginate<AccountDTO>> GetRetailerAccountList(
            int page, int size,
            string? filterField = null,
            string? filterValue = null,
            string? sortField = null,
            string? sortOrder = "asc",
            string? search = null)
        {
            AccountSpecification accountSpecification = new AccountSpecification(filterField, filterValue, sortField, sortOrder, search);

            var accountList = await _unitOfWork.GetRepository<Account>().GetPagingListAsync(
                include: acc => acc.Include(x => x.Role),
                predicate: acc => acc.Role.Name == AccountRoleEnum.RETAILER.ToString(),
                selector: acc => new AccountDTO(
                    acc.AccountId,
                    acc.Name,
                    acc.Email,
                    acc.PhoneNumber,
                    acc.Gender,
                    acc.Avatar,
                    acc.Status,
                    acc.IsVerified,
                    acc.UpdatedAt,
                    acc.Role.Name,
                    _mapper.Map<List<CertificateDTO>>(acc.Certificates.ToList()),
                    _mapper.Map<List<GetAddressDTO>>(acc.Addresses.ToList())),
                page: page, size: size,
                spec: accountSpecification);

            return accountList;
        }

        public async Task<IPaginate<AccountDTO>> GetGardenerAccountList(
            int page, int size,
            string? filterField = null,
            string? filterValue = null,
            string? sortField = null,
            string? sortOrder = "asc",
            string? search = null)
        {
            AccountSpecification accountSpecification = new AccountSpecification(filterField, filterValue, sortField, sortOrder, search);

            var accountList = await _unitOfWork.GetRepository<Account>().GetPagingListAsync(
                include: acc => acc.Include(x => x.Role),
                predicate: acc => acc.Role.Name == AccountRoleEnum.GARDENER.ToString(),
                selector: acc => new AccountDTO(
                    acc.AccountId,
                    acc.Name,
                    acc.Email,
                    acc.PhoneNumber,
                    acc.Gender,
                    acc.Avatar,
                    acc.Status,
                    acc.IsVerified,
                    acc.UpdatedAt,
                    acc.Role.Name,
                    _mapper.Map<List<CertificateDTO>>(acc.Certificates.ToList()),
                    _mapper.Map<List<GetAddressDTO>>(acc.Addresses.ToList())),
        page: page, size: size, 
                spec: accountSpecification);

            return accountList;
        }

        public async Task<AccountDTO> GetAccountInformation(string id)
        {
            Ulid accountId = Ulid.Parse(id);
            var account = await _unitOfWork.GetRepository<Account>()
                .GetAsync(predicate: acc => acc.AccountId == accountId,
                          include: acc => acc.Include(x => x.Role),
                          selector: acc => new AccountDTO(
                                acc.AccountId,
                                acc.Name,
                                acc.Email,
                                acc.PhoneNumber,
                                acc.Gender,
                                acc.Avatar,
                                acc.Status,
                                acc.IsVerified,
                                acc.UpdatedAt,
                                acc.Role.Name,
                                _mapper.Map<List<CertificateDTO>>(acc.Certificates.ToList()),
                                _mapper.Map<List<GetAddressDTO>>(acc.Addresses.ToList())));

            if (account == null) throw new BadHttpRequestException("Account is not found");

            return account!;
        }

        public async Task UpdateAccountStatus(string id, string status)
        {
            Ulid accountId = Ulid.Parse(id);
            var account = await _unitOfWork.GetRepository<Account>()
                .GetAsync(predicate: acc => acc.AccountId == accountId);

            if (account == null) throw new BadHttpRequestException("Account is not found");

            if (Enum.TryParse<AccountStatusEnum>(status.ToUpper(), out var result))
            {
                account.Status = result.ToString();
            }
            else
            {
                account.Status = AccountStatusEnum.INACTIVE.ToString();
            }

            account.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.GetRepository<Account>().UpdateAsync(account);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occurs when updating account status (DB query error)");
        }

        public async Task UpdateProfile(string id, UpdateAccountDTO data)
        {
            Ulid accountId = Ulid.Parse(id);
            var account = await _unitOfWork.GetRepository<Account>()
                .GetAsync(predicate: acc => acc.AccountId == accountId);

            if (account == null) throw new BadHttpRequestException("Account is not found");

            _mapper.Map(data, account);
            _unitOfWork.GetRepository<Account>().UpdateAsync(account);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occurs when updating profile (DB query error)");
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

        public async Task<RegisterResponse> Register(RegisterDTO registerData)
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

            var response = _mapper.Map<RegisterResponse>(newAccount);
            response.Role = role.Name;

            return response;
        }

        public async Task<RegisterResponse> CreateAdmin(RegisterDTO registerData)
        {
            Account account = await _unitOfWork.GetRepository<Account>()
                                       .GetAsync(predicate: a => a.PhoneNumber == registerData.PhoneNumber ||
                                                                 a.Email == registerData.Email);
            if (account != null) throw new BadHttpRequestException("Phone Number or Email have already been used");

            Role role = await _unitOfWork.GetRepository<Role>().GetAsync(predicate: r => r.Name == AccountRoleEnum.ADMIN.ToString());

            var newAccount = _mapper.Map<Account>(registerData);
            newAccount.RoleId = role.RoleId;

            await _unitOfWork.GetRepository<Account>().InsertAsync(newAccount);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occurs when registering");

            var response = _mapper.Map<RegisterResponse>(newAccount);
            response.Role = role.Name;

            return response;
        }

        public async Task<RegisterResponse> GardenerRegister(GardenerRegisterDTO registerData)
        {
            Account account = await _unitOfWork.GetRepository<Account>()
                                       .GetAsync(predicate: a => a.PhoneNumber == registerData.PhoneNumber ||
                                                                 a.Email == registerData.Email);
            if (account != null) throw new BadHttpRequestException("Phone Number or Email have already been used");

            Role role = await _unitOfWork.GetRepository<Role>().GetAsync(predicate: r => r.Name == AccountRoleEnum.GARDENER.ToString());

            var newAccount = _mapper.Map<Account>(registerData);
            newAccount.RoleId = role.RoleId;

            Certificate certificate = _mapper.Map<Certificate>(registerData);
            certificate.GardenerId = newAccount.AccountId;

            List<Address> addresses = _mapper.Map<List<Address>>(registerData);
            addresses.ForEach(x => x.AccountId = newAccount.AccountId);

            await _unitOfWork.GetRepository<Account>().InsertAsync(newAccount);
            await _unitOfWork.GetRepository<Certificate>().InsertAsync(certificate);
            await _unitOfWork.GetRepository<Address>().InsertRangeAsync(addresses);

            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occurs when registering");

            var response = _mapper.Map<RegisterResponse>(newAccount);
            response.Role = role.Name;

            return response;
        }
        #endregion
    }
}
