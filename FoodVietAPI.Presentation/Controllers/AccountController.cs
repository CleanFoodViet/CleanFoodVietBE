using CleanFoodVietAPI.Application.DTOs.AccountDTOs;
using CleanFoodVietAPI.Application.DTOs.AddressDTOs;
using CleanFoodVietAPI.Application.DTOs.AppointmentDTOs;
using CleanFoodVietAPI.Application.DTOs.CertificateDTOs;
using CleanFoodVietAPI.Application.DTOs.NotificationDTOs;
using CleanFoodVietAPI.Application.DTOs.PostDTOs;
using CleanFoodVietAPI.Application.Services.Implements;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Mvc;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class AccountController : BaseController<AccountController>
    {
        private readonly IAccountService _accountService;
        private readonly IAddressService _addressService;
        private readonly ICertificateService _certificateService;
        private readonly IAppointmentService _appointmentService;
        private readonly INotificationService _notificationService;
        private readonly IPostService _postService;

        public AccountController(ILogger<AccountController> logger, IAccountService accountService, IAddressService addressService,
            ICertificateService certificateService, IAppointmentService appointmentService, INotificationService notificationService,
            IPostService postService) 
            : base(logger)
        {
            _accountService = accountService;
            _addressService = addressService;
            _certificateService = certificateService;
            _appointmentService = appointmentService;
            _notificationService = notificationService;
            _postService = postService;
        }

        #region Account APIs Controller
        [HttpGet(ApiEndpointConstant.Account.RetailerAccountsEndpoint)]
        [ProducesResponseType(typeof(IPaginate<AccountDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRetailerAccountList([FromQuery]int page = 1, [FromQuery]int size = 10)
        {
            var res = await _accountService.GetRetailerAccountList(page, size);
            return Ok(res);
        }

        [HttpGet(ApiEndpointConstant.Account.GardenerAccountsEndpoint)]
        [ProducesResponseType(typeof(IPaginate<AccountDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetGardenerAccountList([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var res = await _accountService.GetGardenerAccountList(page, size);
            return Ok(res);
        }

        [HttpPatch(ApiEndpointConstant.Account.AccountStatuEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAccountStatus([FromRoute] string id, [FromQuery] string status)
        {
            await _accountService.UpdateAccountStatus(id, status);
            return Ok("Update status successfully");
        }

        [HttpGet(ApiEndpointConstant.Account.AccountEndpoint)]
        [ProducesResponseType(typeof(AccountDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAccountInformation([FromRoute] string id)
        {
            var res = await _accountService.GetAccountInformation(id);
            return Ok(res);
        }

        [HttpPatch(ApiEndpointConstant.Account.AccountProfileEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateProfile([FromRoute]string id, [FromBody]UpdateAccountDTO data)
        {
            await _accountService.UpdateProfile(id, data);
            return Ok("Update profile successfully");
        }
        #endregion

        #region Address APIs Controller
        [HttpGet(ApiEndpointConstant.Account.AccountAddressesEndpoint)]
        [ProducesResponseType(typeof(List<GetAddressDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAccountAddressList([FromRoute]string id)
        {
            var res = await _addressService.GetAccountAddressList(id);
            return Ok(res);
        }

        [HttpGet(ApiEndpointConstant.Account.AccountAddressEndpoint)]
        [ProducesResponseType(typeof(GetAddressDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAccountAddressDetail([FromRoute]string id, [FromRoute]string addressId)
        {
            var res = await _addressService.GetAccountAddressDetail(id, addressId);
            return Ok(res);
        }

        [HttpPost(ApiEndpointConstant.Account.AccountAddressesEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateAddress([FromRoute]string id, [FromBody]AddressDTO request)
        {
            await _addressService.CreateAddresss(request, id);
            return StatusCode(StatusCodes.Status201Created, "Create address successfully");
        }

        [HttpPatch(ApiEndpointConstant.Account.AccountAddressEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAddress([FromRoute] string id, [FromRoute] string addressId, [FromBody]AddressDTO request)
        {
            await _addressService.UpdateAddress(request, addressId, id);
            return Ok("Update address successfully");
        }

        [HttpDelete(ApiEndpointConstant.Account.AccountAddressEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAddress([FromRoute] string id, [FromRoute] string addressId)
        {
            await _addressService.DeletAddress(addressId, id);
            return Ok("Delete address successfully");
        }
        #endregion

        #region Gardener Certification APIs Controller
        [HttpGet(ApiEndpointConstant.Account.GardenerCertificatesEndpoint)]
        [ProducesResponseType(typeof(List<AddressDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetGardenerCertificateList([FromRoute] string id)
        {
            var res = await _certificateService.GetGardenerCertificateList(id);
            return Ok(res);
        }

        [HttpGet(ApiEndpointConstant.Account.GardenerCertificateEndpoint)]
        [ProducesResponseType(typeof(AddressDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetGardenerCertificateDetail([FromRoute] string id, [FromRoute] string certificateId)
        {
            var res = await _certificateService.GetGardenerCertificateDetail(id, certificateId);
            return Ok(res);
        }

        [HttpPost(ApiEndpointConstant.Account.GardenerCertificatesEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateCertificate([FromRoute] string id, [FromBody] CertificateDTO request)
        {
            await _certificateService.CreateCertificate(request, id);
            return StatusCode(StatusCodes.Status201Created, "Create address successfully");
        }

        [HttpPatch(ApiEndpointConstant.Account.GardenerCertificateEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateCertificate([FromRoute] string id, [FromRoute] string certificateId, [FromBody] CertificateDTO request)
        {
            await _certificateService.UpdateCertificate(request, certificateId, id);
            return Ok("Update address successfully");
        }

        [HttpDelete(ApiEndpointConstant.Account.GardenerCertificateEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteCertificate([FromRoute] string id, [FromRoute] string certificateId)
        {
            await _certificateService.DeleteCertificate(certificateId, id);
            return Ok("Delete address successfully");
        }
        #endregion

        //Appointment api controller
        [HttpGet(ApiEndpointConstant.Account.AccountAppointmentEndpoint)]
        [ProducesResponseType(typeof(List<AppointmentListDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAccountAppointmentList([FromRoute] string accountId)
        {
            var res = await _appointmentService.GetAppointmentList(accountId);
            return Ok(res);
        }

        //Notificaiton api controller
        [HttpGet(ApiEndpointConstant.Account.AccountNotificationEndpoint)]
        [ProducesResponseType(typeof(List<NotificationDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAccountNotification([FromRoute] string id)
        {
            var res = await _notificationService.GetNotificationList(id);
            return Ok(res);
        }

        //Retailer's Favorite post APIs controller
        [HttpGet(ApiEndpointConstant.Account.RetailerFavPostEndpoint)]
        [ProducesResponseType(typeof(List<PostListDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFavoritePostList([FromRoute] string retailerId)
        {
            var res = await _postService.GetRetailerFavoritePost(retailerId);
            return Ok(res);
        }

        [HttpPost(ApiEndpointConstant.Account.RetailerFavPostEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddPostToFavorite([FromRoute]string id, [FromRoute]string postId)
        {
            await _postService.AddPostToFavorite(postId, id);
            return Ok("Add post to favorite successfully");
        }

        [HttpDelete(ApiEndpointConstant.Account.RetailerFavPostEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> RemovePostFromFavorite([FromRoute] string id, [FromRoute] string postId)
        {
            await _postService.RemovePostFromFavorite(postId, id);
            return Ok("Remove Post from favorite successfully");
        }
    }
}