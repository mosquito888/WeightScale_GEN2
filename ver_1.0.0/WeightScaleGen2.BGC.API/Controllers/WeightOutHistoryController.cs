using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeightScaleGen2.BGC.API.APIServices;
using WeightScaleGen2.BGC.Models.ViewModels.WeightOutHistory;

namespace WeightScaleGen2.BGC.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WeightOutHistoryController : Controller
    {
        private readonly ILogger<WeightOutHistoryController> _logger;
        private readonly WeightOutHistoryAPIService _weightOutHistoryAPIService;

        public WeightOutHistoryController(ILogger<WeightOutHistoryController> logger, WeightOutHistoryAPIService weightOutHistoryAPIService)
        {
            _logger = logger;
            _weightOutHistoryAPIService = weightOutHistoryAPIService;
        }

        [HttpGet("GetListWeightOutHistory")]
        public ActionResult GetListWeightOutHistory()
        {
            var res = _weightOutHistoryAPIService.GetListWeightOutHistory().Result;
            return Ok(res);
        }

        [HttpGet("GetSearchWeightOutHistoryListData")]
        public ActionResult GetSearchWeightOutHistoryListData(ParamSearchWeightOutHistoryViewModel param)
        {
            var res = _weightOutHistoryAPIService.GetSearchListWeightOutHistory(param).Result;
            return Ok(res);
        }

    }
}
