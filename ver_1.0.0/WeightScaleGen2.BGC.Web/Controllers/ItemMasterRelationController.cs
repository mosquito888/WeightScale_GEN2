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
using WeightScaleGen2.BGC.Models.ViewModels.ItemMasterRelation;
using WeightScaleGen2.BGC.Web.Common;
using WeightScaleGen2.BGC.Web.Controllers.Base;
using WeightScaleGen2.BGC.Web.Services;

namespace WeightScaleGen2.BGC.Web.Controllers
{
    //[Authorize]
    public class ItemMasterRelationController : BaseController
    {
        private readonly ILogger<ItemMasterRelationController> _logger;
        private readonly IExcelUtilitiesCommon _excel;
        private readonly UserService _userService;
        private readonly MasterService _masterService;
        private readonly ItemMasterRelationService _itemMasterRelationService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ItemMasterRelationController(ILogger<ItemMasterRelationController> logger, IExcelUtilitiesCommon excel, UserService userService, MasterService masterService, ItemMasterRelationService itemMasterRelationService, IWebHostEnvironment hostingEnvironment) : base(userService)
        {
            _logger = logger;
            _excel = excel;
            _userService = userService;
            _masterService = masterService;
            _itemMasterRelationService = itemMasterRelationService;
            _hostingEnvironment = hostingEnvironment;
        }

        private void SetPermission()
        {
            _SetViewBagCurrentUserMenu((long)Models.ViewModels.Base.BaseConst.MENU_DEFINITION.ITEM_MASTER_RELATION);
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

        public IActionResult SearchData(ParamSearchItemMasterRelationViewModel param)
        {
            SetPermission();

            if (!GetAction(Constants.Action.View))
            {
                return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
            }

            var result = _itemMasterRelationService.GetSearchItemMasterRelationListData(this._Username(), param);
            if (result.isCompleted && result.data.Count > 0)
            {
                return Json(new ResultJqueryDataTable<ResultSearchItemMasterRelationViewModel>()
                {
                    draw = param.draw,
                    data = result.data,
                    recordsTotal = result.data[0].total_record,
                    recordsFiltered = result.data[0].total_record
                });
            }
            else
            {
                return Json(new ResultJqueryDataTable<ResultSearchItemMasterRelationViewModel>()
                {
                    draw = param.draw,
                    data = result.data,
                    recordsTotal = 0,
                    recordsFiltered = 0
                });
            }
        }

        public IActionResult ItemMasterRelationInfo(string mode, int id)
        {
            SetPermission();
            ResultGetItemMasterRelationInfoViewModel resultObj = new ResultGetItemMasterRelationInfoViewModel();
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
                        ParamItemMasterRelationInfo param = new ParamItemMasterRelationInfo();
                        param.id = id;
                        var result = _itemMasterRelationService.GetItemMasterRelationInfo(this._Username(), param);
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

        public IActionResult SaveItemMasterRelationInfo(ResultGetItemMasterRelationInfoViewModel model)
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
                        if (CheckDupItemCodeAndSuppliercode(model.item_code, model.supplier_code))
                        {
                            return Json(new { status = Constants.Result.Invalid, message = "Supplier Code has Already in Item Relation." });
                        }
                        else
                        {
                            var resCreate = _itemMasterRelationService.CreateItemMasterRelationInfo(this._Username(), model);
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
                        var resUpdate = _itemMasterRelationService.UpdateItemMasterRelationInfo(this._Username(), model);
                        if (resUpdate.data)
                        {
                            return Json(new { status = Constants.Result.Success });
                        }
                        else
                        {
                            return Json(new { status = Constants.Result.Invalid, message = resUpdate.message });
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

        public IActionResult DeleteItem(string mode, int id)
        {
            SetPermission();
            try
            {
                var resDelete = _itemMasterRelationService.Delete(this._Username(), new ResultGetItemMasterRelationInfoViewModel() { id = id });
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

        private bool CheckDupItemCodeAndSuppliercode(string itemCode, int supplierCode)
        {
            var itmInfo = _itemMasterRelationService.GetItemMasterRelationListData(this._Username()).data
                .Where(i => i.item_code == itemCode && i.supplier_code == supplierCode).FirstOrDefault();
            if (itmInfo != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public IActionResult ActionCreateExcelFile(ParamSearchItemMasterRelationViewModel param)
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
                var result = _itemMasterRelationService.GetSearchItemMasterRelationListData(this._Username(), param);
                if (result.isCompleted && result.data.Count > 0)
                {
                    var excelFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "_Export\\ItemMasterRelation");
                    if (!Directory.Exists(excelFolder))
                    {
                        Directory.CreateDirectory(excelFolder);
                    }
                    excelFile = Path.Combine(excelFolder, "item_master_relation.xlsx");
                    Workbook workbook = new Workbook();
                    Worksheet worksheets = workbook.Worksheets.Add("ItemMasterRelation");
                    try
                    {
                        worksheets.Range["A1"].Value = "Product Code";
                        worksheets.Range["B1"].Value = "Product Name";
                        worksheets.Range["C1"].Value = "Supplier Code";
                        worksheets.Range["D1"].Value = "Supplier Name";
                        worksheets.Range["E1"].Value = "Humidity";
                        worksheets.Range["F1"].Value = "Gravity";
                        worksheets.Range["G1"].Value = "Status";
                        worksheets.Range["H1"].Value = "Remark";
                        worksheets.Range["I1"].Value = "Other";

                        worksheets.Range["A1:I1"].Style.Font.Color = Color.White;
                        worksheets.Range["A1:I1"].Style.Color = Color.Gray;
                        worksheets.Range["A1:I1"].Style.HorizontalAlignment = HorizontalAlignType.Center;
                        worksheets.Range["A1:I1"].Style.VerticalAlignment = VerticalAlignType.Center;

                        for (int i = 0; i < result.data.Count; i++)
                        {
                            var cellNo = i + 2;
                            worksheets.Range["A" + cellNo].Value = result.data[i].item_code;
                            worksheets.Range["B" + cellNo].Value = result.data[i].item_name;
                            worksheets.Range["C" + cellNo].Value = "'" + result.data[i].supplier_code.ToString();
                            worksheets.Range["D" + cellNo].Value = result.data[i].supplier_name;
                            worksheets.Range["E" + cellNo].Value = result.data[i].humidity.ToString("#,##0.00");
                            worksheets.Range["F" + cellNo].Value = result.data[i].gravity.ToString("#,##0.00");
                            worksheets.Range["G" + cellNo].Value = result.data[i].status;
                            worksheets.Range["H" + cellNo].Value = result.data[i].remark_1;
                            worksheets.Range["I" + cellNo].Value = result.data[i].remark_2;
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
                return Json(new { status = Constants.Result.Success, path = excelFile, fileName = "item_master_relation.xlsx" });
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
