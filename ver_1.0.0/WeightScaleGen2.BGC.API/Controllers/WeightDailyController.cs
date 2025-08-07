using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeightScaleGen2.BGC.API.APIServices;
using WeightScaleGen2.BGC.Models.ViewModels.WeightDaily;

namespace WeightScaleGen2.BGC.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WeightDailyController : Controller
    {
        private readonly ILogger<WeightDailyController> _logger;
        private readonly WeightDailyAPIService _weightDailyAPIService;

        public WeightDailyController(ILogger<WeightDailyController> logger, WeightDailyAPIService weightDailyAPIService)
        {
            _logger = logger;
            _weightDailyAPIService = weightDailyAPIService;
        }

        [HttpGet("GetSearchListWeightDaily")]
        public ActionResult GetSearchListWeightDaily(ParamSearchWeightDailyViewModel param)
        {
            var res = _weightDailyAPIService.GetSearchListWeightDaily(param).Result;
            return Ok(res);
        }

    }
}
