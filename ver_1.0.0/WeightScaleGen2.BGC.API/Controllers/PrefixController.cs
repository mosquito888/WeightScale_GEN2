using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeightScaleGen2.BGC.API.APIServices;

namespace WeightScaleGen2.BGC.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PrefixController : Controller
    {
        private readonly ILogger<PrefixController> _logger;
        private readonly PrefixAPIService _refixAPIService;

        public PrefixController(ILogger<PrefixController> logger, PrefixAPIService refixAPIService)
        {
            _logger = logger;
            _refixAPIService = refixAPIService;
        }

        [HttpGet("GetRunningDocument")]
        public ActionResult GetRunningDocument(string docType, string compType, string plantType, string plantShortType)
        {
            var res = _refixAPIService.GetPrefixDoc(docType, compType, plantType, plantShortType).Result;
            return Ok(res);
        }
    }
}
