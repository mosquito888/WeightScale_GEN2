using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeightScaleGen2.BGC.API.APIServices;

namespace WeightScaleGen2.BGC.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SystemController : Controller
    {
        private readonly ILogger<SystemController> _logger;
        private readonly SystemAPIService _systemAPIService;

        public SystemController(ILogger<SystemController> logger, SystemAPIService systemAPIService)
        {
            _logger = logger;
            _systemAPIService = systemAPIService;
        }

        [HttpGet("GetSystemListData")]
        public ActionResult GetSystemListData()
        {
            var res = _systemAPIService.GetListSystem().Result;
            return Ok(res);
        }
    }
}
