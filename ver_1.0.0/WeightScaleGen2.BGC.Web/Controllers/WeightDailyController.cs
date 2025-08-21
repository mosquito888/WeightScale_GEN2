using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Spire.Xls;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.ViewModels.Base;
using WeightScaleGen2.BGC.Models.ViewModels.WeightDaily;
using WeightScaleGen2.BGC.Web.Common;
using WeightScaleGen2.BGC.Web.Controllers.Base;
using WeightScaleGen2.BGC.Web.Services;

namespace WeightScaleGen2.BGC.Web.Controllers
{
    //[Authorize]
    public class WeightDailyController : BaseController
    {
        private readonly ILogger<WeightDailyController> _logger;
        private readonly IExcelUtilitiesCommon _excel;
        private readonly UserService _userService;
        private readonly MasterService _masterService;
        private readonly WeightDailyService _weightDailyService;
        private readonly ItemMasterService _itemMasterService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public WeightDailyController(ILogger<WeightDailyController> logger, IExcelUtilitiesCommon excel, UserService userService, MasterService masterService, WeightDailyService weightDailyService, ItemMasterService itemMasterService, IWebHostEnvironment hostingEnvironment) : base(userService)
        {
            _logger = logger;
            _excel = excel;
            _userService = userService;
            _masterService = masterService;
            _weightDailyService = weightDailyService;
            _itemMasterService = itemMasterService;
            _hostingEnvironment = hostingEnvironment;
        }

        private void SetPermission()
        {
            _SetViewBagCurrentUserMenu((long)Models.ViewModels.Base.BaseConst.MENU_DEFINITION.WEIGHT_DAILY);
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

        public IActionResult SearchData(ParamSearchWeightDailyViewModel param)
        {
            SetPermission();

            if (!GetAction(Constants.Action.View))
            {
                return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
            }

            var result = _weightDailyService.GetSearchListWeightDaily(this._Username(), param);
            if (result.isCompleted && result.data.Count > 0)
            {
                return Json(new ResultJqueryDataTable<ResultSearchWeightDailyViewModel>()
                {
                    draw = param.draw,
                    data = result.data,
                    recordsTotal = result.data[0].total_record,
                    recordsFiltered = result.data[0].total_record
                });
            }
            else
            {
                return Json(new ResultJqueryDataTable<ResultSearchWeightDailyViewModel>()
                {
                    draw = param.draw,
                    data = result.data,
                    recordsTotal = 0,
                    recordsFiltered = 0
                });
            }
        }

        public IActionResult ActionCreateExcelFile(ParamSearchWeightDailyViewModel param)
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
                var result = _weightDailyService.GetSearchListWeightDaily(this._Username(), param);
                if (result.isCompleted && result.data.Count > 0)
                {
                    var excelFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "_Export\\WeightDaily");
                    if (!Directory.Exists(excelFolder))
                    {
                        Directory.CreateDirectory(excelFolder);
                    }
                    excelFile = Path.Combine(excelFolder, "weight_daily.xlsx");
                    Workbook workbook = new Workbook();
                    Worksheet worksheets = workbook.Worksheets.Add("WeightDaily");
                    try
                    {
                        worksheets.Range["A1"].Value = "เลขที่ใบชั่งเข้า";
                        worksheets.Range["B1"].Value = "ทะเบียนรถ";
                        worksheets.Range["C1"].Value = "วันที่ชั่งเข้า";
                        worksheets.Range["D1"].Value = "เวลาชั่งเข้า";
                        worksheets.Range["E1"].Value = "น้ำหนัก(KG)";
                        worksheets.Range["F1"].Value = "วันที่ชั่งออก";
                        worksheets.Range["G1"].Value = "เวลาชั่งออก";
                        worksheets.Range["H1"].Value = "น้ำหนัก(KG)";
                        worksheets.Range["I1"].Value = "น้ำหนักสุทธิ";
                        worksheets.Range["J1"].Value = "ผู้ชั่ง";
                        worksheets.Range["K1"].Value = "เลขที่ใบชั่งออก";
                        worksheets.Range["L1"].Value = "สถานะ";
                        worksheets.Range["M1"].Value = "รหัสสินค้า";
                        worksheets.Range["N1"].Value = "ชื่อสินค้า";
                        worksheets.Range["O1"].Value = "รหัสผู้ส่ง";
                        worksheets.Range["P1"].Value = "ชื่อผู้ส่ง";
                        worksheets.Range["Q1"].Value = "ค่าชดเชย";
                        worksheets.Range["R1"].Value = "หมายเหตุ";

                        worksheets.Range["A1:R1"].Style.Font.Color = Color.White;
                        worksheets.Range["A1:R1"].Style.Color = Color.Gray;
                        worksheets.Range["A1:R1"].Style.HorizontalAlignment = HorizontalAlignType.Center;
                        worksheets.Range["A1:R1"].Style.VerticalAlignment = VerticalAlignType.Center;

                        for (int i = 0; i < result.data.Count; i++)
                        {
                            var cellNo = i + 2;
                            worksheets.Range["A" + cellNo].Value = result.data[i].weight_in_no;
                            worksheets.Range["B" + cellNo].Value = !String.IsNullOrEmpty(result.data[i].car_license) ? result.data[i].car_license : " ";
                            worksheets.Range["C" + cellNo].Value = result.data[i].weight_in_date.Value.ToString("dd/MM/yyyy");
                            worksheets.Range["D" + cellNo].Value = result.data[i].weight_in_date.Value.ToString("HH:mm:ss");
                            worksheets.Range["E" + cellNo].Value = "'" + result.data[i].weight_in.ToString("#,##0.00");
                            worksheets.Range["F" + cellNo].Value = result.data[i].weight_out_date.Value.ToString("dd/MM/yyyy");
                            worksheets.Range["G" + cellNo].Value = result.data[i].weight_out_date.Value.ToString("HH:mm:ss");
                            worksheets.Range["H" + cellNo].Value = "'" + result.data[i].before_weight_out.ToString("#,##0.00");
                            worksheets.Range["I" + cellNo].Value = "'" + result.data[i].weight_receive.ToString("#,##0.00");
                            worksheets.Range["J" + cellNo].Value = result.data[i].user_id;
                            worksheets.Range["K" + cellNo].Value = result.data[i].weight_out_no;
                            worksheets.Range["L" + cellNo].Value = result.data[i].status;
                            worksheets.Range["M" + cellNo].Value = result.data[i].item_code;
                            worksheets.Range["N" + cellNo].Value = result.data[i].item_name;
                            worksheets.Range["O" + cellNo].Value = "'" + result.data[i].supplier_code;
                            worksheets.Range["P" + cellNo].Value = result.data[i].supplier_name;
                            worksheets.Range["Q" + cellNo].Value = "'" + result.data[i].weight_diff.ToString("#,##0.00");
                            worksheets.Range["R" + cellNo].Value = result.data[i].remark_1;
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
                return Json(new { status = Constants.Result.Success, path = excelFile, fileName = "weight_daily.xlsx" });
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

        public async Task<IActionResult> ActionCreatePdfFile(ParamSearchWeightDailyViewModel param)
        {
            try
            {
                var result = _weightDailyService.GetSearchListWeightDaily(this._Username(), param);
                if (result.isCompleted && result.data.Count > 0)
                {
                    DateTime dateNow = DateTime.Now;
                    var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                    var pathTemplate = "";
                    pathTemplate = configuration.GetSection("Template").GetSection("TemplateWeightDaily").Value;
                    var pathTempSave = configuration.GetSection("Template").GetSection("TemplateWeightDailySave").Value;

                    string fileTemplate = $@"{pathTemplate}";
                    string fileData = $@"{pathTempSave}weight_daily_{dateNow.ToString("yyyyMMddHHmmss")}.xlsx";
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
                        sheet[3, 2].Text = "ช่วงวันที่ : " + result.data.FirstOrDefault().weight_in_date.Value.ToString("yyyyMMdd") + " - " + result.data.LastOrDefault().weight_in_date.Value.ToString("yyyyMMdd");
                        sheet[4, 4].Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        sheet[4, 9].Text = this._Username();

                        var start = 9;
                        foreach (var i in result.data)
                        {
                            sheet[start, 2].Text = i.weight_in_no;
                            sheet[start, 3].Text = !String.IsNullOrEmpty(i.car_license) ? i.car_license : " ";
                            sheet[start, 4].Text = i.weight_in_date.Value.ToString("dd/MM/yyyy");
                            sheet[start, 5].Text = i.weight_in_date.Value.ToString("HH:mm:ss");
                            sheet[start, 6].Text = "'" + i.weight_in.ToString("#,##0.00");
                            sheet[start, 7].Text = i.weight_out_date.Value.ToString("dd/MM/yyyy");
                            sheet[start, 8].Text = i.weight_out_date.Value.ToString("HH:mm:ss");
                            sheet[start, 9].Text = "'" + i.before_weight_out.ToString("#,##0.00");
                            sheet[start, 10].Text = "'" + i.weight_receive.ToString("#,##0.00");
                            sheet[start, 11].Text = i.user_id;
                            sheet[start, 12].Text = i.document_ref;
                            sheet[start + 1, 2].Text = i.weight_out_no;
                            sheet[start + 1, 3].Text = i.status;
                            sheet[start + 1, 4].Text = i.item_code;
                            sheet[start + 1, 5].Text = i.item_name;
                            sheet[start + 1, 7].Text = "'" + i.supplier_code.ToString();
                            sheet[start + 1, 8].Text = i.supplier_name;
                            sheet[start + 1, 10].Text = "'" + i.weight_diff.ToString("#,##0.00");
                            sheet[start + 1, 11].Text = !String.IsNullOrEmpty(i.remark_1) ? i.remark_1 : " ";
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


                        string filename = $"weight_daily_{result.data.FirstOrDefault().weight_in_date.Value.ToString("yyyyMMdd")}_{result.data.LastOrDefault().weight_out_date.Value.ToString("yyyyMMdd")}.pdf";

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
