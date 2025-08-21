using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Web.Common;
using WeightScaleGen2.BGC.Web.Controllers.Base;
using WeightScaleGen2.BGC.Web.Services;

namespace WeightScaleGen2.BGC.Web.Controllers
{
    //[Authorize]
    public class WeighingScaleController : BaseController
    {
        private readonly ILogger<WeighingScaleController> _logger;
        private readonly IExcelUtilitiesCommon _excel;
        private readonly UserService _userService;
        private readonly MasterService _masterService;
        private readonly WeighingScaleService _weighingScaleService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public WeighingScaleController(ILogger<WeighingScaleController> logger, IExcelUtilitiesCommon excel, UserService userService, MasterService masterService, WeighingScaleService weighingScaleService, IWebHostEnvironment hostingEnvironment) : base(userService)
        {
            _logger = logger;
            _excel = excel;
            _userService = userService;
            _masterService = masterService;
            _weighingScaleService = weighingScaleService;
            _hostingEnvironment = hostingEnvironment;
        }

        private void SetPermission()
        {
            _SetViewBagCurrentUserMenu((long)Models.ViewModels.Base.BaseConst.MENU_DEFINITION.NOTSET);
            ViewBag.view = _GetPermission(_GetControllerName(), Constants.Action.View);
            ViewBag.created = _GetPermission(_GetControllerName(), Constants.Action.Created);
            ViewBag.edit = _GetPermission(_GetControllerName(), Constants.Action.Edit);
            ViewBag.deleted = _GetPermission(_GetControllerName(), Constants.Action.Deleted);
            ViewBag.print = _GetPermission(_GetControllerName(), Constants.Action.Print);
            ViewBag.MenuName = _GetMenuName(_GetControllerName());
        }

        private bool GetAction(string action)
        {
            bool result = _GetPermission(_GetControllerName(), action) == "Y" ? true : false;
            return result;
        }

        public bool IsToken()
        {
            string tokenVal = Request.Cookies["token"];
            if (tokenVal != null && !String.IsNullOrEmpty(tokenVal))
            {
                return true;
            }
            return false;
        }

        public IActionResult GetConnectWeighingScale()
        {
            SetPermission();

            var result = _weighingScaleService.GetConnectWeighingScale(this._Username());
            if (result.isCompleted)
            {
                return Json(new { status = Constants.Result.Success, message = result.data });
            }
            else
            {
                return Json(new { status = Constants.Result.Invalid, message = result.data });
            }
        }

        public IActionResult GetWeightByWeighingScale()
        {
            SetPermission();

            var result = _weighingScaleService.GetWeightByWeighingScale(this._Username(), "A");
            if (result.isCompleted)
            {
                return Json(new { status = Constants.Result.Success, data = result.data, message = result.message[0] });
            }
            else
            {
                return Json(new { status = Constants.Result.Invalid, data = result.data, message = result.message[0] });
            }
        }
    }
}
