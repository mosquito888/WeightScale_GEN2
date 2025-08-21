using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeightScaleGen2.BGC.API.APIServices;
using WeightScaleGen2.BGC.Models.ViewModels.WeightCompare;

namespace WeightScaleGen2.BGC.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WeightCompareController : Controller
    {
        private readonly ILogger<WeightCompareController> _logger;
        private readonly WeightCompareAPIService _weightCompareAPIService;

        public WeightCompareController(ILogger<WeightCompareController> logger, WeightCompareAPIService weightCompareAPIService)
        {
            _logger = logger;
            _weightCompareAPIService = weightCompareAPIService;
        }

        [HttpGet("GetSearchListWeightCompare")]
        public ActionResult GetSearchListWeightCompare(ParamSearchWeightCompareViewModel param)
        {
            var res = _weightCompareAPIService.GetSearchListWeightCompare(param).Result;
            return Ok(res);
        }

    }
}
