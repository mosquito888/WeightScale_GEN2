using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeightScaleGen2.BGC.API.APIServices;
using WeightScaleGen2.BGC.Models.ViewModels.Sender;

namespace WeightScaleGen2.BGC.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SenderController : Controller
    {
        private readonly ILogger<SenderController> _logger;
        private readonly SenderAPIService _senderAPIService;

        public SenderController(ILogger<SenderController> logger, SenderAPIService senderAPIService)
        {
            _logger = logger;
            _senderAPIService = senderAPIService;
        }

        [HttpGet("GetSenderListData")]
        public ActionResult GetSenderListData()
        {
            var res = _senderAPIService.GetSenderListData().Result;
            return Ok(res);
        }

        [HttpGet("GetSearchSenderListData")]
        public ActionResult GetSearchEmployeeListData(ParamSearchSenderViewModel param)
        {
            var res = _senderAPIService.GetSearchListSender(param).Result;
            return Ok(res);
        }

        [HttpGet("GetSenderInfo")]
        public ActionResult GetSenderInfo(ParamSenderInfo param)
        {
            var res = _senderAPIService.GetSenderInfo(param).Result;
            return Ok(res);
        }

        [HttpPost("PostSenderInfo")]
        public ActionResult PostSenderInfo(ResultGetSenderInfoViewModel param)
        {
            var res = _senderAPIService.PostSenderInfo(param).Result;
            return Ok(res);
        }

        [HttpPut("PutSenderInfo")]
        public ActionResult PutSenderInfo(ResultGetSenderInfoViewModel param)
        {
            var res = _senderAPIService.PutSenderInfo(param).Result;
            return Ok(res);
        }

        [HttpDelete("DeleteItem")]
        public ActionResult DeleteItem(ResultGetSenderInfoViewModel param)
        {
            var res = _senderAPIService.DeleteInfo(param).Result;
            return Ok(res);
        }
    }
}
