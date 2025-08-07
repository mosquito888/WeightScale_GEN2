using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeightScaleGen2.BGC.API.APIServices;
using WeightScaleGen2.BGC.Models.ViewModels.WeightHistory;

namespace WeightScaleGen2.BGC.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WeightHistoryController : Controller
    {
        private readonly ILogger<WeightHistoryController> _logger;
        private readonly WeightHistoryAPIService _weightHistoryAPIService;

        public WeightHistoryController(ILogger<WeightHistoryController> logger, WeightHistoryAPIService weightHistoryAPIService)
        {
            _logger = logger;
            _weightHistoryAPIService = weightHistoryAPIService;
        }

        //[HttpGet("GetItemMasterListData")]
        //public ActionResult GetItemMasterListData()
        //{
        //    var res = _itemMasterAPIService.GetListItemMaster().Result;
        //    return Ok(res);
        //}

        [HttpGet("GetSearchWeightHistoryListData")]
        public ActionResult GetSearchWeightHistoryListData(ParamSearchWeightHistoryViewModel param)
        {
            var res = _weightHistoryAPIService.GetSearchListWeightHistory(param).Result;
            return Ok(res);
        }

        [HttpGet("GetWeightHistoryInfo")]
        public ActionResult GetWeightHistoryInfo(ParamWeightHistoryInfo param)
        {
            var res = _weightHistoryAPIService.GetWeightHistoryInfo(param).Result;
            return Ok(res);
        }

    }
}
