using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.ViewModels.UOMConversion;
using WeightScaleGen2.BGC.Web.Common;
using WeightScaleGen2.BGC.Web.Controllers.Base;
using WeightScaleGen2.BGC.Web.Services;

namespace WeightScaleGen2.BGC.Web.Controllers
{
    //[Authorize]
    public class UOMConversionController : BaseController
    {
        private readonly ILogger<UOMConversionController> _logger;
        private readonly IExcelUtilitiesCommon _excel;
        private readonly UserService _userService;
        private readonly MasterService _masterService;
        private readonly UOMConversionService _UOMConversionService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public UOMConversionController(ILogger<UOMConversionController> logger, IExcelUtilitiesCommon excel, UserService userService, MasterService masterService, UOMConversionService UOMConversionService, IWebHostEnvironment hostingEnvironment) : base(userService)
        {
            _logger = logger;
            _excel = excel;
            _userService = userService;
            _masterService = masterService;
            _UOMConversionService = UOMConversionService;
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

        public IActionResult SearchDataByMaterialCode(ParamSearchUOMConversionViewModel param)
        {
            SetPermission();

            if (!GetAction(Constants.Action.View))
            {
                return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
            }

            var result = _UOMConversionService.GetSearchUOMConversionListByMaterialCode(this._Username(), param.material_code);
            if (result.isCompleted && result.data.Count > 0)
            {
                return Json(new { status = Constants.Result.Success, data = result.data, message = result.message[0] });
            }
            else
            {
                return Json(new { status = Constants.Result.Invalid, data = result.data, message = result.message[0] });
            }
        }

        public IActionResult SearchData(ParamSearchUOMConversionViewModel param)
        {
            SetPermission();

            if (!GetAction(Constants.Action.View))
            {
                return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
            }

            var result = _UOMConversionService.GetSearchUOMConversionListBy(this._Username(), param);
            if (result.isCompleted && result.data.Count > 0)
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
