using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Spire.Xls;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.ViewModels.WeightSummaryDay;
using WeightScaleGen2.BGC.Web.Common;
using WeightScaleGen2.BGC.Web.Controllers.Base;
using WeightScaleGen2.BGC.Web.Services;

namespace WeightScaleGen2.BGC.Web.Controllers
{
    //[Authorize]
    public class WeightSummaryDayController : BaseController
    {
        private readonly ILogger<WeightSummaryDayController> _logger;
        private readonly IExcelUtilitiesCommon _excel;
        private readonly UserService _userService;
        private readonly MasterService _masterService;
        private readonly WeightSummaryDayService _weightSummaryDayService;
        private readonly ItemMasterService _itemMasterService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public WeightSummaryDayController(ILogger<WeightSummaryDayController> logger, IExcelUtilitiesCommon excel, UserService userService, MasterService masterService, WeightSummaryDayService weightSummaryDayService, ItemMasterService itemMasterService, IWebHostEnvironment hostingEnvironment) : base(userService)
        {
            _logger = logger;
            _excel = excel;
            _userService = userService;
            _masterService = masterService;
            _weightSummaryDayService = weightSummaryDayService;
            _itemMasterService = itemMasterService;
            _hostingEnvironment = hostingEnvironment;
        }

        private void SetPermission()
        {
            _SetViewBagCurrentUserMenu((long)Models.ViewModels.Base.BaseConst.MENU_DEFINITION.WEIGHT_SUMMARY_DAY);
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

        //public IActionResult SearchData(ParamSearchWeightSummaryDayViewModel param)
        //{
        //    SetPermission();

        //    if (!GetAction(Constants.Action.View))
        //    {
        //        return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
        //    }

        //    var result = _weightSummaryDayService.GetSearchListWeightSummaryDay(this._Username(), param);
        //    if (result.isCompleted && result.data.Count > 0)
        //    {
        //        return Json(new ResultJqueryDataTable<ResultSearchWeightSummaryDayViewModel>()
        //        {
        //            draw = param.draw,
        //            data = result.data,
        //            recordsTotal = result.data[0].total_record,
        //            recordsFiltered = result.data[0].total_record
        //        });
        //    }
        //    else
        //    {
        //        return Json(new ResultJqueryDataTable<ResultSearchWeightSummaryDayViewModel>()
        //        {
        //            draw = param.draw,
        //            data = result.data,
        //            recordsTotal = 0,
        //            recordsFiltered = 0
        //        });
        //    }
        //}

        public IActionResult ActionCreateExcelFile(ParamSearchWeightSummaryDayViewModel param)
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
                var result = _weightSummaryDayService.GetSearchListWeightSummaryDay(this._Username(), param);
                if (result.isCompleted && result.data.Count > 1)
                {
                    var excelFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "_Export\\WeightSummaryDay");
                    if (!Directory.Exists(excelFolder))
                    {
                        Directory.CreateDirectory(excelFolder);
                    }
                    excelFile = Path.Combine(excelFolder, "weight_summary_day.xlsx");
                    Workbook workbook = new Workbook();
                    Worksheet worksheets = workbook.Worksheets.Add("WeightSummaryDay");
                    try
                    {
                        worksheets.Range["A1"].Value = "รหัสสินค้า";
                        worksheets.Range["B1"].Value = "ชื่อสินค้า";
                        worksheets.Range["C1"].Value = "รหัสผู้ส่ง";
                        worksheets.Range["D1"].Value = "ชื่อผู้ส่ง";
                        worksheets.Range["E1"].Value = "จำนวน";
                        worksheets.Range["F1"].Value = "น้ำหนัก";

                        worksheets.Range["A1:F1"].Style.Font.Color = Color.White;
                        worksheets.Range["A1:F1"].Style.Color = Color.Gray;
                        worksheets.Range["A1:F1"].Style.HorizontalAlignment = HorizontalAlignType.Center;
                        worksheets.Range["A1:F1"].Style.VerticalAlignment = VerticalAlignType.Center;

                        for (int i = 0; i < result.data.Count; i++)
                        {
                            var cellNo = i + 2;
                            worksheets.Range["A" + cellNo].Value = result.data[i].item_code;
                            worksheets.Range["B" + cellNo].Value = result.data[i].item_name;
                            worksheets.Range["C" + cellNo].Value = result.data[i].supplier_code != 0 ? "'" + result.data[i].supplier_code : " ";
                            worksheets.Range["D" + cellNo].Value = result.data[i].supplier_name;
                            worksheets.Range["E" + cellNo].Value = "'" + result.data[i].weight_count.ToString();
                            worksheets.Range["F" + cellNo].Value = "'" + result.data[i].sum_weight_out.ToString("#,##0.00");
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
                return Json(new { status = Constants.Result.Success, path = excelFile, fileName = "weight_summary_day.xlsx" });
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

        public async Task<IActionResult> ActionCreatePdfFile(ParamSearchWeightSummaryDayViewModel param)
        {
            try
            {
                var result = _weightSummaryDayService.GetSearchListWeightSummaryDay(this._Username(), param);
                if (result.isCompleted && result.data.Count > 1)
                {
                    DateTime dateNow = DateTime.Now;
                    var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                    var pathTemplate = "";
                    pathTemplate = configuration.GetSection("Template").GetSection("TemplateWeightSummaryDay").Value;
                    var pathTempSave = configuration.GetSection("Template").GetSection("TemplateWeightSummaryDaySave").Value;

                    string fileTemplate = $@"{pathTemplate}";
                    string fileData = $@"{pathTempSave}weight_summary_day_{dateNow.ToString("yyyyMMddHHmmss")}.xlsx";
                    string fileOutputPDF = $@"{pathTempSave}_{dateNow.ToString("yyyyMMddHHmmss")}.pdf";

                    using (Workbook workbookTemplate = new Workbook())
                    {
                        // load execl file template
                        workbookTemplate.LoadFromFile(fileTemplate, ExcelVersion.Version2016);

                        // init data
                        Worksheet sheet = workbookTemplate.Worksheets[0];

                        // Set the page setup to landscape orientation
                        sheet.PageSetup.Orientation = PageOrientationType.Landscape;

                        // Set paper size to A4 (PaperSize = 9 corresponds to A4)
                        sheet.PageSetup.PaperSize = Spire.Xls.PaperSizeType.PaperA4;

                        // Set margins to 0
                        sheet.PageSetup.LeftMargin = 0;
                        sheet.PageSetup.RightMargin = 0;
                        sheet.PageSetup.TopMargin = 0;
                        sheet.PageSetup.BottomMargin = 0;

                        // Optionally center content horizontally and vertically
                        //sheet.PageSetup.CenterHorizontally = true;
                        //sheet.PageSetup.CenterVertically = true;

                        // Write Data
                        string dateValue;
                        if (param.start_date.HasValue && param.start_date.HasValue) {
                            dateValue = "ช่วงวันที่ : " + param.start_date.Value.ToString("dd/MM/yyyy") + " - " + param.end_date.Value.ToString("dd/MM/yyyy");
                        }
                        else if (param.start_date.HasValue)
                        {
                            dateValue = "วันที่ : " + param.start_date.Value.ToString("dd/MM/yyyy");
                        }
                        else if (param.end_date.HasValue)
                        {
                            dateValue = "วันที่ : " + param.end_date.Value.ToString("dd/MM/yyyy");
                        }
                        else 
                        {
                            dateValue = "ทั้งหมด";
                        }

                        sheet[3, 2].Text = "'" + $@"{dateValue}";
                        sheet[4, 3].Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        sheet[4, 8].Text = this._Username();
                        sheet[10, 4].Text = "กลุ่มสินค้า : " + result.data[0].group_name;

                        var start = 11;
                        foreach (var i in result.data)
                        {
                            sheet[start, 2].Text = i.item_code;
                            sheet[start, 3].Text = i.item_name;
                            sheet[start + 1, 2].Text = i.supplier_code != 0 ? "'" + i.supplier_code.ToString() : "รวมทั้งหมด";
                            sheet[start + 1, 3].Text = !String.IsNullOrEmpty(i.supplier_name) && i.supplier_name != "รวมทั้งหมด" ? i.supplier_name : " ";
                            sheet[start + 1, 5].Text = "'" + i.weight_count.ToString();
                            sheet[start + 1, 6].Text = "'" + i.sum_weight_out.ToString("#,##0.00");
                            sheet[start + 1, 11].Text = "'" + i.weight_count.ToString();
                            sheet[start + 1, 12].Text = "'" + i.sum_weight_out.ToString("#,##0.00");
                            start = start + 2;
                        }

                        // save data to excel
                        workbookTemplate.SaveToFile(fileData, ExcelVersion.Version2016);
                        workbookTemplate.Dispose();
                    }

                    // load excel file data
                    using (Workbook workbookData = new Workbook())
                    {
                        workbookData.LoadFromFile(fileData, ExcelVersion.Version2016);
                        workbookData.SaveToFile(fileOutputPDF, FileFormat.PDF);
                        workbookData.Dispose();
                    }

                    var fileInfo = new FileInfo(fileOutputPDF);
                    if (fileInfo != null)
                    {
                        var memory = new MemoryStream();
                        using (var stream = new FileStream(fileOutputPDF, FileMode.Open))
                        {
                            await stream.CopyToAsync(memory);
                        }
                        memory.Position = 0;


                        string filename = $"weight_summary_day.pdf";

                        //ลบ temp ไฟล์
                        System.IO.File.Delete(fileData);
                        System.IO.File.Delete(fileOutputPDF);


                        return File(memory, "application/pdf", filename);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return Json(new { status = Constants.Result.Invalid, message = result.message[0] });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = Constants.Result.Invalid, message = ex.Message });
            }
        }
    }
}
