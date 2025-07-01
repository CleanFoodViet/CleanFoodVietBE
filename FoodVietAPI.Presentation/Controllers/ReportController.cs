using CleanFoodVietAPI.Application.DTOs.ReportDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class ReportController : BaseController<ReportController>
    {
        private readonly IReportService _reportService;

        public ReportController(ILogger<ReportController> logger, IReportService reportService) : base(logger)
        {
            _reportService = reportService;
        }

        [HttpGet(ApiEndpointConstant.Report.ReportsEndpoint)]
        [ProducesResponseType(typeof(IPaginate<ReportListDTO>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get User' Report for Admin")]
        public async Task<IActionResult> GetReportList(int page = 1, int size = 10)
        {
            var res = await _reportService.GetReportList(page, size);

            return Ok(res);
        }

        [HttpGet(ApiEndpointConstant.Report.ReportEndpoint)]
        [ProducesResponseType(typeof(ReportDTO), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get a Report Information")]
        public async Task<IActionResult> GetReportInformation(string id)
        {
            var res = await _reportService.GetReportInformation(id);

            return Ok(res);
        }

        [HttpPost(ApiEndpointConstant.Report.ReportsEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [SwaggerOperation(Summary = "Create a new Report to Admin")]
        public async Task<IActionResult> CreateReport([FromBody]CreateReportDTO request)
        {
            await _reportService.CreateReport(request);
            return StatusCode(StatusCodes.Status201Created, "Report create successfully");
        }

        [HttpPatch(ApiEndpointConstant.Report.ReportEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Admin Update a Report Status")]
        public async Task<IActionResult> UpdateReportStatus([FromRoute]string id, [FromQuery]string status)
        {
            await _reportService.UpdateReportStatus(id, status);
            return Ok("Update successfully");
        }
    }
}
