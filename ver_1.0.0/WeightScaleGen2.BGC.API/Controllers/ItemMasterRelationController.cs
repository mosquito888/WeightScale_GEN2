using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeightScaleGen2.BGC.API.APIServices;
using WeightScaleGen2.BGC.Models.ViewModels.ItemMasterRelation;

namespace WeightScaleGen2.BGC.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ItemMasterRelationController : Controller
    {
        private readonly ILogger<ItemMasterRelationController> _logger;
        private readonly ItemMasterRelationAPIService _itemMasterRelationAPIService;

        public ItemMasterRelationController(ILogger<ItemMasterRelationController> logger, ItemMasterRelationAPIService itemMasterRelationAPIService)
        {
            _logger = logger;
            _itemMasterRelationAPIService = itemMasterRelationAPIService;
        }

        [HttpGet("GetItemMasterRelationListData")]
        public ActionResult GetEmployeeListData()
        {
            var res = _itemMasterRelationAPIService.GetListItemMasterRelation().Result;
            return Ok(res);
        }

        [HttpGet("GetSearchItemMasterRelationListData")]
        public ActionResult GetSearchEmployeeListData(ParamSearchItemMasterRelationViewModel param)
        {
            var res = _itemMasterRelationAPIService.GetSearchListItemMasterRelation(param).Result;
            return Ok(res);
        }

        [HttpGet("GetItemMasterRelationInfo")]
        public ActionResult GetItemMasterInfo(ParamItemMasterRelationInfo param)
        {
            var res = _itemMasterRelationAPIService.GetItemMasterRelationInfo(param).Result;
            return Ok(res);
        }

        [HttpPost("PostItemMasterRelationInfo")]
        public ActionResult PostItemMasterRelationInfo(ResultGetItemMasterRelationInfoViewModel param)
        {
            var res = _itemMasterRelationAPIService.PostItemMasterRelationInfo(param).Result;
            return Ok(res);
        }

        [HttpPut("PutItemMasterRelationInfo")]
        public ActionResult PutItemMasterRelationInfo(ResultGetItemMasterRelationInfoViewModel param)
        {
            var res = _itemMasterRelationAPIService.PutItemMasterRelationInfo(param).Result;
            return Ok(res);
        }

        [HttpDelete("DeleteItem")]
        public ActionResult DeleteItem(ResultGetItemMasterRelationInfoViewModel param)
        {
            var res = _itemMasterRelationAPIService.DeleteInfo(param).Result;
            return Ok(res);
        }
    }
}
