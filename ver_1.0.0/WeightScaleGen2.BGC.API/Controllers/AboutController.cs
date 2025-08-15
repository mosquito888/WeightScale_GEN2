using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeightScaleGen2.BGC.API.APIServices;

namespace WeightScaleGen2.BGC.API.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AboutController : Controller
    {
        private readonly ILogger<AboutController> _logger;
        private readonly AboutAPIService _aboutAPIService;

        public AboutController(ILogger<AboutController> logger, AboutAPIService aboutAPIService)
        {
            _logger = logger;
            _aboutAPIService = aboutAPIService;
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        public string Get()
        {
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            _logger.LogInformation("Api v1.0");
            return $"v1.0.0 Your IP is : {remoteIpAddress}";
        }

        [HttpGet("TestConnection")]
        public ActionResult TestConnection()
        {
            var res = _aboutAPIService.GetConnectionDb().Result.data == true ? "Connected" : "Not Connected";
            return Ok(res);
        }

        [HttpGet("GetConnection")]
        public ActionResult GetConnectionDb()
        {
            var res = _aboutAPIService.GetConnectionDb().Result;
            return Ok(res);
        }
    }
}
