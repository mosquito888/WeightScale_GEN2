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
using WeightScaleGen2.BGC.Models.ViewModels.Department;
using WeightScaleGen2.BGC.Web.Common;
using WeightScaleGen2.BGC.Web.Controllers.Base;
using WeightScaleGen2.BGC.Web.Services;

namespace WeightScaleGen2.BGC.Web.Controllers
{
    //[Authorize]
    public class DepartmentController : BaseController
    {
        private readonly ILogger<DepartmentController> _logger;
        private readonly IExcelUtilitiesCommon _excel;
        private readonly UserService _userService;
        private readonly LogService _logService;
        private readonly DepartmentService _departmentService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public DepartmentController(
            ILogger<DepartmentController> logger,
            IExcelUtilitiesCommon excel,
            UserService userService,
            LogService logService,
            DepartmentService departmentService,
            IWebHostEnvironment hostingEnvironment) : base(userService)
        {
            _logger = logger;
            _excel = excel;
            _userService = userService;
            _logService = logService;
            _departmentService = departmentService;
            _hostingEnvironment = hostingEnvironment;
        }

        private void SetPermission()
        {
            this._SetViewBagCurrentUserMenu((long)BaseConst.MENU_DEFINITION.DEPARTMENT);
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

        public IActionResult Index()
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

            ParamSearchDeptViewModel model = new ParamSearchDeptViewModel();
            return View(model);
        }

        public IActionResult SearchData(ParamSearchDeptViewModel param)
        {
            SetPermission();

            if (!GetAction(Constants.Action.View))
            {
                return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
            }

            var result = _departmentService.GetSearchDepartmentListData(this._Username(), param);
            if (result.isCompleted && result.data.Count > 0)
            {
                return Json(new ResultJqueryDataTable<ResultSearchDeptViewModel>()
                {
                    draw = param.draw,
                    data = result.data,
                    recordsTotal = result.data[0].total_record,
                    recordsFiltered = result.data[0].total_record
                });
            }
            else
            {
                return Json(new ResultJqueryDataTable<ResultSearchDeptViewModel>());
            }
        }

        public IActionResult ActionItem(string mode, string itemId)
        {
            SetPermission();
            ResultGetDeptInfoViewModel resultObj = new ResultGetDeptInfoViewModel();
            try
            {
                if (!GetAction(Constants.Action.Created) && mode == Constants.Mode.Created)
                {
                    return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
                }

                if (!GetAction(Constants.Action.Edit) && mode == Constants.Mode.Updated)
                {
                    return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
                }

                switch (mode)
                {
                    case Constants.Mode.Created:
                        resultObj.mode = Constants.Mode.Created;
                        break;
                    case Constants.Mode.Updated:
                        var deptInfo = _departmentService.GetDepartmentListData(this._Username()).data
                            .Where(i => i.dept_code == itemId).FirstOrDefault();
                        if (deptInfo != null)
                        {
                            resultObj.dept_code = deptInfo.dept_code;
                            resultObj.name_th = deptInfo.name_th;
                            resultObj.short_code = deptInfo.short_code;
                        }
                        resultObj.mode = Constants.Mode.Updated;
                        break;
                }

                return PartialView("_ModalAction", resultObj);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IActionResult DepartmentInfo(string mode, string itemId)
        {
            SetPermission();
            ResultGetDeptInfoViewModel resultObj = new ResultGetDeptInfoViewModel();
            try
            {
                if (!GetAction(Constants.Action.Created) && mode == Constants.Mode.Created)
                {
                    return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
                }

                if (!GetAction(Constants.Action.Edit) && mode == Constants.Mode.Updated)
                {
                    return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
                }

                switch (mode)
                {
                    case Constants.Mode.Created:
                        resultObj.mode = Constants.Mode.Created;
                        break;
                    case Constants.Mode.Updated:
                        var deptInfo = _departmentService.GetDepartmentListData(this._Username()).data
                            .Where(i => i.dept_code == itemId).FirstOrDefault();
                        if (deptInfo != null)
                        {
                            resultObj.dept_code = deptInfo.dept_code;
                            resultObj.name_th = deptInfo.name_th;
                            resultObj.short_code = deptInfo.short_code;
                        }
                        resultObj.mode = Constants.Mode.Updated;
                        break;
                }

                return View(resultObj);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IActionResult DeleteItem(string mode, string itemId)
        {
            SetPermission();
            try
            {
                if (!GetAction(Constants.Action.Deleted))
                {
                    return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
                }

                string checkMsg = "";
                if (CheckDel(itemId, out checkMsg))
                {
                    return Json(new { status = Constants.Result.Invalid, message = checkMsg });
                }
                else
                {
                    var resDelete = _departmentService.Delete(this._Username(), new ResultGetDeptInfoViewModel() { dept_code = itemId });
                    if (resDelete.data)
                    {
                        return Json(new { status = Constants.Result.Success });
                    }
                    else
                    {
                        return Json(new { status = Constants.Result.Invalid, message = resDelete.message });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = Constants.Result.Error, message = ex.Message });
            }
        }

        public IActionResult SaveItem(ResultGetDeptInfoViewModel param)
        {
            SetPermission();
            try
            {
                if (!GetAction(Constants.Action.Created) && param.mode == Constants.Mode.Created)
                {
                    return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
                }

                if (!GetAction(Constants.Action.Edit) && param.mode == Constants.Mode.Updated)
                {
                    return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
                }

                switch (param.mode)
                {
                    case Constants.Mode.Created:
                        string checkMsg = "";
                        if (CheckAdd(param.dept_code, out checkMsg))
                        {
                            return Json(new { status = Constants.Result.Invalid, message = checkMsg });
                        }
                        else
                        {
                            var resCreate = _departmentService.Create(this._Username(), param);
                            if (resCreate.data)
                            {
                                return Json(new { status = Constants.Result.Success });
                            }
                            else
                            {
                                return Json(new { status = Constants.Result.Invalid, message = resCreate.message });
                            }
                        }
                    case Constants.Mode.Updated:
                        var resUpdate = _departmentService.Update(this._Username(), param);
                        if (resUpdate.data)
                        {
                            return Json(new { status = Constants.Result.Success });
                        }
                        else
                        {
                            return Json(new { status = Constants.Result.Invalid, message = resUpdate.message });
                        }
                }

                return Json(new { status = Constants.Result.Invalid, message = "Invalid." });
            }
            catch (Exception ex)
            {
                return Json(new { status = Constants.Result.Error, message = ex.Message });
            }
        }

        private bool CheckAdd(string itemId, out string checkMsg)
        {
            bool res = false;
            checkMsg = "";

            var dupDept = _departmentService.GetDepartmentListData(this._Username()).data
                .Where(i => i.dept_code == itemId).FirstOrDefault();
            if (dupDept != null)
            {
                res = true;
                checkMsg = "ขออภัยมีหน่วยงานนี้อยู่แล้วในระบบ";
            }

            return res;
        }

        private bool CheckDel(string itemId, out string checkMsg)
        {
            bool res = true;
            checkMsg = "";
            return res;
        }

        public IActionResult ActionCreateExcelFile(ParamSearchDeptViewModel param)
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
                var result = _departmentService.GetSearchDepartmentListData(this._Username(), param);
                if (result.isCompleted && result.data.Count > 0)
                {
                    var excelFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "_Export\\PlanSaleOrders");
                    if (!Directory.Exists(excelFolder))
                    {
                        Directory.CreateDirectory(excelFolder);
                    }
                    excelFile = Path.Combine(excelFolder, "plan_sale_orders.xlsx");
                    Workbook workbook = new Workbook();
                    Worksheet worksheets = workbook.Worksheets.Add("Plan Sale Orders");
                    try
                    {
                        worksheets.Range["A1"].Value = "Department Code";
                        worksheets.Range["B1"].Value = "Department Name";
                        worksheets.Range["C1"].Value = "Department Short Name";

                        worksheets.Range["A1:C1"].Style.Font.Color = Color.White;
                        worksheets.Range["A1:C1"].Style.Color = Color.Gray;
                        worksheets.Range["A1:C1"].Style.HorizontalAlignment = HorizontalAlignType.Center;
                        worksheets.Range["A1:C1"].Style.VerticalAlignment = VerticalAlignType.Center;

                        for (int i = 0; i < result.data.Count; i++)
                        {
                            var cellNo = i + 2;
                            worksheets.Range["A" + cellNo].Value = result.data[i].dept_code;
                            worksheets.Range["B" + cellNo].Value = result.data[i].name_th;
                            worksheets.Range["C" + cellNo].Value = result.data[i].short_code;
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
                    return Json(new { status = Constants.Result.Invalid, message = "Data Not Found" });
                }
                return Json(new { status = Constants.Result.Success, path = excelFile, fileName = "department.xlsx" });

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
