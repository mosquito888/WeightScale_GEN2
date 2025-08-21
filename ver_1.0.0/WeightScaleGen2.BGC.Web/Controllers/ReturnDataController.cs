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
using WeightScaleGen2.BGC.Models.ViewModels.ReturnData;
using WeightScaleGen2.BGC.Web.Common;
using WeightScaleGen2.BGC.Web.Controllers.Base;
using WeightScaleGen2.BGC.Web.Services;

namespace WeightScaleGen2.BGC.Web.Controllers
{
    //[Authorize]
    public class ReturnDataController : BaseController
    {
        private readonly ILogger<ReturnDataController> _logger;
        private readonly IExcelUtilitiesCommon _excel;
        private readonly UserService _userService;
        private readonly MasterService _masterService;
        private readonly ReturnDataService _returnDataService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ReturnDataController(ILogger<ReturnDataController> logger, IExcelUtilitiesCommon excel, UserService userService, MasterService masterService, ReturnDataService returnDataService, IWebHostEnvironment hostingEnvironment) : base(userService)
        {
            _logger = logger;
            _excel = excel;
            _userService = userService;
            _masterService = masterService;
            _returnDataService = returnDataService;
            _hostingEnvironment = hostingEnvironment;
        }

        private void SetPermission()
        {
            _SetViewBagCurrentUserMenu((long)Models.ViewModels.Base.BaseConst.MENU_DEFINITION.RETURN_DATA);
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

        public IActionResult SearchData(ParamSearchReturnDataViewModel param)
        {
            SetPermission();

            if (!GetAction(Constants.Action.View))
            {
                return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
            }

            var result = _returnDataService.GetSearchListReturnData(this._Username(), param);
            if (result.isCompleted && result.data.Count > 0)
            {
                return Json(new ResultJqueryDataTable<ResultSearchReturnDataViewModel>()
                {
                    draw = param.draw,
                    data = result.data,
                    recordsTotal = result.data[0].total_record,
                    recordsFiltered = result.data[0].total_record
                });
            }
            else
            {
                return Json(new ResultJqueryDataTable<ResultSearchReturnDataViewModel>()
                {
                    draw = param.draw,
                    data = result.data,
                    recordsTotal = 0,
                    recordsFiltered = 0
                });
            }
        }

        public IActionResult SendData(ParamSearchReturnDataViewModel param)
        {
            SetPermission();

            if (!GetAction(Constants.Action.View))
            {
                return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
            }

            param.start = 0;
            param.draw = int.MaxValue;
            param.length = int.MaxValue;
            var result = _returnDataService.PostDataToSAP(this._Username(), param);
            if (result.isCompleted)
            {
                return Json(new { status = Constants.Result.Success });

            }
            else
            {
                return Json(new { status = Constants.Result.Error, message = result.message[0] });
            }
        }

        public IActionResult ActionCreateExcelFile(ParamSearchReturnDataViewModel param)
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
                var result = _returnDataService.GetSearchListReturnData(this._Username(), param);
                if (result.isCompleted && result.data.Count > 0)
                {
                    var excelFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "_Export\\ReturnData");
                    if (!Directory.Exists(excelFolder))
                    {
                        Directory.CreateDirectory(excelFolder);
                    }
                    excelFile = Path.Combine(excelFolder, "return_data.xlsx");
                    Workbook workbook = new Workbook();
                    Worksheet worksheets = workbook.Worksheets.Add("ReturnData");
                    try
                    {
                        worksheets.Range["A1"].Value = "ใบชั่งออก";
                        worksheets.Range["B1"].Value = "ใบชั่งเข้า";
                        worksheets.Range["C1"].Value = "ลำดับการทำงาน";
                        worksheets.Range["D1"].Value = "ประเภทการชั่ง";
                        worksheets.Range["E1"].Value = "วันที่ชั่ง";
                        worksheets.Range["F1"].Value = "วันที่รับ";
                        worksheets.Range["G1"].Value = "เอกสารอ้างอิง";
                        worksheets.Range["H1"].Value = "ประเภทการทำงาน";
                        worksheets.Range["I1"].Value = "รหัสสินค้า";
                        worksheets.Range["J1"].Value = "บริษัท";
                        worksheets.Range["K1"].Value = "สถานที่จัดเก็บ";
                        worksheets.Range["L1"].Value = "ประเภทสินค้า";
                        worksheets.Range["M1"].Value = "หมายเหตุ";
                        worksheets.Range["N1"].Value = "ใบสั่งซื้อ";
                        worksheets.Range["O1"].Value = "ลำดับใบสั่งซื้อ";
                        worksheets.Range["P1"].Value = "ทะเบียนรถ";
                        worksheets.Range["Q1"].Value = "น้ำหนักชั่งเข้า";
                        worksheets.Range["R1"].Value = "น้ำหนักชั่งออก";
                        worksheets.Range["S1"].Value = "น้ำหนักรับ";
                        worksheets.Range["T1"].Value = "น้ำหนักชั่งผู้ขาย";
                        worksheets.Range["U1"].Value = "น้ำหนักตีกลับ";
                        worksheets.Range["V1"].Value = "หน่วย";
                        worksheets.Range["W1"].Value = "วันเริ่มต้นใบอนุญาต";
                        worksheets.Range["X1"].Value = "วันสิ้นสุดใบอนุญาต";
                        worksheets.Range["Y1"].Value = "หมายเลขใบอนุญาต";
                        worksheets.Range["Z1"].Value = "ประเภทข้อความ";
                        worksheets.Range["AA1"].Value = "ข้อความจาก SAP";
                        worksheets.Range["AB1"].Value = "ส่งข้อมูล";
                        worksheets.Range["AC1"].Value = "หมายเลข GR";
                        worksheets.Range["AD1"].Value = "ปีของ GR";

                        worksheets.Range["A1:AD1"].Style.Font.Color = Color.White;
                        worksheets.Range["A1:AD1"].Style.Color = Color.Gray;
                        worksheets.Range["A1:AD1"].Style.HorizontalAlignment = HorizontalAlignType.Center;
                        worksheets.Range["A1:AD1"].Style.VerticalAlignment = VerticalAlignType.Center;

                        for (int i = 0; i < result.data.Count; i++)
                        {
                            var cellNo = i + 2;
                            worksheets.Range["A" + cellNo].Value = result.data[i].weight_out_no;
                            worksheets.Range["B" + cellNo].Value = result.data[i].weight_in_no;
                            worksheets.Range["C" + cellNo].Value = "'" + result.data[i].sequence.ToString();
                            worksheets.Range["D" + cellNo].Value = "'" + result.data[i].gr_type.ToString();
                            worksheets.Range["E" + cellNo].Value = result.data[i].doc_date.ToString("dd/MM/yyyy");
                            worksheets.Range["F" + cellNo].Value = result.data[i].post_date.ToString("dd/MM/yyyy");
                            worksheets.Range["G" + cellNo].Value = result.data[i].ref_doc;
                            worksheets.Range["H" + cellNo].Value = result.data[i].good_movement;
                            worksheets.Range["I" + cellNo].Value = result.data[i].material;
                            worksheets.Range["J" + cellNo].Value = result.data[i].plant;
                            worksheets.Range["K" + cellNo].Value = result.data[i].sloc;
                            worksheets.Range["L" + cellNo].Value = result.data[i].stock_type;
                            worksheets.Range["M" + cellNo].Value = result.data[i].item_text;
                            worksheets.Range["N" + cellNo].Value = result.data[i].po_number;
                            worksheets.Range["O" + cellNo].Value = "'" + result.data[i].po_line_number.ToString();
                            worksheets.Range["P" + cellNo].Value = result.data[i].truck_no;
                            worksheets.Range["Q" + cellNo].Value = "'" + result.data[i].weight_in.ToString("#,##0.00");
                            worksheets.Range["R" + cellNo].Value = "'" + result.data[i].weight_out.ToString("#,##0.00");
                            worksheets.Range["S" + cellNo].Value = "'" + result.data[i].weight_rec.ToString("#,##0.00");
                            worksheets.Range["T" + cellNo].Value = "'" + result.data[i].weight_vendor.ToString("#,##0.00");
                            worksheets.Range["U" + cellNo].Value = "'" + result.data[i].weight_reject.ToString("#,##0.00");
                            worksheets.Range["V" + cellNo].Value = result.data[i].weight_unit;
                            worksheets.Range["W" + cellNo].Value = result.data[i].doc_start.Value.ToString("dd/MM/yyyy HH:mm:ss");
                            worksheets.Range["X" + cellNo].Value = result.data[i].doc_stop.Value.ToString(@"dd/MM/yyyy HH:mm:ss");
                            worksheets.Range["Y" + cellNo].Value = result.data[i].doc_send;
                            worksheets.Range["Z" + cellNo].Value = result.data[i].message_type;
                            worksheets.Range["AA" + cellNo].Value = result.data[i].message;
                            worksheets.Range["AB" + cellNo].Value = result.data[i].send_data;
                            worksheets.Range["AC" + cellNo].Value = result.data[i].material_document;
                            worksheets.Range["AD" + cellNo].Value = "'" + result.data[i].document_year.ToString();
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
                return Json(new { status = Constants.Result.Success, path = excelFile, fileName = "return_data.xlsx" });
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
