using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Spire.Xls;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.ViewModels.Base;
using WeightScaleGen2.BGC.Models.ViewModels.WeightHistory;
using WeightScaleGen2.BGC.Web.Common;
using WeightScaleGen2.BGC.Web.Controllers.Base;
using WeightScaleGen2.BGC.Web.Services;

namespace WeightScaleGen2.BGC.Web.Controllers
{
    //[Authorize]
    public class WeightHistoryController : BaseController
    {
        private readonly ILogger<WeightHistoryController> _logger;
        private readonly IExcelUtilitiesCommon _excel;
        private readonly UserService _userService;
        private readonly MasterService _masterService;
        private readonly WeightHistoryService _weightHistoryService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public WeightHistoryController(ILogger<WeightHistoryController> logger, IExcelUtilitiesCommon excel, UserService userService, MasterService masterService, WeightHistoryService weightHistoryService, IWebHostEnvironment hostingEnvironment) : base(userService)
        {
            _logger = logger;
            _excel = excel;
            _userService = userService;
            _masterService = masterService;
            _weightHistoryService = weightHistoryService;
            _hostingEnvironment = hostingEnvironment;
        }

        private void SetPermission()
        {
            _SetViewBagCurrentUserMenu((long)Models.ViewModels.Base.BaseConst.MENU_DEFINITION.WEIGHT_HISTORY);
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

        public IActionResult SearchData(ParamSearchWeightHistoryViewModel param)
        {
            SetPermission();

            if (!GetAction(Constants.Action.View))
            {
                return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
            }

            var result = _weightHistoryService.GetSearchWeightHistoryListData(this._Username(), param);
            if (result.isCompleted && result.data.Count > 0)
            {
                return Json(new ResultJqueryDataTable<ResultSearchWeightHistoryViewModel>()
                {
                    draw = param.draw,
                    data = result.data,
                    recordsTotal = result.data[0].total_record,
                    recordsFiltered = result.data[0].total_record
                });
            }
            else
            {
                return Json(new ResultJqueryDataTable<ResultSearchWeightHistoryViewModel>()
                {
                    draw = param.draw,
                    data = result.data,
                    recordsTotal = 0,
                    recordsFiltered = 0
                });
            }
        }

        public IActionResult WeightHistoryInfo(string mode, int id)
        {
            SetPermission();
            ResultGetWeightHistoryInfoViewModel resultObj = new ResultGetWeightHistoryInfoViewModel();
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
                        ParamWeightHistoryInfo param = new ParamWeightHistoryInfo();
                        param.id = id;
                        var result = _weightHistoryService.GetWeightHistoryInfo(this._Username(), param);
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

        public IActionResult ActionCreateExcelFile(ParamSearchWeightHistoryViewModel param)
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
                var result = _weightHistoryService.GetSearchWeightHistoryListData(this._Username(), param);
                if (result.isCompleted && result.data.Count > 0)
                {
                    var excelFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "_Export\\WeightHistory");
                    if (!Directory.Exists(excelFolder))
                    {
                        Directory.CreateDirectory(excelFolder);
                    }
                    excelFile = Path.Combine(excelFolder, "weight_history.xlsx");
                    Workbook workbook = new Workbook();
                    Worksheet worksheets = workbook.Worksheets.Add("WeightHistory");
                    try
                    {
                        worksheets.Range["A1"].Value = "เลขที่ใบชั่งออก";
                        worksheets.Range["B1"].Value = "เลขที่ใบชั่งเข้า";
                        worksheets.Range["C1"].Value = "น้ำหนักชั่งออก";
                        worksheets.Range["D1"].Value = "น้ำหนักชั่งเข้า";
                        worksheets.Range["E1"].Value = "น้ำหนักที่รับ";
                        worksheets.Range["F1"].Value = "รหัสผู้ส่ง";
                        worksheets.Range["G1"].Value = "ชื่อผู้ส่ง";
                        worksheets.Range["H1"].Value = "รหัสสินค้า";
                        worksheets.Range["I1"].Value = "ชื่อสินค้า";
                        worksheets.Range["J1"].Value = "รหัสสินค้า JDE";
                        worksheets.Range["K1"].Value = "ประเภทการชั่ง";
                        worksheets.Range["L1"].Value = "ทะเบียนรถ";
                        worksheets.Range["M1"].Value = "วันที่ชั่งออก";
                        worksheets.Range["N1"].Value = "รหัสบริษัท";
                        worksheets.Range["O1"].Value = "ผู้แก้ไข 1";
                        worksheets.Range["P1"].Value = "ผู้แก้ไข 2";
                        worksheets.Range["Q1"].Value = "ผู้แก้ไข 3";
                        worksheets.Range["R1"].Value = "SG BG";
                        worksheets.Range["S1"].Value = "SG ผู้ส่ง";
                        worksheets.Range["T1"].Value = "API BG";
                        worksheets.Range["U1"].Value = "API ผู้ส่ง";
                        worksheets.Range["V1"].Value = "Temp BG";
                        worksheets.Range["W1"].Value = "Temp ผู้ส่ง";
                        worksheets.Range["X1"].Value = "หมายเหตุ 1";
                        worksheets.Range["Y1"].Value = "หมายเหตุ 2";
                        worksheets.Range["Z1"].Value = "เลขเอกสารผู้ส่ง";
                        worksheets.Range["AA1"].Value = "วันที่ชั่งเข้า";
                        worksheets.Range["AB1"].Value = "ผู้ชั่ง";
                        worksheets.Range["AC1"].Value = "เลขที่ PO";
                        worksheets.Range["AD1"].Value = "PO Type";
                        worksheets.Range["AE1"].Value = "น้ำหนักที่ผู้ส่งวัด";

                        worksheets.Range["A1:AE1"].Style.Font.Color = Color.White;
                        worksheets.Range["A1:AE1"].Style.Color = Color.Gray;
                        worksheets.Range["A1:AE1"].Style.HorizontalAlignment = HorizontalAlignType.Center;
                        worksheets.Range["A1:AE1"].Style.VerticalAlignment = VerticalAlignType.Center;

                        for (int i = 0; i < result.data.Count; i++)
                        {
                            var cellNo = i + 2;
                            worksheets.Range["A" + cellNo].Value = result.data[i].weight_out_no;
                            worksheets.Range["B" + cellNo].Value = result.data[i].weight_in_no;
                            worksheets.Range["C" + cellNo].Value = "'" + result.data[i].before_weight_out.ToString("#,##0.00");
                            worksheets.Range["D" + cellNo].Value = "'" + result.data[i].weight_in.ToString("#,##0.00");
                            worksheets.Range["E" + cellNo].Value = "'" + result.data[i].weight_receive.ToString("#,##0.00");
                            worksheets.Range["F" + cellNo].Value = "'" + result.data[i].supplier_code.ToString();
                            worksheets.Range["G" + cellNo].Value = result.data[i].supplier_name;
                            worksheets.Range["H" + cellNo].Value = result.data[i].item_code;
                            worksheets.Range["I" + cellNo].Value = result.data[i].item_name;
                            worksheets.Range["J" + cellNo].Value = result.data[i].item_remark;
                            worksheets.Range["K" + cellNo].Value = result.data[i].weight_out_type;
                            worksheets.Range["L" + cellNo].Value = result.data[i].car_license;
                            worksheets.Range["M" + cellNo].Value = result.data[i].weight_out_date.Value.ToString("dd/MM/yyyy HH:mm:ss");
                            worksheets.Range["N" + cellNo].Value = result.data[i].company;
                            worksheets.Range["O" + cellNo].Value = result.data[i].user_edit_1;
                            worksheets.Range["P" + cellNo].Value = result.data[i].user_edit_2;
                            worksheets.Range["Q" + cellNo].Value = result.data[i].user_edit_3;
                            worksheets.Range["R" + cellNo].Value = "'" + result.data[i].sg_bg.ToString("#,##0.00");
                            worksheets.Range["S" + cellNo].Value = "'" + result.data[i].sg_supplier.ToString("#,##0.00");
                            worksheets.Range["T" + cellNo].Value = "'" + result.data[i].api_bg.ToString("#,##0.00");
                            worksheets.Range["U" + cellNo].Value = "'" + result.data[i].api_supplier.ToString("#,##0.00");
                            worksheets.Range["V" + cellNo].Value = "'" + result.data[i].temp_bg.ToString("#,##0.00");
                            worksheets.Range["W" + cellNo].Value = "'" + result.data[i].temp_supplier.ToString("#,##0.00");
                            worksheets.Range["X" + cellNo].Value = result.data[i].remark_1;
                            worksheets.Range["Y" + cellNo].Value = result.data[i].remark_2;
                            worksheets.Range["Z" + cellNo].Value = result.data[i].document_def;
                            worksheets.Range["AA" + cellNo].Value = result.data[i].weight_in_date.Value.ToString("dd/MM/yyyy HH:mm:ss");
                            worksheets.Range["AB" + cellNo].Value = result.data[i].user_id;
                            worksheets.Range["AC" + cellNo].Value = result.data[i].document_po.ToString();
                            worksheets.Range["AD" + cellNo].Value = result.data[i].doc_type_po.ToString();
                            worksheets.Range["AE" + cellNo].Value = "'" + result.data[i].weight_by_supplier.ToString("#,##0.00");
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
                return Json(new { status = Constants.Result.Success, path = excelFile, fileName = "weight_history.xlsx" });
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
