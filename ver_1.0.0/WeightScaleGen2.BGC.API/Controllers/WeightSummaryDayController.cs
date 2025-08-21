using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeightScaleGen2.BGC.API.APIServices;
using WeightScaleGen2.BGC.Models.ViewModels.WeightSummaryDay;

namespace WeightScaleGen2.BGC.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WeightSummaryDayController : Controller
    {
        private readonly ILogger<WeightSummaryDayController> _logger;
        private readonly WeightSummaryDayAPIService _weightSummaryDayAPIService;

        public WeightSummaryDayController(ILogger<WeightSummaryDayController> logger, WeightSummaryDayAPIService weightSummaryDayAPIService)
        {
            _logger = logger;
            _weightSummaryDayAPIService = weightSummaryDayAPIService;
        }

        [HttpGet("GetSearchListWeightSummaryDay")]
        public ActionResult GetSearchListWeightSummaryDay(ParamSearchWeightSummaryDayViewModel param)
        {
            var res = _weightSummaryDayAPIService.GetSearchListWeightSummaryDay(param).Result;
            return Ok(res);
        }

    }
}
