using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeightScaleGen2.BGC.API.APIServices;
using WeightScaleGen2.BGC.Models.ViewModels.Company;

namespace WeightScaleGen2.BGC.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CompanyController : Controller
    {
        private readonly ILogger<CompanyController> _logger;
        private readonly CompanyAPIService _companyAPIService;

        public CompanyController(ILogger<CompanyController> logger, CompanyAPIService companyAPIService)
        {
            _logger = logger;
            _companyAPIService = companyAPIService;
        }

        [HttpGet("GetCompanyListData")]
        public ActionResult GetCompanyListData()
        {
            var res = _companyAPIService.GetListComp().Result;
            return Ok(res);
        }

        [HttpGet("GetSearchCompanyListData")]
        public ActionResult GetSearchCompanyListData(ParamSearchCompViewModel param)
        {
            var res = _companyAPIService.GetSearchListComp(param).Result;
            return Ok(res);
        }

        [HttpPost("PostItem")]
        public ActionResult PostItem(ResultGetCompInfoViewModel param)
        {
            var res = _companyAPIService.PostInfo(param).Result;
            return Ok(res);
        }

        [HttpPut("PutItem")]
        public ActionResult PutItem(ResultGetCompInfoViewModel param)
        {
            var res = _companyAPIService.PutInfo(param).Result;
            return Ok(res);
        }

        [HttpDelete("DeleteItem")]
        public ActionResult DeleteItem(ResultGetCompInfoViewModel param)
        {
            var res = _companyAPIService.DeleteInfo(param).Result;
            return Ok(res);
        }
    }
}
