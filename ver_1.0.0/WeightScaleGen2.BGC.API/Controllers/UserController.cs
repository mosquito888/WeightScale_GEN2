using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeightScaleGen2.BGC.API.APIServices;
using WeightScaleGen2.BGC.Models.ViewModels.User;

namespace WeightScaleGen2.BGC.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserAPIService _userAPIService;

        public UserController(ILogger<UserController> logger, UserAPIService userAPIService)
        {
            _logger = logger;
            _userAPIService = userAPIService;
        }

        [HttpPost("PostItem")]
        public ActionResult PostInfo(ResultGetUserInfo param)
        {
            var res = _userAPIService.PostInfo(param).Result;
            return Ok(res);
        }

        [HttpGet("GetSearchUserCriteria")]
        public ActionResult GetSearchUserCriteria()
        {
            var res = _userAPIService.GetSearchUserCriteria().Result;
            return Ok(res);
        }

        [HttpGet("GetSearchUserCriteriaNotAssign")]
        public ActionResult GetSearchUserCriteriaNotAssign()
        {
            var res = _userAPIService.GetSearchUserCriteriaNotAssign().Result;
            return Ok(res);
        }

        [HttpGet("SearchUser")]
        public ActionResult SearchUser(ParamSearchUser param)
        {
            var res = _userAPIService.SearchUser(param).Result;
            return Ok(res);
        }

        [HttpGet("GetUser")]
        public ActionResult GetUser(string id)
        {
            var res = _userAPIService.GetUserById(id).Result;
            return Ok(res);
        }

        [HttpGet("GetUserByUsername")]
        public ActionResult GetUserByUsername(string username)
        {
            var res = _userAPIService.GetUserByUsername(username).Result;
            return Ok(res);
        }

        [HttpGet("GetUserByName")]
        public ActionResult GetUserByName(string name)
        {
            var res = _userAPIService.GetUserByName(name).Result;
            return Ok(res);
        }

        [HttpPut("UpdateUser")]
        public ActionResult UpdateUser(ParamUpdateUser param)
        {
            var res = _userAPIService.UpdateUser(param).Result;
            return Ok(res);
        }

        [HttpPost("UploadImage")]
        public ActionResult UploadImage(ParamUploadImage param)
        {
            var res = _userAPIService.UploadImage(param).Result;
            return Ok(null);
        }

        [HttpGet("GetAllImage")]
        public ActionResult GetImageAll()
        {
            var res = _userAPIService.GetImageAll().Result;
            return Ok(res);
        }

        [HttpGet("GetUserByUsernamePassword")]
        public ActionResult GetUserByUsernamePassword(ParamLoginUser param)
        {
            var res = _userAPIService.GetUserByUsernamePassword(param).Result;
            return Ok(res);
        }

    }
}
