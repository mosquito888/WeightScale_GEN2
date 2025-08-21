using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeightScaleGen2.BGC.API.APIServices;
using WeightScaleGen2.BGC.Models.ViewModels.Department;

namespace WeightScaleGen2.BGC.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DepartmentController : Controller
    {
        private readonly ILogger<DepartmentController> _logger;
        private readonly DepartmentAPIService _departmentAPIService;

        public DepartmentController(ILogger<DepartmentController> logger, DepartmentAPIService departmentAPIService)
        {
            _logger = logger;
            _departmentAPIService = departmentAPIService;
        }

        [HttpGet("GetDepartmentListData")]
        public ActionResult GetDepartmentListData()
        {
            var res = _departmentAPIService.GetListDept().Result;
            return Ok(res);
        }

        [HttpGet("GetSearchDepartmentListData")]
        public ActionResult GetSearchDepartmentListData(ParamSearchDeptViewModel param)
        {
            var res = _departmentAPIService.GetSearchListDept(param).Result;
            return Ok(res);
        }

        [HttpPost("PostItem")]
        public ActionResult PostItem(ResultGetDeptInfoViewModel param)
        {
            var res = _departmentAPIService.PostInfo(param).Result;
            return Ok(res);
        }

        [HttpPut("PutItem")]
        public ActionResult PutItem(ResultGetDeptInfoViewModel param)
        {
            var res = _departmentAPIService.PutInfo(param).Result;
            return Ok(res);
        }

        [HttpDelete("DeleteItem")]
        public ActionResult DeleteItem(ResultGetDeptInfoViewModel param)
        {
            var res = _departmentAPIService.DeleteInfo(param).Result;
            return Ok(res);
        }
    }
}
