using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeightScaleGen2.BGC.API.APIServices;

namespace WeightScaleGen2.BGC.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly DashboardAPIService _dashboardAPIService;

        public DashboardController(ILogger<DashboardController> logger, DashboardAPIService dashboardAPIService)
        {
            _logger = logger;
            _dashboardAPIService = dashboardAPIService;
        }

        [HttpGet("GetSearchListDashboardSummary")]
        public ActionResult GetSearchListDashboardSummary()
        {
            var res = _dashboardAPIService.GetSearchListDashboardSummary().Result;
            return Ok(res);
        }

        [HttpGet("GetSearchListDashboardHistorySummary")]
        public ActionResult GetSearchListDashboardHistorySummary()
        {
            var res = _dashboardAPIService.GetSearchListDashboardHistorySummary().Result;
            return Ok(res);
        }

    }
}
