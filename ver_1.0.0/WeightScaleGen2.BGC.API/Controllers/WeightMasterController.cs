using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeightScaleGen2.BGC.API.APIServices;

namespace WeightScaleGen2.BGC.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WeightMasterController : Controller
    {
        private readonly ILogger<WeightMasterController> _logger;
        private readonly WeightMasterAPIService _weightMasterAPIService;

        public WeightMasterController(ILogger<WeightMasterController> logger, WeightMasterAPIService weightMasterAPIService)
        {
            _logger = logger;
            _weightMasterAPIService = weightMasterAPIService;
        }

        [HttpDelete("CopyDeleteWeightMaster")]
        public ActionResult CopyDeleteWeightMaster()
        {
            var res = _weightMasterAPIService.CopyDeleteWeightMaster().Result;
            return Ok(res);
        }

    }
}
