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
using WeightScaleGen2.BGC.Models.ViewModels.Supplier;
using WeightScaleGen2.BGC.Web.Common;
using WeightScaleGen2.BGC.Web.Controllers.Base;
using WeightScaleGen2.BGC.Web.Services;

namespace WeightScaleGen2.BGC.Web.Controllers
{
    //[Authorize]
    public class SupplierController : BaseController
    {
        private readonly ILogger<SupplierController> _logger;
        private readonly IExcelUtilitiesCommon _excel;
        private readonly UserService _userService;
        private readonly MasterService _masterService;
        private readonly SupplierService _supplierService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public SupplierController(ILogger<SupplierController> logger, IExcelUtilitiesCommon excel, UserService userService, MasterService masterService, SupplierService supplierService, IWebHostEnvironment hostingEnvironment) : base(userService)
        {
            _logger = logger;
            _excel = excel;
            _userService = userService;
            _masterService = masterService;
            _supplierService = supplierService;
            _hostingEnvironment = hostingEnvironment;
        }

        private void SetPermission()
        {
            _SetViewBagCurrentUserMenu((long)Models.ViewModels.Base.BaseConst.MENU_DEFINITION.SUPPLIER);
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

            return View();
        }

        public IActionResult SearchData(ParamSearchSupplierViewModel param)
        {
            SetPermission();

            if (!GetAction(Constants.Action.View))
            {
                return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
            }

            var result = _supplierService.GetSearchSupplierListData(this._Username(), param);
            if (result.isCompleted && result.data.Count > 0)
            {
                return Json(new ResultJqueryDataTable<ResultSearchSupplierViewModel>()
                {
                    draw = param.draw,
                    data = result.data,
                    recordsTotal = result.data[0].total_record,
                    recordsFiltered = result.data[0].total_record
                });
            }
            else
            {
                return Json(new ResultJqueryDataTable<ResultSearchSupplierViewModel>()
                {
                    draw = param.draw,
                    data = result.data,
                    recordsTotal = 0,
                    recordsFiltered = 0
                });
            }
        }

        public IActionResult SupplierInfo(string mode, string supplierCode)
        {
            SetPermission();
            ResultGetSupplierInfoViewModel resultObj = new ResultGetSupplierInfoViewModel();
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
                        ParamSupplierInfo param = new ParamSupplierInfo();
                        param.supplier_code = supplierCode;
                        var result = _supplierService.GetSupplierInfo(this._Username(), param);
                        if (result.isCompleted)
                        {
                            resultObj = result.data;
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

        public IActionResult SaveSupplierInfo(ResultGetSupplierInfoViewModel model)
        {
            SetPermission();
            try
            {
                if (model.mode != Constants.Mode.Created && model.mode != Constants.Mode.Updated)
                {
                    return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
                }

                if (!GetAction(Constants.Action.Created) && model.mode == Constants.Mode.Created)
                {
                    return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
                }

                if (!GetAction(Constants.Action.Edit) && model.mode == Constants.Mode.Updated)
                {
                    return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
                }

                switch (model.mode)
                {
                    case Constants.Mode.Created:
                        if (CheckDupSupplierNameOrSupplierCode(model.supplier_name, model.supplier_code))
                        {
                            return Json(new { status = Constants.Result.Invalid, message = "Supplier Name 0r Supplier Code has Already in System." });
                        }
                        else
                        {
                            var resCreate = _supplierService.CreateSupplierInfo(this._Username(), model);
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
                        if (CheckDupSupplierNameOrSupplierCode(model.supplier_name, null))
                        {
                            return Json(new { status = Constants.Result.Invalid, message = "Sender Name 0r Supplier Code has Already in System." });
                        }
                        else
                        {
                            var resUpdate = _supplierService.UpdateSupplierInfo(this._Username(), model);
                            if (resUpdate.data)
                            {
                                return Json(new { status = Constants.Result.Success });
                            }
                            else
                            {
                                return Json(new { status = Constants.Result.Invalid, message = resUpdate.message });
                            }
                        }
                    default:
                        return Json(new { status = Constants.Result.Invalid, message = Constants.Message.Invalid });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = Constants.Result.Error, message = ex.Message });
            }
        }

        public IActionResult DeleteItem(string mode, int supplierCode)
        {
            SetPermission();
            try
            {
                var resDelete = _supplierService.Delete(this._Username(), new ResultGetSupplierInfoViewModel() { supplier_code = supplierCode });
                if (resDelete.data)
                {
                    return Json(new { status = Constants.Result.Success });
                }
                else
                {
                    return Json(new { status = Constants.Result.Invalid, message = resDelete.message });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = Constants.Result.Error, message = ex.Message });
            }
        }

        private bool CheckDupSupplierNameOrSupplierCode(string supplierName, int? supplierCode)
        {
            var itmInfo = _supplierService.GetSupplierListData(this._Username()).data
                .Where(i => i.supplier_name == supplierName || i.supplier_code == supplierCode).FirstOrDefault();
            if (itmInfo != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public IActionResult ActionCreateExcelFile(ParamSearchSupplierViewModel param)
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
                var result = _supplierService.GetSearchSupplierListData(this._Username(), param);
                if (result.isCompleted && result.data.Count > 0)
                {
                    var excelFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "_Export\\Supplier");
                    if (!Directory.Exists(excelFolder))
                    {
                        Directory.CreateDirectory(excelFolder);
                    }
                    excelFile = Path.Combine(excelFolder, "supplier.xlsx");
                    Workbook workbook = new Workbook();
                    Worksheet worksheets = workbook.Worksheets.Add("Supplier");
                    try
                    {
                        worksheets.Range["A1"].Value = "Supplier Code";
                        worksheets.Range["B1"].Value = "Supplier Name";

                        worksheets.Range["A1:B1"].Style.Font.Color = Color.White;
                        worksheets.Range["A1:B1"].Style.Color = Color.Gray;
                        worksheets.Range["A1:B1"].Style.HorizontalAlignment = HorizontalAlignType.Center;
                        worksheets.Range["A1:B1"].Style.VerticalAlignment = VerticalAlignType.Center;

                        for (int i = 0; i < result.data.Count; i++)
                        {
                            var cellNo = i + 2;
                            worksheets.Range["A" + cellNo].Value = "'" + result.data[i].supplier_code.ToString();
                            worksheets.Range["B" + cellNo].Value = result.data[i].supplier_name;
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
                return Json(new { status = Constants.Result.Success, path = excelFile, fileName = "supplier.xlsx" });
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
