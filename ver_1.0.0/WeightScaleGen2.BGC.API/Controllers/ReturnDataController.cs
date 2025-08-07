using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeightScaleGen2.BGC.API.APIServices;
using WeightScaleGen2.BGC.Models.ViewModels.ReturnData;

namespace WeightScaleGen2.BGC.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ReturnDataController : Controller
    {
        private readonly ILogger<ReturnDataController> _logger;
        private readonly ReturnDataAPIService _returnDataAPIService;

        public ReturnDataController(ILogger<ReturnDataController> logger, ReturnDataAPIService returnDataAPIService)
        {
            _logger = logger;
            _returnDataAPIService = returnDataAPIService;
        }

        [HttpGet("GetSearchListReturnData")]
        public ActionResult GetSearchListReturnData(ParamSearchReturnDataViewModel param)
        {
            var res = _returnDataAPIService.GetSearchListReturnData(param).Result;
            return Ok(res);
        }

        [HttpPost("PostDataToSAP")]
        public ActionResult PostDataToSAP(ParamSearchReturnDataViewModel param)
        {
            var res = _returnDataAPIService.PostDataToSAP(param).Result;
            return Ok(res);
        }

    }
}
