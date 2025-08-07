using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.ViewModels.Home;
using WeightScaleGen2.BGC.Web.Common;
using WeightScaleGen2.BGC.Web.Controllers.Base;
using WeightScaleGen2.BGC.Web.Services;

namespace WeightScaleGen2.BGC.Web.Controllers
{
    //[Authorize]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IExcelUtilitiesCommon _excel;
        private readonly UserService _userService;
        private readonly MasterService _masterService;
        private readonly AuthService _authenService;

        public HomeController(ILogger<HomeController> logger, IExcelUtilitiesCommon excel, UserService userService, MasterService masterService, AuthService authenService) : base(userService)
        {
            _logger = logger;
            _excel = excel;
            _userService = userService;
            _masterService = masterService;
            _authenService = authenService;
        }

        private void SetPermission()
        {
            _SetViewBagCurrentUserMenu((long)Models.ViewModels.Base.BaseConst.MENU_DEFINITION.HOME);
            ViewBag.view = _GetPermission(_GetControllerName(), Constants.Action.View);
            ViewBag.created = _GetPermission(_GetControllerName(), Constants.Action.Created);
            ViewBag.edit = _GetPermission(_GetControllerName(), Constants.Action.Edit);
            ViewBag.deleted = _GetPermission(_GetControllerName(), Constants.Action.Deleted);
            ViewBag.print = _GetPermission(_GetControllerName(), Constants.Action.Print);
            ViewBag.MenuName = _GetMenuName(_GetControllerName());
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

        [AllowAnonymous]
        public IActionResult Index()
        {
            ResultHomeViewModel model = new ResultHomeViewModel
            {
                ls_file_system = new List<VMRESULT_FILE_SYSTEM>()
            };

            if (!IsToken())
            {
                return RedirectToAction("Index", "Login");
            }

            SetPermission();
            TempData["SuccessMessages"] = "Login Success";

            var getMasterInfo = _masterService.GetMasterListData(this._Username()).data
                .Where(i => i.master_type == Constants.MasterType.FileSystem).ToList();

            if (getMasterInfo.Count > 0)
            {
                foreach (var item in getMasterInfo)
                {
                    VMRESULT_FILE_SYSTEM obj = new VMRESULT_FILE_SYSTEM
                    {
                        type = item.master_type,
                        code = item.master_code,
                        name = item.master_value1,
                        path = item.master_value2
                    };
                    model.ls_file_system.Add(obj);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Download(string fileCode)
        {
            try
            {
                var getMasterInfo = _masterService.GetMasterListData(this._Username()).data
                    .Where(i => i.master_type == Constants.MasterType.FileSystem)
                    .Where(i => i.master_code == fileCode).FirstOrDefault();

                if (getMasterInfo != null)
                {
                    string fullPath = getMasterInfo.master_value2;
                    var fileInfo = new FileInfo(fullPath);
                    if (fileInfo != null)
                    {
                        var memory = new MemoryStream();
                        using (var stream = new FileStream(fullPath, FileMode.Open))
                        {
                            await stream.CopyToAsync(memory);
                        }
                        memory.Position = 0;

                        return File(memory, "application/octet-stream", getMasterInfo.master_value3);
                    }
                    else
                    {
                        return null;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Logout()
        {
            _authenService.ClearCookie();
            return RedirectToAction("Index", "Login");
        }

        public IActionResult Modal()
        {
            return PartialView();
        }

    }
}
