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
using WeightScaleGen2.BGC.Models.ViewModels.Log;
using WeightScaleGen2.BGC.Web.Common;
using WeightScaleGen2.BGC.Web.Controllers.Base;
using WeightScaleGen2.BGC.Web.Services;

namespace WeightScaleGen2.BGC.Web.Controllers
{
    //[Authorize]
    public class LogController : BaseController
    {

        private readonly ILogger<LogController> _logger;
        private readonly IExcelUtilitiesCommon _excel;
        private readonly UserService _userService;
        private readonly LogService _logService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public LogController(ILogger<LogController> logger, IExcelUtilitiesCommon excel, UserService userService, LogService logService, IWebHostEnvironment hostingEnvironment) : base(userService)
        {
            _logger = logger;
            _excel = excel;
            _userService = userService;
            _logService = logService;
            _hostingEnvironment = hostingEnvironment;
        }

        private void SetPermission()
        {
            _SetViewBagCurrentUserMenu((long)BaseConst.MENU_DEFINITION.LOG);
            ViewBag.view = _GetPermission(_GetControllerName(), Constants.Action.View);
            ViewBag.created = _GetPermission(_GetControllerName(), Constants.Action.Created);
            ViewBag.edit = _GetPermission(_GetControllerName(), Constants.Action.Edit);
            ViewBag.deleted = _GetPermission(_GetControllerName(), Constants.Action.Deleted);
            ViewBag.print = _GetPermission(_GetControllerName(), Constants.Action.Print);
            ViewBag.MenuName = _GetMenuName(_GetControllerName());
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

            var result = _logService.GetSearchLogCriteria(_Username());
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

        public IActionResult LogReportData(ParamSearchLogViewModel param)
        {
            SetPermission();
            var result = _logService.SearchLogData(_Username(), param);
            if (result.isCompleted && result.data.Count > 0)
            {
                return Json(new ResultJqueryDataTable<ResultSearchLogViewModel>()
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
                return Json(new ResultJqueryDataTable<ResultSearchLogViewModel>());
            }
        }

        public IActionResult ActionCreateExcelFile(ParamSearchLogViewModel param)
        {
            SetPermission();
            try
            {
                param.start = 0;
                param.draw = int.MaxValue;
                param.length = int.MaxValue;
                string excelFile = "";
                var result = _logService.SearchLogData(_Username(), param);
                if (result.isCompleted && result.data.Count > 0)
                {
                    var excelFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "_Export\\Log");
                    if (!Directory.Exists(excelFolder))
                    {
                        Directory.CreateDirectory(excelFolder);
                    }
                    excelFile = Path.Combine(excelFolder, "log.xlsx");
                    Workbook workbook = new Workbook();
                    Worksheet worksheets = workbook.Worksheets.Add("Log");
                    try
                    {
                        worksheets.Range["A1"].Value = "Level";
                        worksheets.Range["B1"].Value = "LogDate";
                        worksheets.Range["C1"].Value = "UserName";
                        worksheets.Range["D1"].Value = "Message";
                        worksheets.Range["E1"].Value = "ExceptionMessage";
                        worksheets.Range["F1"].Value = "LogCallerFilePath";
                        worksheets.Range["G1"].Value = "LogSoureceLineNumber";

                        worksheets.Range["A1:G1"].Style.Font.Color = Color.White;
                        worksheets.Range["A1:G1"].Style.Color = Color.Gray;
                        worksheets.Range["A1:G1"].Style.HorizontalAlignment = HorizontalAlignType.Center;
                        worksheets.Range["A1:G1"].Style.VerticalAlignment = VerticalAlignType.Center;

                        for (int i = 0; i < result.data.Count; i++)
                        {
                            var cellNo = i + 2;
                            worksheets.Range["A" + cellNo].Value = result.data[i].level;
                            worksheets.Range["B" + cellNo].Value = result.data[i].log_date.ToString("dd/MM/yyyy HH:mm:ss");
                            worksheets.Range["C" + cellNo].Value = result.data[i].username;
                            worksheets.Range["D" + cellNo].Value = result.data[i].message;
                            worksheets.Range["E" + cellNo].Value = result.data[i].exception_message;
                            worksheets.Range["F" + cellNo].Value = result.data[i].log_caller_file_path;
                            worksheets.Range["G" + cellNo].Value = result.data[i].log_source_line_number;
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
                return Json(new { status = Constants.Result.Success, path = excelFile, fileName = "log.xlsx" });
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
