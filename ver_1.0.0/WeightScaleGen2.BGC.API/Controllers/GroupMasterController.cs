using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeightScaleGen2.BGC.API.APIServices;

namespace WeightScaleGen2.BGC.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class GroupMasterController : Controller
    {
        private readonly ILogger<GroupMasterController> _logger;
        private readonly GroupMasterAPIService _groupMasterAPIService;

        public GroupMasterController(ILogger<GroupMasterController> logger, GroupMasterAPIService groupMasterAPIService)
        {
            _logger = logger;
            _groupMasterAPIService = groupMasterAPIService;
        }

        [HttpGet("GetGroupMasterListData")]
        public ActionResult GetGroupMasterListData()
        {
            var res = _groupMasterAPIService.GetListGroupMaster().Result;
            return Ok(res);
        }
    }
}
