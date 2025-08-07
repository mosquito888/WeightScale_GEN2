using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeightScaleGen2.BGC.API.APIServices;
using WeightScaleGen2.BGC.Models.ViewModels.Menu;
using WeightScaleGen2.BGC.Models.ViewModels.Role;

namespace WeightScaleGen2.BGC.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly ILogger<MenuController> _logger;
        private readonly MenuAPIService _menuAPIService;

        public MenuController(ILogger<MenuController> logger, MenuAPIService menuAPIService)
        {
            _logger = logger;
            _menuAPIService = menuAPIService;
        }

        [HttpGet("AuthenticateMenuUser")]
        public ActionResult AuthenticateMenuUser([FromBody] PramGetMenuViewModel param)
        {
            var res = _menuAPIService.AuthenticateMenuUser(param).Result;
            return Ok(res);
        }

        [HttpGet("GetMenuRole")]
        public ActionResult GetMenuRole(string id)
        {
            var res = _menuAPIService.GetMenuRole(id).Result;
            return Ok(res);
        }

        [HttpPut("UpdateMenuRole")]
        public ActionResult UpdateMenuRole(ParamUpdateRoleItemViewModel param)
        {
            var res = _menuAPIService.UpdateMenuRole(param).Result;
            return Ok(res);
        }

        [HttpPut("UpdateMenuRoleSelect")]
        public ActionResult UpdateMenuRoleSelect(UpdateRoleItemSection param)
        {
            var res = _menuAPIService.UpdateMenuRoleSelectItem(param).Result;
            return Ok(res);
        }

        [HttpPost("PostNewRole")]
        public ActionResult PostNewRole(ResultRoleInfo param)
        {
            var res = _menuAPIService.CreatedRole(param).Result;
            return Ok(res);
        }

        [HttpGet("GetRoleInfo")]
        public ActionResult GetRoleInfo(ResultRoleInfo param)
        {
            var res = _menuAPIService.GetRoleInfo(param).Result;
            return Ok(res);
        }

        [HttpPut("PutRole")]
        public ActionResult PutRole(ResultRoleInfo param)
        {
            var res = _menuAPIService.UpdatedRole(param).Result;
            return Ok(res);
        }

        [HttpDelete("DeleteRole")]
        public ActionResult DeleteRole(ResultRoleInfo param)
        {
            var res = _menuAPIService.DeletedRole(param).Result;
            return Ok(res);
        }
    }
}
