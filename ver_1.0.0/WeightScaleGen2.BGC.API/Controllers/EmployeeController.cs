using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeightScaleGen2.BGC.API.APIServices;
using WeightScaleGen2.BGC.Models.ViewModels.Employee;

namespace WeightScaleGen2.BGC.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly EmployeeAPIService _employeeAPIService;

        public EmployeeController(ILogger<EmployeeController> logger, EmployeeAPIService employeeAPIService)
        {
            _logger = logger;
            _employeeAPIService = employeeAPIService;
        }

        [HttpGet("GetEmployeeListData")]
        public ActionResult GetEmployeeListData()
        {
            var res = _employeeAPIService.GetListEmp().Result;
            return Ok(res);
        }

        [HttpGet("GetSearchEmployeeListData")]
        public ActionResult GetSearchEmployeeListData(ParamSearchEmpViewModel param)
        {
            var res = _employeeAPIService.GetSearchListEmp(param).Result;
            return Ok(res);
        }

        [HttpGet("GetEmployeeInfo")]
        public ActionResult GetEmployeeInfo(ParamEmpInfo param)
        {
            var res = _employeeAPIService.GetEmpInfo(param).Result;
            return Ok(res);
        }

        [HttpPost("PostEmployeeInfo")]
        public ActionResult PostEmployeeInfo(ResultGetEmpInfoViewModel param)
        {
            var res = _employeeAPIService.PostEmployeeInfo(param).Result;
            return Ok(res);
        }

        [HttpPut("PutEmployeeInfo")]
        public ActionResult PutEmployeeInfo(ResultGetEmpInfoViewModel param)
        {
            var res = _employeeAPIService.PutEmployeeInfo(param).Result;
            return Ok(res);
        }
    }
}
