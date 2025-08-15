using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeightScaleGen2.BGC.API.APIServices;
using WeightScaleGen2.BGC.Models.ViewModels.WeightInHistory;

namespace WeightScaleGen2.BGC.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WeightInHistoryController : Controller
    {
        private readonly ILogger<WeightInHistoryController> _logger;
        private readonly WeightInHistoryAPIService _weightInHistoryAPIService;

        public WeightInHistoryController(ILogger<WeightInHistoryController> logger, WeightInHistoryAPIService weightInHistoryAPIService)
        {
            _logger = logger;
            _weightInHistoryAPIService = weightInHistoryAPIService;
        }

        [HttpGet("GetListWeightInHistory")]
        public ActionResult GetListWeightInHistory()
        {
            var res = _weightInHistoryAPIService.GetListWeightInHistory().Result;
            return Ok(res);
        }

        [HttpGet("GetSearchWeightInHistoryListData")]
        public ActionResult GetSearchWeightInHistoryListData(ParamSearchWeightInHistoryViewModel param)
        {
            var res = _weightInHistoryAPIService.GetSearchListWeightInHistory(param).Result;
            return Ok(res);
        }

    }
}
