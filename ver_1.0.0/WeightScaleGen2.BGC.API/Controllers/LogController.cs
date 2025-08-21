using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeightScaleGen2.BGC.API.APIServices;
using WeightScaleGen2.BGC.Models.ViewModels.Log;

namespace WeightScaleGen2.BGC.API.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LogController : ControllerBase
    {
        private readonly ILogger<LogController> _logger;
        private readonly LogAPIService _logService;

        public LogController(ILogger<LogController> logger, LogAPIService logService)
        {
            _logger = logger;
            _logService = logService;
        }

        [HttpGet("GetSearchLogCriteria")]
        public ActionResult GetSearchLogCriteria()
        {
            var res = _logService.GetSearchLogCriteria().Result;
            return Ok(res);
        }

        [HttpPost("SearchLogData")]
        public ActionResult SearchLogData(ParamSearchLogViewModel param)
        {
            var res = _logService.SearchLogData(param).Result;
            return Ok(res);
        }
    }
}
