using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeightScaleGen2.BGC.API.APIServices;
using WeightScaleGen2.BGC.Models.ViewModels.UOMConversion;

namespace WeightScaleGen2.BGC.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UOMConversionController : Controller
    {
        private readonly ILogger<UOMConversionController> _logger;
        private readonly UOMConversionAPIService _uomonversionAPIService;

        public UOMConversionController(ILogger<UOMConversionController> logger, UOMConversionAPIService uomConversionAPIService)
        {
            _logger = logger;
            _uomonversionAPIService = uomConversionAPIService;
        }

        [HttpGet("GetUOMConversionListData")]
        public ActionResult GetUOMConversionListData()
        {
            var res = _uomonversionAPIService.GetUOMConversionListData().Result;
            return Ok(res);
        }

        [HttpGet("GetUOMConversionListDataBy")]
        public ActionResult GetUOMConversionListDataBy(ParamSearchUOMConversionViewModel param)
        {
            var res = _uomonversionAPIService.GetUOMConversionList_By(param).Result;
            return Ok(res);
        }

        [HttpGet("GetUOMConversionListByMaterialCode")]
        public ActionResult GetUOMConversionListByMaterialCode(string materialCode)
        {
            var res = _uomonversionAPIService.GetUOMConversionListByMaterialCode(materialCode).Result;
            return Ok(res);
        }
    }
}
