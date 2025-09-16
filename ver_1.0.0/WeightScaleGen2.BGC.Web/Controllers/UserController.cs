using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Spire.Xls;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.ViewModels.Base;
using WeightScaleGen2.BGC.Models.ViewModels.User;
using WeightScaleGen2.BGC.Web.Common;
using WeightScaleGen2.BGC.Web.Controllers.Base;
using WeightScaleGen2.BGC.Web.Services;

namespace WeightScaleGen2.BGC.Web.Controllers
{
    //[Authorize]
    public class UserController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IExcelUtilitiesCommon _excel;
        private readonly UserService _userService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public UserController(ILogger<UserController> logger, IExcelUtilitiesCommon excel, UserService userService, IWebHostEnvironment webHostEnvironment) : base(userService)
        {
            _logger = logger;
            _excel = excel;
            _userService = userService;
            _hostingEnvironment = webHostEnvironment;
        }

        private void SetPermission()
        {
            this._SetViewBagCurrentUserMenu((long)BaseConst.MENU_DEFINITION.USER);
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

        public IActionResult UserManagement()
        {
            if (!IsToken())
            {
                return RedirectToAction("Index", "Login");
            }

            SetPermission();

            if (!GetAction(Constants.Action.View))
            {
                return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
            }

            var result = _userService.GetSearchUserCriteria(_Username());
            if (result.isCompleted && result.data.role_dll.Count() > 0)
            {
                return View(result.data);
            }
            else
            {
                _logger.LogWarning(result.message[0]);
                return View();
            }
        }

        public IActionResult UserDataTableExmOne()
        {
            SetPermission();

            if (!GetAction(Constants.Action.View))
            {
                return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
            }

            var result = _userService.GetSearchUserCriteria(_Username());
            if (result.isCompleted)
            {
                return View(result.data);
            }
            else
            {
                _logger.LogWarning(result.message[0]);
                return View();
            }
        }

        public IActionResult UserDataTableExmTwo()
        {
            SetPermission();

            if (!GetAction(Constants.Action.View))
            {
                return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
            }

            var result = _userService.GetSearchUserCriteria(_Username());
            if (result.isCompleted)
            {
                return View(result.data);
            }
            else
            {
                _logger.LogWarning(result.message[0]);
                return View();
            }
        }

        public IActionResult UserInfo(string id)
        {
            SetPermission();

            if (!GetAction(Constants.Action.View))
            {
                return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
            }

            var result = _userService.GetUser(id, _Username());
            if (result.isCompleted)
            {
                return View(result.data);
            }
            else
            {
                _logger.LogWarning(result.message[0]);
                return View();
            }
        }

        public IActionResult UpdateUser(ParamUpdateUser param)
        {
            SetPermission();

            if (!GetAction(Constants.Action.Edit))
            {
                return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
            }

            var result = _userService.UpdateUser(param, _Username());
            if (result.isCompleted)
            {
                return Json(new { status = Constants.Result.Success });
            }
            else
            {
                return Json(new { status = Constants.Result.Error, message = result.message[0] });
            }
        }

        public IActionResult SearchUserManagement(ParamSearchUser param)
        {
            SetPermission();

            if (!GetAction(Constants.Action.View))
            {
                return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
            }

            var result = _userService.SearchUser(param, _Username());
            if (result.isCompleted && result.data.Count > 0)
            {
                return Json(new ResultJqueryDataTable<ResultSearchUser>()
                {
                    draw = param.draw,
                    data = result.data,
                    recordsTotal = result.data[0].total_record,
                    recordsFiltered = result.data[0].total_record
                });
            }
            else
            {
                _logger.LogWarning(result.message[0]);
                return Json(new ResultJqueryDataTable<ResultSearchUser>());
            }
        }

        public IActionResult UserImport()
        {
            if (!GetAction(Constants.Action.View))
            {
                return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
            }

            return View();
        }

        public async Task<JsonResult> CreateUserImport(BaseUpload param)
        {
            if (!GetAction(Constants.Action.Created))
            {
                return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
            }

            var desc = param.file_desc;
            var file = param.file_upload;
            var dt = await this._excel.ConvertExcelToDataTable(currentDir: Directory.GetCurrentDirectory(), wwwrootPath: "uploaded/fileEx/01", excelFile: file, keepFile: false);

            return Json(dt);
        }

        public IActionResult UploadFile(BaseUpload param)
        {
            if (!GetAction(Constants.Action.Created))
            {
                return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
            }

            ParamUploadImage paramUpload = new ParamUploadImage();
            if (param.file_upload.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    param.file_upload.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    paramUpload.file = fileBytes;

                }
            }

            var result = _userService.UpLoadImage(paramUpload, _Username());
            var rsx = _userService.GetImageAll(_Username());

            return Json(rsx);
        }

        public IActionResult ActionCreateExcelFile(ParamSearchUser param)
        {
            SetPermission();

            if (!GetAction(Constants.Action.View))
            {
                return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
            }
            try
            {
                param.start = 0;
                param.draw = int.MaxValue;
                param.length = int.MaxValue;
                string excelFile = "";
                var result = _userService.SearchUser(param, _Username());
                if (result.isCompleted && result.data.Count > 0)
                {
                    var excelFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "_Export\\User");
                    if (!Directory.Exists(excelFolder))
                    {
                        Directory.CreateDirectory(excelFolder);
                    }
                    excelFile = Path.Combine(excelFolder, "user.xlsx");
                    Workbook workbook = new Workbook();
                    Worksheet worksheets = workbook.Worksheets.Add("User");
                    try
                    {
                        worksheets.Range["A1"].Value = "Name";
                        worksheets.Range["B1"].Value = "E-mail";
                        worksheets.Range["C1"].Value = "Role";

                        worksheets.Range["A1:C1"].Style.Font.Color = Color.White;
                        worksheets.Range["A1:C1"].Style.Color = Color.Gray;
                        worksheets.Range["A1:C1"].Style.HorizontalAlignment = HorizontalAlignType.Center;
                        worksheets.Range["A1:C1"].Style.VerticalAlignment = VerticalAlignType.Center;

                        for (int i = 0; i < result.data.Count; i++)
                        {
                            var cellNo = i + 2;
                            worksheets.Range["A" + cellNo].Value = result.data[i].name;
                            worksheets.Range["B" + cellNo].Value = result.data[i].email;
                            worksheets.Range["C" + cellNo].Value = result.data[i].role_name;
                        }
                        worksheets.AllocatedRange.AutoFitColumns();
                        worksheets.AllocatedRange.AutoFitRows();
                        foreach (CellRange range in worksheets.Cells)
                        {
                            range.Style.HorizontalAlignment = HorizontalAlignType.Center;
                        }
                        workbook.SaveToFile(excelFile);
                        Workbook wb = new Workbook();
                        wb.LoadFromFile(excelFile);
                        wb.Worksheets.Remove("Sheet1");
                        wb.Worksheets.Remove("Sheet2");
                        wb.Worksheets.Remove("Sheet3");
                        wb.SaveToFile(excelFile);
                    }
                    catch (Exception ex)
                    {
                        return Json(new { status = Constants.Result.Invalid, message = ex.Message });
                    }
                }
                else
                {
                    _logger.LogWarning(result.message[0]);
                    return Json(new { status = Constants.Result.Invalid, message = "Data Not Found" });
                }
                return Json(new { status = Constants.Result.Success, path = excelFile, fileName = "user.xlsx" });
            }
            catch (Exception ex)
            {
                return Json(new { status = Constants.Result.Invalid, message = ex.Message });
            }
        }

        public async Task<IActionResult> ActionOpenFileExcel(string path, string fileName)
        {
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/octet-stream", fileName);
        }

    }
}
