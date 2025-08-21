using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeightScaleGen2.BGC.API.APIServices;
using WeightScaleGen2.BGC.Models.ViewModels.Master;

namespace WeightScaleGen2.BGC.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MasterController : Controller
    {
        private readonly ILogger<MasterController> _logger;
        private readonly MasterAPIService _masterAPIService;

        public MasterController(ILogger<MasterController> logger, MasterAPIService masterAPIService)
        {
            _logger = logger;
            _masterAPIService = masterAPIService;
        }

        [HttpGet("GetMasterListData")]
        public ActionResult GetMasterListData()
        {
            var res = _masterAPIService.GetListMaster().Result;
            return Ok(res);
        }

        [HttpGet("GetMasterListDataType")]
        public ActionResult GetMasterListDataType()
        {
            var res = _masterAPIService.GetListMasterType().Result;
            return Ok(res);
        }

        [HttpGet("GetSearchMasterListData")]
        public ActionResult GetSearchMasterListData(ParamSearchMasterViewModel param)
        {
            var res = _masterAPIService.GetSearchListMaster(param).Result;
            return Ok(res);
        }

        [HttpPost("PostItem")]
        public ActionResult PostItem(ResultGetMasterInfoViewModel param)
        {
            var res = _masterAPIService.PostInfo(param).Result;
            return Ok(res);
        }

        [HttpPut("PutItem")]
        public ActionResult PutItem(ResultGetMasterInfoViewModel param)
        {
            var res = _masterAPIService.PutInfo(param).Result;
            return Ok(res);
        }

        [HttpDelete("DeleteItem")]
        public ActionResult DeleteItem(ResultGetMasterInfoViewModel param)
        {
            var res = _masterAPIService.DeleteInfo(param).Result;
            return Ok(res);
        }
    }
}
