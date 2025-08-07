using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeightScaleGen2.BGC.API.APIServices;
using WeightScaleGen2.BGC.Models.ViewModels.WeightIn;

namespace WeightScaleGen2.BGC.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WeightInController : Controller
    {
        private readonly ILogger<WeightInController> _logger;
        private readonly WeightInAPIService _weightInAPIService;

        public WeightInController(ILogger<WeightInController> logger, WeightInAPIService weightInAPIService)
        {
            _logger = logger;
            _weightInAPIService = weightInAPIService;
        }

        [HttpPost("PostWeightInInfo")]
        public ActionResult PostWeightInInfo(ResultGetWeightInInfoViewModel param)
        {
            var res = _weightInAPIService.PostWeightInInfo(param).Result;
            return Ok(res);
        }

        [HttpPut("PutWeightInInfo")]
        public ActionResult PutWeightOutInfo(ResultGetWeightInInfoViewModel param)
        {
            var res = _weightInAPIService.PutWeightInInfo(param).Result;
            return Ok(res);
        }

        [HttpPut("PutWeightInStatus")]
        public ActionResult PutWeightOutStatus(ResultGetWeightInInfoViewModel param)
        {
            var res = _weightInAPIService.PutWeightInStatus(param).Result;
            return Ok(res);
        }

        [HttpGet("GetSearchWeightInListData")]
        public ActionResult GetSearchWeightInListData(ParamSearchWeightInViewModel param)
        {
            var res = _weightInAPIService.GetSearchListWeightIn(param).Result;
            return Ok(res);
        }

        [HttpGet("GetWeightInInfo")]
        public ActionResult GetWeightInInfo(ParamWeightInInfo param)
        {
            var res = _weightInAPIService.GetWeightInInfo(param).Result;
            return Ok(res);
        }

        [HttpGet("GetWeightInInfoByCarLicense")]
        public ActionResult GetWeightInInfoByCarLicense(ParamWeightInInfo param)
        {
            var res = _weightInAPIService.GetWeightInInfoByCarLicense(param).Result;
            return Ok(res);
        }

    }
}
