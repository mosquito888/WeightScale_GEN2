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
using WeightScaleGen2.BGC.Models.ViewModels.WeightCompare;
using WeightScaleGen2.BGC.Web.Common;
using WeightScaleGen2.BGC.Web.Controllers.Base;
using WeightScaleGen2.BGC.Web.Services;

namespace WeightScaleGen2.BGC.Web.Controllers
{
    //[Authorize]
    public class WeightCompareController : BaseController
    {
        private readonly ILogger<WeightCompareController> _logger;
        private readonly IExcelUtilitiesCommon _excel;
        private readonly UserService _userService;
        private readonly MasterService _masterService;
        private readonly WeightCompareService _weightCompareService;
        private readonly ItemMasterService _itemMasterService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public WeightCompareController(ILogger<WeightCompareController> logger, IExcelUtilitiesCommon excel, UserService userService, MasterService masterService, WeightCompareService weightCompareService, ItemMasterService itemMasterService, IWebHostEnvironment hostingEnvironment) : base(userService)
        {
            _logger = logger;
            _excel = excel;
            _userService = userService;
            _masterService = masterService;
            _weightCompareService = weightCompareService;
            _itemMasterService = itemMasterService;
            _hostingEnvironment = hostingEnvironment;
        }

        private void SetPermission()
        {
            _SetViewBagCurrentUserMenu((long)Models.ViewModels.Base.BaseConst.MENU_DEFINITION.WEIGHT_COMPARE);
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

        public IActionResult SearchData(ParamSearchWeightCompareViewModel param)
        {
            SetPermission();

            if (!GetAction(Constants.Action.View))
            {
                return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
            }

            var result = _weightCompareService.GetSearchListWeightCompare(this._Username(), param);
            if (result.isCompleted && result.data.Count > 0)
            {
                return Json(new ResultJqueryDataTable<ResultSearchWeightCompareViewModel>()
                {
                    draw = param.draw,
                    data = result.data,
                    recordsTotal = result.data[0].total_record,
                    recordsFiltered = result.data[0].total_record
                });
            }
            else
            {
                return Json(new ResultJqueryDataTable<ResultSearchWeightCompareViewModel>()
                {
                    draw = param.draw,
                    data = result.data,
                    recordsTotal = 0,
                    recordsFiltered = 0
                });
            }
        }

        public IActionResult ActionCreateExcelFile(ParamSearchWeightCompareViewModel param)
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
                var result = _weightCompareService.GetSearchListWeightCompare(this._Username(), param);
                if (result.isCompleted && result.data.Count > 1)
                {
                    var excelFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "_Export\\WeightCompare");
                    if (!Directory.Exists(excelFolder))
                    {
                        Directory.CreateDirectory(excelFolder);
                    }
                    excelFile = Path.Combine(excelFolder, "weight_compare.xlsx");
                    Workbook workbook = new Workbook();
                    Worksheet worksheets = workbook.Worksheets.Add("WeightCompare");
                    try
                    {
                        worksheets.Range["A1"].Value = "วันที่";
                        worksheets.Range["B1"].Value = "ทะเบียนรถ";
                        worksheets.Range["C1"].Value = "Invoice";
                        worksheets.Range["D1"].Value = "น้ำหนักสุทธิ";
                        worksheets.Range["E1"].Value = "น้ำหนักหักลด";
                        worksheets.Range["F1"].Value = "น้ำหนักที่รับ";
                        worksheets.Range["G1"].Value = "น้ำหนักผู้ส่ง";
                        worksheets.Range["H1"].Value = "น้ำหนักแตกต่าง";
                        worksheets.Range["I1"].Value = "%";
                        worksheets.Range["J1"].Value = "หมายเหตุ";

                        worksheets.Range["A1:J1"].Style.Font.Color = Color.White;
                        worksheets.Range["A1:J1"].Style.Color = Color.Gray;
                        worksheets.Range["A1:J1"].Style.HorizontalAlignment = HorizontalAlignType.Center;
                        worksheets.Range["A1:J1"].Style.VerticalAlignment = VerticalAlignType.Center;

                        for (int i = 0; i < result.data.Count; i++)
                        {
                            var cellNo = i + 2;
                            worksheets.Range["A" + cellNo].Value = result.data[i].date != null ? result.data[i].date.Value.ToString("dd/MM/yyyy") : " ";
                            worksheets.Range["B" + cellNo].Value = !String.IsNullOrEmpty(result.data[i].car_license) ? result.data[i].car_license : " ";
                            worksheets.Range["C" + cellNo].Value = "'" + result.data[i].document_ref;
                            worksheets.Range["D" + cellNo].Value = "'" + result.data[i].weight_out.ToString("#,##0.00");
                            worksheets.Range["E" + cellNo].Value = "'" + result.data[i].weight_cal.ToString("#,##0.00");
                            worksheets.Range["F" + cellNo].Value = "'" + result.data[i].weight_receive.ToString();
                            worksheets.Range["G" + cellNo].Value = "'" + result.data[i].weight_by_supplier.ToString();
                            worksheets.Range["H" + cellNo].Value = "'" + result.data[i].weight_diff.ToString();
                            worksheets.Range["I" + cellNo].Value = "'" + result.data[i].weight_percent.ToString();
                            worksheets.Range["J" + cellNo].Value = !String.IsNullOrEmpty(result.data[i].remark_1) ? result.data[i].remark_1 : " ";
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
                return Json(new { status = Constants.Result.Success, path = excelFile, fileName = "weight_compare.xlsx" });
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

        public async Task<IActionResult> ActionCreatePdfFile(ParamSearchWeightCompareViewModel param)
        {
            try
            {
                var result = _weightCompareService.GetSearchListWeightCompare(this._Username(), param);
                if (result.isCompleted && result.data.Count > 1)
                {
                    DateTime dateNow = DateTime.Now;
                    var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                    var pathTemplate = "";
                    pathTemplate = configuration.GetSection("Template").GetSection("TemplateWeightCompare").Value;
                    var pathTempSave = configuration.GetSection("Template").GetSection("TemplateWeightCompareSave").Value;

                    string fileTemplate = $@"{pathTemplate}";
                    string fileData = $@"{pathTempSave}weight_compare_{dateNow.ToString("yyyyMMddHHmmss")}.xlsx";
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
                        sheet.PageSetup.CenterHorizontally = true;
                        sheet.PageSetup.CenterVertically = true;

                        var item = _itemMasterService.GetItemMasterListData(this._Username()).data.Where(i => i.item_code == param.item_code).FirstOrDefault();

                        // Write Data
                        sheet[3, 2].Text = "ช่วงวันที่ : " + param.start_date.Value.ToString("dd/MM/yyyy") + " - " + param.end_date.Value.ToString("dd/MM/yyyy");
                        sheet[4, 4].Text = item.item_code + " - " + item.item_name;
                        sheet[4, 10].Text = this._Username();
                        sheet[5, 4].Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                        var start = 9;
                        foreach (var i in result.data)
                        {
                            sheet[start, 2].Text = i.date != null ? i.date.Value.ToString("dd/MM/yyyy") : " ";
                            sheet[start, 3].Text = !String.IsNullOrEmpty(i.car_license) ? i.car_license : " ";
                            sheet[start, 4].Text = i.document_ref;
                            sheet[start, 5].Text = "'" + i.weight_out.ToString("#,##0.00");
                            sheet[start, 6].Text = "'" + i.weight_cal.ToString("#,##0.00");
                            sheet[start, 7].Text = "'" + i.weight_receive.ToString("#,##0.00");
                            sheet[start, 8].Text = "'" + i.weight_by_supplier.ToString("#,##0.00");
                            sheet[start, 9].Text = "'" + i.weight_diff.ToString("#,##0.00");
                            sheet[start, 10].Text = "'" + i.weight_percent.ToString("#,##0.00");
                            sheet[start, 11].Text = !String.IsNullOrEmpty(i.remark_1) ? i.remark_1 : " ";
                            start = start + 1;
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


                        string filename = $"weight_compare_{param.item_code}.pdf";

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
