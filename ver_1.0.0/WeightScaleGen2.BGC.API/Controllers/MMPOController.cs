using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeightScaleGen2.BGC.API.APIServices;
using WeightScaleGen2.BGC.Models.ViewModels.MMPO;

namespace WeightScaleGen2.BGC.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MMPOController : Controller
    {
        private readonly ILogger<MMPOController> _logger;
        private readonly MMPOAPIService _MMPOAPIService;

        public MMPOController(ILogger<MMPOController> logger, MMPOAPIService MMPOAPIService)
        {
            _logger = logger;
            _MMPOAPIService = MMPOAPIService;
        }

        [HttpGet("GetSearchListMMPO")]
        public ActionResult GetSearchListMMPO(ParamSearchMMPOViewModel param)
        {
            var res = _MMPOAPIService.GetSearchListMMPO(param).Result;
            return Ok(res);
        }

        [HttpGet("GetSearchMMPOCheckQtyPending")]
        public ActionResult GetSearchMMPOCheckQtyPending(ParamSearchMMPOQtyPendingViewModel param)
        {
            var res = _MMPOAPIService.GetSearchMMPOCheckQtyPending(param).Result;
            return Ok(res);
        }

        [HttpPost("UpdateMMPOSapToDocumentPO")]
        public ActionResult UpdateMMPOSapToDocumentPO(ParamSearchMMPOViewModel param)
        {
            var res = _MMPOAPIService.UpdateMMPOSapToDocumentPO(param).Result;
            return Ok(res);
        }

        [HttpPost("UpdateMMPOSapToDocumentPOData")]
        public ActionResult UpdateMMPOSapToDocumentPOData()
        {
            var res = _MMPOAPIService.UpdateMMPOSapToDocumentPOData().Result;
            return Ok(res);
        }

    }
}
