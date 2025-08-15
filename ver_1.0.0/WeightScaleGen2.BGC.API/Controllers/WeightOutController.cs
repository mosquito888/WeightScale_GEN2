using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeightScaleGen2.BGC.API.APIServices;
using WeightScaleGen2.BGC.Models.ViewModels.WeightOut;

namespace WeightScaleGen2.BGC.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WeightOutController : Controller
    {
        private readonly ILogger<WeightOutController> _logger;
        private readonly WeightOutAPIService _weightOutAPIService;

        public WeightOutController(ILogger<WeightOutController> logger, WeightOutAPIService weightOutAPIService)
        {
            _logger = logger;
            _weightOutAPIService = weightOutAPIService;
        }

        [HttpPost("PostWeightOutInfo")]
        public ActionResult PostWeightInInfo(ResultGetWeightOutInfoViewModel param)
        {
            var res = _weightOutAPIService.PostWeightOutInfo(param).Result;
            return Ok(res);
        }

        [HttpPut("PutWeightOutInfo")]
        public ActionResult PutWeightOutInfo(ResultGetWeightOutInfoViewModel param)
        {
            var res = _weightOutAPIService.PutWeightOutInfo(param).Result;
            return Ok(res);
        }

        [HttpPut("PutWeightOutStatus")]
        public ActionResult PutWeightOutStatus(ResultGetWeightOutInfoViewModel param)
        {
            var res = _weightOutAPIService.PutWeightOutStatus(param).Result;
            return Ok(res);
        }

        [HttpGet("GetSearchWeightOutListData")]
        public ActionResult GetSearchWeightOutListData(ParamSearchWeightOutViewModel param)
        {
            var res = _weightOutAPIService.GetSearchListWeightOut(param).Result;
            return Ok(res);
        }

        [HttpGet("GetWeightOutInfo")]
        public ActionResult GetWeightOutInfo(ParamWeightOutInfo param)
        {
            var res = _weightOutAPIService.GetWeightOutInfo(param).Result;
            return Ok(res);
        }

        [HttpGet("GetWeightOutInfoByCarLicense")]
        public ActionResult GetWeightOutInfoByCarLicense(ParamWeightOutInfo param)
        {
            var res = _weightOutAPIService.GetWeightOutInfoByCarLicense(param).Result;
            return Ok(res);
        }

    }
}
