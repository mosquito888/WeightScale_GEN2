using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeightScaleGen2.BGC.API.APIServices;
using WeightScaleGen2.BGC.Models.ViewModels.Supplier;

namespace WeightScaleGen2.BGC.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SupplierController : Controller
    {
        private readonly ILogger<SupplierController> _logger;
        private readonly SupplierAPIService _supplierAPIService;

        public SupplierController(ILogger<SupplierController> logger, SupplierAPIService supplierAPIService)
        {
            _logger = logger;
            _supplierAPIService = supplierAPIService;
        }

        [HttpGet("GetSupplierListData")]
        public ActionResult GetSupplierListData()
        {
            var res = _supplierAPIService.GetSupplierListData().Result;
            return Ok(res);
        }

        [HttpGet("GetSearchSupplierListData")]
        public ActionResult GetSearchEmployeeListData(ParamSearchSupplierViewModel param)
        {
            var res = _supplierAPIService.GetSearchListSupplier(param).Result;
            return Ok(res);
        }

        [HttpGet("GetSupplierInfo")]
        public ActionResult GetSupplierInfo(ParamSupplierInfo param)
        {
            var res = _supplierAPIService.GetSupplierInfo(param).Result;
            return Ok(res);
        }

        [HttpPost("PostSupplierInfo")]
        public ActionResult PostSupplierInfo(ResultGetSupplierInfoViewModel param)
        {
            var res = _supplierAPIService.PostSupplierInfo(param).Result;
            return Ok(res);
        }

        [HttpPut("PutSupplierInfo")]
        public ActionResult PutSupplierInfo(ResultGetSupplierInfoViewModel param)
        {
            var res = _supplierAPIService.PutSupplierInfo(param).Result;
            return Ok(res);
        }

        [HttpDelete("DeleteItem")]
        public ActionResult DeleteItem(ResultGetSupplierInfoViewModel param)
        {
            var res = _supplierAPIService.DeleteInfo(param).Result;
            return Ok(res);
        }
    }
}
