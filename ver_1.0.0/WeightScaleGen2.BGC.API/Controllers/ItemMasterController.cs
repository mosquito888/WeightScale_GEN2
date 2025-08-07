using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeightScaleGen2.BGC.API.APIServices;
using WeightScaleGen2.BGC.Models.ViewModels.ItemMaster;

namespace WeightScaleGen2.BGC.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ItemMasterController : Controller
    {
        private readonly ILogger<ItemMasterController> _logger;
        private readonly ItemMasterAPIService _itemMasterAPIService;

        public ItemMasterController(ILogger<ItemMasterController> logger, ItemMasterAPIService itemMasterAPIService)
        {
            _logger = logger;
            _itemMasterAPIService = itemMasterAPIService;
        }

        [HttpGet("GetItemMasterListData")]
        public ActionResult GetItemMasterListData()
        {
            var res = _itemMasterAPIService.GetListItemMaster().Result;
            return Ok(res);
        }

        [HttpGet("GetSearchItemMasterListData")]
        public ActionResult GetSearchEmployeeListData(ParamSearchItemMasterViewModel param)
        {
            var res = _itemMasterAPIService.GetSearchListItemMaster(param).Result;
            return Ok(res);
        }

        [HttpGet("GetItemMasterInfo")]
        public ActionResult GetItemMasterInfo(ParamItemMasterInfo param)
        {
            var res = _itemMasterAPIService.GetItemMasterInfo(param).Result;
            return Ok(res);
        }

        [HttpPost("PostItemMasterInfo")]
        public ActionResult PostItemMasterInfo(ResultGetItemMasterInfoViewModel param)
        {
            var res = _itemMasterAPIService.PostItemMasterInfo(param).Result;
            return Ok(res);
        }

        [HttpPut("PutItemMasterInfo")]
        public ActionResult PutItemMasterInfo(ResultGetItemMasterInfoViewModel param)
        {
            var res = _itemMasterAPIService.PutItemMasterInfo(param).Result;
            return Ok(res);
        }

        [HttpDelete("DeleteItem")]
        public ActionResult DeleteItem(ResultGetItemMasterInfoViewModel param)
        {
            var res = _itemMasterAPIService.DeleteInfo(param).Result;
            return Ok(res);
        }
    }
}
