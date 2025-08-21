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
    public class DashboardController : BaseController
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IExcelUtilitiesCommon _excel;
        private readonly UserService _userService;
        private readonly MasterService _masterService;
        private readonly DashboardService _DashboardService;
        private readonly ItemMasterService _itemMasterService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public DashboardController(ILogger<DashboardController> logger, IExcelUtilitiesCommon excel, UserService userService, MasterService masterService, DashboardService DashboardService, ItemMasterService itemMasterService, IWebHostEnvironment hostingEnvironment) : base(userService)
        {
            _logger = logger;
            _excel = excel;
            _userService = userService;
            _masterService = masterService;
            _DashboardService = DashboardService;
            _itemMasterService = itemMasterService;
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

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult SearchSummaryData()
        {
            try
            {
                var result = _DashboardService.GetSearchListDashboard(this._Username());
                if (result.isCompleted)
                {
                    return Json(new { status = Constants.Result.Success, data = result.data });
                }
                else
                {
                    return Json(new { status = Constants.Result.Invalid, data = result.data, message = result.message[0] });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = Constants.Result.Invalid, message = ex.Message });
            }
        }

        public ActionResult SearchSummaryHistoryData()
        {
            try
            {
                var result = _DashboardService.GetSearchListDashboardHistory(this._Username());
                if (result.isCompleted)
                {
                    return Json(new { status = Constants.Result.Success, data = result.data });
                }
                else
                {
                    return Json(new { status = Constants.Result.Invalid, data = result.data, message = result.message[0] });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = Constants.Result.Invalid, message = ex.Message });
            }
        }
    }
}
