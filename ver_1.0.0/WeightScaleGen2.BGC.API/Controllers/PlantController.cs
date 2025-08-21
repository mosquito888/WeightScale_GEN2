using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeightScaleGen2.BGC.API.APIServices;
using WeightScaleGen2.BGC.Models.ViewModels.Plant;

namespace WeightScaleGen2.BGC.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PlantController : Controller
    {
        private readonly ILogger<PlantController> _logger;
        private readonly PlantAPIService _plantAPIService;

        public PlantController(ILogger<PlantController> logger, PlantAPIService plantAPIService)
        {
            _logger = logger;
            _plantAPIService = plantAPIService;
        }

        [HttpGet("GetPlantListData")]
        public ActionResult GetPlantListData()
        {
            var res = _plantAPIService.GetListPlant().Result;
            return Ok(res);
        }

        [HttpGet("GetSearchPlantListData")]
        public ActionResult GetSearchPlantListData(ParamSearchPlantViewModel param)
        {
            var res = _plantAPIService.GetSearchListPlant(param).Result;
            return Ok(res);
        }


        [HttpPost("PostItem")]
        public ActionResult PostItem(ResultGetPlantInfoViewModel param)
        {
            var res = _plantAPIService.PostInfo(param).Result;
            return Ok(res);
        }

        [HttpPut("PutItem")]
        public ActionResult PutItem(ResultGetPlantInfoViewModel param)
        {
            var res = _plantAPIService.PutInfo(param).Result;
            return Ok(res);
        }

        [HttpDelete("DeleteItem")]
        public ActionResult DeleteItem(ResultGetPlantInfoViewModel param)
        {
            var res = _plantAPIService.DeleteInfo(param).Result;
            return Ok(res);
        }
    }
}
