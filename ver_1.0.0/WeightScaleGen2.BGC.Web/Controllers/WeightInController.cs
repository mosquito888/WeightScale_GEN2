using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Spire.Xls;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.ViewModels.Base;
using WeightScaleGen2.BGC.Models.ViewModels.WeightIn;
using WeightScaleGen2.BGC.Models.ViewModels.WeightInHistory;
using WeightScaleGen2.BGC.Models.ViewModels.WeightOutHistory;
using WeightScaleGen2.BGC.Web.Common;
using WeightScaleGen2.BGC.Web.Controllers.Base;
using WeightScaleGen2.BGC.Web.Services;

namespace WeightScaleGen2.BGC.Web.Controllers
{
    //[Authorize]
    public class WeightInController : BaseController
    {
        private readonly ILogger<WeightInController> _logger;
        private readonly IExcelUtilitiesCommon _excel;
        private readonly UserService _userService;
        private readonly MasterService _masterService;
        private readonly WeightInService _weightInService;
        private readonly SupplierService _supplierService;
        private readonly SenderService _senderService;
        private readonly WeightInHistoryService _weightInHistoryService;
        private readonly WeightOutHistoryService _weightOutHistoryService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public WeightInController(ILogger<WeightInController> logger, IExcelUtilitiesCommon excel, UserService userService, MasterService masterService, WeightInService weightInService, SupplierService supplierService, SenderService senderService, WeightInHistoryService weightInHistoryService, WeightOutHistoryService weightOutHistoryService, IWebHostEnvironment hostingEnvironment) : base(userService)
        {
            _logger = logger;
            _excel = excel;
            _userService = userService;
            _masterService = masterService;
            _weightInService = weightInService;
            _supplierService = supplierService;
            _senderService = senderService;
            _weightInHistoryService = weightInHistoryService;
            _weightOutHistoryService = weightOutHistoryService;
            _hostingEnvironment = hostingEnvironment;
        }

        private void SetPermission()
        {
            _SetViewBagCurrentUserMenu((long)Models.ViewModels.Base.BaseConst.MENU_DEFINITION.WEIGHT_IN);
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

        [AllowAnonymous]
        public IActionResult SearchData(ParamSearchWeightInViewModel param)
        {
            SetPermission();

            if (!GetAction(Constants.Action.View))
            {
                return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
            }

            var result = _weightInService.GetSearchWeightInListData(this._Username(), param);
            if (result.isCompleted && result.data.Count > 0)
            {
                return Json(new ResultJqueryDataTable<ResultSearchWeightInViewModel>()
                {
                    draw = param.draw,
                    data = result.data,
                    recordsTotal = result.data[0].total_record,
                    recordsFiltered = result.data[0].total_record
                });
            }
            else
            {
                return Json(new ResultJqueryDataTable<ResultSearchWeightInViewModel>()
                {
                    draw = param.draw,
                    data = result.data,
                    recordsTotal = 0,
                    recordsFiltered = 0
                });
            }
        }

        public IActionResult WeightInProcess()
        {
            if (!IsToken())
            {
                return RedirectToAction("Index", "Login");
            }
            SetPermission();
            ResultGetWeightInInfoViewModel resultObj = new ResultGetWeightInInfoViewModel();
            resultObj.mode = Constants.Mode.Created;
            return View(resultObj);
        }

        [AllowAnonymous]
        public IActionResult WeightInInfo(string mode, int id)
        {
            SetPermission();
            ResultGetWeightInInfoViewModel resultObj = new ResultGetWeightInInfoViewModel();
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
                        ParamWeightInInfo param = new ParamWeightInInfo();
                        param.id = id;
                        var result = _weightInService.GetWeightInInfo(this._Username(), param);
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

        public IActionResult SaveWeightInInfo(ResultGetWeightInInfoViewModel model)
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
                        var resCreate = _weightInService.CreateWeightInInfo(this._Username(), model);
                        if (resCreate.isCompleted && resCreate.data != 0)
                        {
                            return Json(new { status = Constants.Result.Success, id = resCreate.data });
                        }
                        else
                        {
                            return Json(new { status = Constants.Result.Invalid, message = resCreate.message });
                        }
                    case Constants.Mode.Updated:
                        var resUpdate = _weightInService.UpdateWeightInInfo(this._Username(), model);
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

        public IActionResult SaveWeightInStatus(ResultGetWeightInInfoViewModel model)
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
                    case Constants.Mode.Updated:
                        model.status = "Deactive";
                        var resUpdate = _weightInService.UpdateWeightInStatus(this._Username(), model);
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

        [AllowAnonymous]
        public IActionResult WeightInInfoByCarLicense(ParamWeightInInfo param)
        {
            SetPermission();
            ResultGetWeightInInfoViewModel resultObj = new ResultGetWeightInInfoViewModel();
            try
            {
                var result = _weightInService.GetWeightInInfoByCarLicense(this._Username(), param);
                if (result.isCompleted && result.data.car_license != null)
                {
                    return Json(new { status = Constants.Result.Success, data = result.data });
                }
                else
                {
                    return Json(new { status = Constants.Result.Invalid, message = result.message[0], car_license = param.car_license });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = Constants.Result.Invalid, message = ex.Message });
            }
        }

        public IActionResult WeightHistory(string car_license)
        {
            SetPermission();

            try
            {
                var resHisIn = _weightInHistoryService.GetSearchWeightInHistoryListData(this._Username(), new ParamSearchWeightInHistoryViewModel { car_license = car_license });
                var resHisOut = _weightOutHistoryService.GetSearchWeightOutHistoryListData(this._Username(), new ParamSearchWeightOutHistoryViewModel { car_license = car_license });
                return Json(new { status = Constants.Result.Success, in_his_data = resHisIn.data, out_his_data = resHisOut.data });
            }
            catch (Exception ex)
            {
                return Json(new { status = Constants.Result.Invalid, message = ex.Message });
            }
        }

        public IActionResult ActionCreateExcelFile(ParamSearchWeightInViewModel param)
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
                var result = _weightInService.GetSearchWeightInListData(this._Username(), param);
                if (result.isCompleted && result.data.Count > 0)
                {
                    var excelFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "_Export\\WeightIn");
                    if (!Directory.Exists(excelFolder))
                    {
                        Directory.CreateDirectory(excelFolder);
                    }
                    excelFile = Path.Combine(excelFolder, "weight_in.xlsx");
                    Workbook workbook = new Workbook();
                    Worksheet worksheets = workbook.Worksheets.Add("WeightIn");
                    try
                    {
                        worksheets.Range["A1"].Value = "เลขที่ใบชั่งเข้า";
                        worksheets.Range["B1"].Value = "รหัสสินค้า";
                        worksheets.Range["C1"].Value = "ชื่อสินค้า";
                        worksheets.Range["D1"].Value = "ประเภทรถ";
                        worksheets.Range["E1"].Value = "ทะเบียนรถ";
                        worksheets.Range["F1"].Value = "รหัสผู้ส่ง";
                        worksheets.Range["G1"].Value = "ชื่อผู้ส่ง";
                        worksheets.Range["H1"].Value = "น้ำหนัก";
                        worksheets.Range["I1"].Value = "วันที่เริ่มต้น";
                        worksheets.Range["J1"].Value = "วันที่สิ้นสุด";

                        worksheets.Range["A1:J1"].Style.Font.Color = Color.White;
                        worksheets.Range["A1:J1"].Style.Color = Color.Gray;
                        worksheets.Range["A1:J1"].Style.HorizontalAlignment = HorizontalAlignType.Center;
                        worksheets.Range["A1:J1"].Style.VerticalAlignment = VerticalAlignType.Center;

                        for (int i = 0; i < result.data.Count; i++)
                        {
                            var cellNo = i + 2;
                            worksheets.Range["A" + cellNo].Value = result.data[i].weight_in_no;
                            worksheets.Range["B" + cellNo].Value = result.data[i].item_code;
                            worksheets.Range["C" + cellNo].Value = result.data[i].item_name;
                            worksheets.Range["D" + cellNo].Value = result.data[i].car_type;
                            worksheets.Range["E" + cellNo].Value = result.data[i].supplier_code.ToString();
                            worksheets.Range["F" + cellNo].Value = _supplierService.GetSupplierListData(this._Username()).data.Where(s => s.supplier_code == result.data[i].supplier_code).FirstOrDefault().supplier_name;
                            worksheets.Range["G" + cellNo].Value = "'" + result.data[i].weight_in.ToString("#,##0.00");
                            worksheets.Range["I" + cellNo].Value = result.data[i].doc_start.Value.ToString("dd/MM/yyyy HH:mm:ss");
                            worksheets.Range["J" + cellNo].Value = result.data[i].doc_stop.Value.ToString("dd/MM/yyyy HH:mm:ss");
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
                return Json(new { status = Constants.Result.Success, path = excelFile, fileName = "weight_in.xlsx" });
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

        public async Task<IActionResult> ActionPrintPdfWeightInInfo(int id)
        {
            try
            {
                ParamWeightInInfo param = new ParamWeightInInfo();
                param.id = id;
                var result = _weightInService.GetWeightInInfo(this._Username(), param);
                if (result.isCompleted)
                {
                    DateTime dateNow = DateTime.Now;
                    var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                    var pathTemplate = "";
                    pathTemplate = configuration.GetSection("Template").GetSection("TemplateWeightIn").Value;
                    var pathTempSave = configuration.GetSection("Template").GetSection("TemplateWeightInSave").Value;

                    string fileTemplate = $@"{pathTemplate}";
                    string fileData = $@"{pathTempSave}weight_in_{dateNow.ToString("yyyyMMddHHmmss")}.xlsx";
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

                        // Optional: Adjust scaling to fit all content on one page
                        sheet.PageSetup.FitToPagesWide = 1;
                        sheet.PageSetup.FitToPagesTall = 1;

                        // Write Data
                        sheet[6, 12].Text = result.data.weight_in_no;
                        sheet[9, 3].Text = result.data.weight_in_no;
                        //sheet[11, 8].Text = result.data.sender_id != 0 ? _senderService.GetSenderListData(this._Username()).data.Where(s => s.id == result.data.sender_id).FirstOrDefault().sender_name : "-";
                        sheet[9, 13].Text = result.data.date.Value.ToString("dd/MM/yyyy");

                        sheet[12, 4].Text = result.data.car_license + " (" + result.data.car_type + ")";
                        sheet[12, 11].Text = result.data.date.Value.ToString("dd/MM/yyyy HH:mm:ss");

                        sheet[13, 4].Text = result.data.item_name + " (" + result.data.item_code + ")";
                        sheet[13, 11].Text = _supplierService.GetSupplierListData(this._Username()).data.Where(s => s.supplier_code == result.data.supplier_code).FirstOrDefault().supplier_name;

                        sheet[16, 11].Text = "'" + result.data.weight_in.ToString("#,##0.00");

                        var resHis = _weightOutHistoryService.GetSearchWeightOutHistoryListData(this._Username(), new ParamSearchWeightOutHistoryViewModel { car_license = result.data.car_license });
                        if (resHis.isCompleted && resHis.data.Count > 0)
                        {
                            int position = 20;
                            resHis.data = resHis.data.Take(3).ToList();
                            foreach (var his in resHis.data)
                            {
                                sheet[position, 12].Text = "'" + his.before_weight_out.ToString("#,##0.00");
                                position = position + 2;
                            }
                        }

                        sheet[28, 3].Text = result.data.sender_id != 0 ? _senderService.GetSenderListData(this._Username()).data.Where(s => s.id == result.data.sender_id).FirstOrDefault().sender_name : "-";

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


                        string filename = $"weight_in_{result.data.weight_in_no}.pdf";

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
