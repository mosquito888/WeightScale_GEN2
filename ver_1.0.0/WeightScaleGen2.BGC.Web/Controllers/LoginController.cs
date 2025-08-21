using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.ViewModels.User;
using WeightScaleGen2.BGC.Web.Common;
using WeightScaleGen2.BGC.Web.Controllers.Base;
using WeightScaleGen2.BGC.Web.Services;

namespace WeightScaleGen2.BGC.Web.Controllers
{
    //[Authorize]
    public class LoginController : BaseController
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IExcelUtilitiesCommon _excel;
        private readonly UserService _userService;
        private readonly MasterService _masterService;
        private readonly MMPOService _mmPOService;
        private readonly AuthService _authService;

        public LoginController(ILogger<LoginController> logger, IExcelUtilitiesCommon excel, UserService userService, MasterService masterService, MMPOService mmPOService, AuthService authService) : base(userService)
        {
            _logger = logger;
            _excel = excel;
            _userService = userService;
            _masterService = masterService;
            _mmPOService = mmPOService;
            _authService = authService;
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

        [AllowAnonymous]
        public IActionResult Index()
        {
            string tokenVal = Request.Cookies["token"];
            if (tokenVal != null && !String.IsNullOrEmpty(tokenVal))
            {
                if (GetAction(Constants.Action.Created))
                {
                    _ = _mmPOService.UpdateMMPOSapToDocumentPOData(this._Username());
                }
                return RedirectToAction("Index", "Home");
            }
            _authService.ClearCookie();
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login(ParamLoginUser param)
        {
            var result = _userService.GetUserByUsernamePassword(param, param.username);
            if (result.isCompleted && result.data != null)
            {
                var token = _authService.GenerateToken(result.data.email);
                _authService.SetTokenToCookie(token.Result.token);
            }
            return Json(result);
        }

        [AllowAnonymous]
        public void GenerateToken(string value)
        {
            var token = _authService.GenerateToken(value);
            _authService.SetTokenToCookie(token.Result.token);
        }
    }
}
