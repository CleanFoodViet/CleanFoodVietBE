using CleanFoodVietAPI.Application.DTOs.AddressDTOs;
using CleanFoodVietAPI.Application.DTOs.CertificateDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class CertificateController : BaseController<CertificateController>
    {
        private readonly ICertificateService _certificateService;
        public CertificateController(ILogger<CertificateController> logger, ICertificateService certificateService) : base(logger)
        {
            _certificateService = certificateService;
        }

        [HttpGet(ApiEndpointConstant.Certificate.GardenerCertificatesEndpoint)]
        [ProducesResponseType(typeof(List<AddressDTO>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all certificates of Gardener")]
        public async Task<IActionResult> GetGardenerCertificateList([FromRoute] string id)
        {
            var res = await _certificateService.GetGardenerCertificateList(id);
            return Ok(res);
        }

        [HttpGet(ApiEndpointConstant.Certificate.CertificateEndpoint)]
        [ProducesResponseType(typeof(AddressDTO), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get Gardener certificate's information")]
        public async Task<IActionResult> GetGardenerCertificateDetail([FromRoute] string id)
        {
            var res = await _certificateService.GetGardenerCertificateDetail(id);
            return Ok(res);
        }

        [HttpPost(ApiEndpointConstant.Certificate.GardenerCertificatesEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Create a Certificate for Gardener")]
        public async Task<IActionResult> CreateCertificate([FromRoute] string id, [FromBody] CreateCertificateDTO request)
        {
            await _certificateService.CreateCertificate(request, id);
            return StatusCode(StatusCodes.Status201Created, "Create address successfully");
        }

        [HttpPatch(ApiEndpointConstant.Certificate.CertificateEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Update the Certificate's information (Not using this api - Policy rules violated)")]
        public async Task<IActionResult> UpdateCertificate([FromRoute] string id, [FromBody] UpdateCertificateDTO request)
        {
            await _certificateService.UpdateCertificate(request, id);
            return Ok("Update address successfully");
        }

        [HttpDelete(ApiEndpointConstant.Certificate.CertificateEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Delete a Gardener's Certificate")]
        public async Task<IActionResult> DeleteCertificate([FromRoute] string id)
        {
            await _certificateService.DeleteCertificate(id);
            return Ok("Delete address successfully");
        }
    }
}
