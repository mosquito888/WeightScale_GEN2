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
using WeightScaleGen2.BGC.Models.ViewModels.DocumentPO;
using WeightScaleGen2.BGC.Models.ViewModels.MMPO;
using WeightScaleGen2.BGC.Web.Common;
using WeightScaleGen2.BGC.Web.Controllers.Base;
using WeightScaleGen2.BGC.Web.Services;

namespace WeightScaleGen2.BGC.Web.Controllers
{
    //[Authorize]
    public class DocumentPOController : BaseController
    {
        private readonly ILogger<DocumentPOController> _logger;
        private readonly IExcelUtilitiesCommon _excel;
        private readonly UserService _userService;
        private readonly MasterService _masterService;
        private readonly DocumentPOService _documentPOService;
        private readonly MMPOService _mmPOService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public DocumentPOController(ILogger<DocumentPOController> logger, IExcelUtilitiesCommon excel, UserService userService, MasterService masterService, DocumentPOService documentPOService, MMPOService mmPOService, IWebHostEnvironment hostingEnvironment) : base(userService)
        {
            _logger = logger;
            _excel = excel;
            _userService = userService;
            _masterService = masterService;
            _documentPOService = documentPOService;
            _mmPOService = mmPOService;
            _hostingEnvironment = hostingEnvironment;
        }

        private void SetPermission()
        {
            _SetViewBagCurrentUserMenu((long)Models.ViewModels.Base.BaseConst.MENU_DEFINITION.DOCUMENT_PO);
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

        public IActionResult SearchData(ParamSearchDocumentPOViewModel param)
        {
            SetPermission();

            if (!GetAction(Constants.Action.View))
            {
                return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
            }

            var result = _documentPOService.GetSearchListDocumentPO(this._Username(), param);
            if (result.isCompleted && result.data.Count > 0)
            {
                return Json(new ResultJqueryDataTable<ResultSearchDocumentPOViewModel>()
                {
                    draw = param.draw,
                    data = result.data,
                    recordsTotal = result.data[0].total_record,
                    recordsFiltered = result.data[0].total_record
                });
            }
            else
            {
                return Json(new ResultJqueryDataTable<ResultSearchDocumentPOViewModel>()
                {
                    draw = param.draw,
                    data = result.data,
                    recordsTotal = 0,
                    recordsFiltered = 0
                });
            }
        }

        public IActionResult SearchMMPOData(ParamSearchMMPOViewModel param)
        {
            SetPermission();

            if (!GetAction(Constants.Action.View))
            {
                return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
            }

            var result = _mmPOService.GetSearchListMMPO(this._Username(), param);
            if (result.isCompleted && result.data.Count > 0)
            {
                return Json(new ResultJqueryDataTable<ResultSearchMMPOViewModel>()
                {
                    draw = param.draw,
                    data = result.data,
                    recordsTotal = result.data[0].total_record,
                    recordsFiltered = result.data[0].total_record
                });
            }
            else
            {
                return Json(new ResultJqueryDataTable<ResultSearchMMPOViewModel>()
                {
                    draw = param.draw,
                    data = result.data,
                    recordsTotal = 0,
                    recordsFiltered = 0
                });
            }
        }

        public IActionResult DocumentPOInfo(string purchase_number)
        {
            SetPermission();
            ResultSearchDocumentPOViewModel resultObj = new ResultSearchDocumentPOViewModel();
            try
            {
                var result = _documentPOService.GetDocumentPOInfo(this._Username(), purchase_number);
                if (result.isCompleted && result.data != null)
                {
                    return Json(new { status = Constants.Result.Success, data = result.data });
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

        public IActionResult CheckQtyPendingDocumentPO(ParamSearchMMPOQtyPendingViewModel param)
        {
            SetPermission();
            ResultSearchDocumentPOViewModel resultObj = new ResultSearchDocumentPOViewModel();
            try
            {
                var result = _mmPOService.GetSearchMMPOCheckQtyPending(this._Username(), param);
                if (result.isCompleted)
                {
                    return Json(new { status = Constants.Result.Success, data = result.data, message = result.message });
                }
                else
                {
                    return Json(new { status = Constants.Result.Invalid, message = result.message });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = Constants.Result.Invalid, message = ex.Message });
            }
        }

        public IActionResult UpdateMMPOSapToDocumentPO(ParamSearchMMPOViewModel param)
        {
            SetPermission();

            if (!GetAction(Constants.Action.View))
            {
                return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
            }

            param.start = 0;
            param.draw = int.MaxValue;
            param.length = int.MaxValue;
            var result = _mmPOService.UpdateMMPOSapToDocumentPO(this._Username(), param);
            if (result.data)
            {
                return Json(new { status = Constants.Result.Success });
            }
            else
            {
                return Json(new { status = Constants.Result.Invalid, message = result.message });
            }
        }

        public IActionResult UpdateMMPOSapToDocumentPOData()
        {
            SetPermission();

            if (!GetAction(Constants.Action.View))
            {
                return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
            }

            var result = _mmPOService.UpdateMMPOSapToDocumentPOData(this._Username());
            if (result.data)
            {
                return Json(new { status = Constants.Result.Success });
            }
            else
            {
                return Json(new { status = Constants.Result.Invalid, message = result.message });
            }
        }

        public IActionResult ActionCreateExcelFile(ParamSearchDocumentPOViewModel param)
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
                var result = _documentPOService.GetSearchListDocumentPO(this._Username(), param);
                if (result.isCompleted && result.data.Count > 0)
                {
                    var excelFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "_Export\\DocumentPO");
                    if (!Directory.Exists(excelFolder))
                    {
                        Directory.CreateDirectory(excelFolder);
                    }
                    excelFile = Path.Combine(excelFolder, "document_po.xlsx");
                    Workbook workbook = new Workbook();
                    Worksheet worksheets = workbook.Worksheets.Add("DocumentPO");
                    try
                    {
                        worksheets.Range["A1"].Value = "Purchase Number";
                        worksheets.Range["B1"].Value = "Num of Rec";
                        worksheets.Range["C1"].Value = "Company Code";
                        worksheets.Range["D1"].Value = "Plant";
                        worksheets.Range["E1"].Value = "Storage Loc";
                        worksheets.Range["F1"].Value = "Status";
                        worksheets.Range["G1"].Value = "Vender Code";
                        worksheets.Range["H1"].Value = "Vender Name";
                        worksheets.Range["I1"].Value = "Material Code";
                        worksheets.Range["J1"].Value = "Material Desc";
                        worksheets.Range["K1"].Value = "Order QTY";
                        worksheets.Range["L1"].Value = "UOM";
                        worksheets.Range["M1"].Value = "UOM In";
                        worksheets.Range["N1"].Value = "Good Received";
                        worksheets.Range["O1"].Value = "Pending QTY";
                        worksheets.Range["P1"].Value = "Pending QTY All";
                        worksheets.Range["Q1"].Value = "Allowance";
                        worksheets.Range["R1"].Value = "DLV Complete";
                        worksheets.Range["S1"].Value = "Created By";
                        worksheets.Range["T1"].Value = "Created Date";
                        worksheets.Range["U1"].Value = "Created Time";
                        worksheets.Range["V1"].Value = "Modified By";
                        worksheets.Range["W1"].Value = "Modified Date";
                        worksheets.Range["X1"].Value = "Modified Time";

                        worksheets.Range["A1:X1"].Style.Font.Color = Color.White;
                        worksheets.Range["A1:X1"].Style.Color = Color.Gray;
                        worksheets.Range["A1:X1"].Style.HorizontalAlignment = HorizontalAlignType.Center;
                        worksheets.Range["A1:X1"].Style.VerticalAlignment = VerticalAlignType.Center;

                        for (int i = 0; i < result.data.Count; i++)
                        {
                            var cellNo = i + 2;
                            worksheets.Range["A" + cellNo].Value = "'" + result.data[i].purchase_number;
                            worksheets.Range["B" + cellNo].Value = "'" + result.data[i].num_of_rec.ToString();
                            worksheets.Range["C" + cellNo].Value = "'" + result.data[i].company_code;
                            worksheets.Range["D" + cellNo].Value = "'" + result.data[i].plant;
                            worksheets.Range["E" + cellNo].Value = "'" + result.data[i].storage_loc;
                            worksheets.Range["F" + cellNo].Value = result.data[i].status;
                            worksheets.Range["G" + cellNo].Value = result.data[i].vender_code;
                            worksheets.Range["H" + cellNo].Value = result.data[i].vender_name;
                            worksheets.Range["I" + cellNo].Value = result.data[i].material_code;
                            worksheets.Range["J" + cellNo].Value = result.data[i].material_desc;
                            worksheets.Range["K" + cellNo].Value = "'" + result.data[i].order_qty.ToString("#,##0.00");
                            worksheets.Range["L" + cellNo].Value = result.data[i].uom;
                            worksheets.Range["M" + cellNo].Value = result.data[i].uom_in;
                            worksheets.Range["N" + cellNo].Value = "'" + result.data[i].good_received.ToString("#,##0.00");
                            worksheets.Range["O" + cellNo].Value = "'" + result.data[i].pending_qty.ToString("#,##0.00");
                            worksheets.Range["P" + cellNo].Value = "'" + result.data[i].pending_qty_all.ToString("#,##0.00");
                            worksheets.Range["Q" + cellNo].Value = "'" + result.data[i].allowance.ToString("#,##0.00");
                            worksheets.Range["R" + cellNo].Value = result.data[i].dlv_complete;
                            worksheets.Range["S" + cellNo].Value = result.data[i].created_by;
                            worksheets.Range["T" + cellNo].Value = result.data[i].created_date.ToString("dd/MM/yyyy");
                            worksheets.Range["U" + cellNo].Value = result.data[i].created_time.ToString(@"hh\:mm\:ss");
                            worksheets.Range["V" + cellNo].Value = result.data[i].modified_by;
                            worksheets.Range["W" + cellNo].Value = result.data[i].modified_date.Value.ToString("dd/MM/yyyy");
                            worksheets.Range["X" + cellNo].Value = result.data[i].modified_time.ToString(@"hh\:mm\:ss");
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
                return Json(new { status = Constants.Result.Success, path = excelFile, fileName = "document_po.xlsx" });
            }
            catch (Exception ex)
            {
                return Json(new { status = Constants.Result.Invalid, message = ex.Message });
            }
        }

        public IActionResult ActionCreateExcelMMPOFile(ParamSearchMMPOViewModel param)
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
                var result = _mmPOService.GetSearchListMMPO(this._Username(), param);
                if (result.isCompleted && result.data.Count > 0)
                {
                    var excelFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "_Export\\DocumentPO");
                    if (!Directory.Exists(excelFolder))
                    {
                        Directory.CreateDirectory(excelFolder);
                    }
                    excelFile = Path.Combine(excelFolder, "document_sap_po.xlsx");
                    Workbook workbook = new Workbook();
                    Worksheet worksheets = workbook.Worksheets.Add("DocumentPO");
                    try
                    {
                        worksheets.Range["A1"].Value = "Purchase Number";
                        worksheets.Range["B1"].Value = "Num of Rec";
                        worksheets.Range["C1"].Value = "Company Code";
                        worksheets.Range["D1"].Value = "Plant";
                        worksheets.Range["E1"].Value = "Storage Loc";
                        worksheets.Range["F1"].Value = "Status";
                        worksheets.Range["G1"].Value = "Vender Code";
                        worksheets.Range["H1"].Value = "Vender Name";
                        worksheets.Range["I1"].Value = "Material Code";
                        worksheets.Range["J1"].Value = "Material Desc";
                        worksheets.Range["K1"].Value = "Order QTY";
                        worksheets.Range["L1"].Value = "UOM";
                        worksheets.Range["M1"].Value = "UOM In";
                        worksheets.Range["N1"].Value = "Good Received";
                        worksheets.Range["O1"].Value = "Pending QTY";
                        worksheets.Range["P1"].Value = "Pending QTY All";
                        worksheets.Range["Q1"].Value = "Allowance";
                        worksheets.Range["R1"].Value = "DLV Complete";
                        worksheets.Range["S1"].Value = "Created By";
                        worksheets.Range["T1"].Value = "Created Date";
                        worksheets.Range["U1"].Value = "Created Time";
                        worksheets.Range["V1"].Value = "Modified By";
                        worksheets.Range["W1"].Value = "Modified Date";
                        worksheets.Range["X1"].Value = "Modified Time";

                        worksheets.Range["A1:X1"].Style.Font.Color = Color.White;
                        worksheets.Range["A1:X1"].Style.Color = Color.Gray;
                        worksheets.Range["A1:X1"].Style.HorizontalAlignment = HorizontalAlignType.Center;
                        worksheets.Range["A1:X1"].Style.VerticalAlignment = VerticalAlignType.Center;

                        for (int i = 0; i < result.data.Count; i++)
                        {
                            var cellNo = i + 2;
                            worksheets.Range["A" + cellNo].Value = "'" + result.data[i].purchase_number;
                            worksheets.Range["B" + cellNo].Value = "'" + result.data[i].num_of_rec.ToString();
                            worksheets.Range["C" + cellNo].Value = "'" + result.data[i].company_code;
                            worksheets.Range["D" + cellNo].Value = "'" + result.data[i].plant;
                            worksheets.Range["E" + cellNo].Value = "'" + result.data[i].storage_loc;
                            worksheets.Range["F" + cellNo].Value = result.data[i].status;
                            worksheets.Range["G" + cellNo].Value = result.data[i].vender_code;
                            worksheets.Range["H" + cellNo].Value = result.data[i].vender_name;
                            worksheets.Range["I" + cellNo].Value = result.data[i].material_code;
                            worksheets.Range["J" + cellNo].Value = result.data[i].material_desc;
                            worksheets.Range["K" + cellNo].Value = "'" + result.data[i].order_qty.ToString("#,##0.00");
                            worksheets.Range["L" + cellNo].Value = result.data[i].uom;
                            worksheets.Range["M" + cellNo].Value = result.data[i].uom_in;
                            worksheets.Range["N" + cellNo].Value = "'" + result.data[i].good_received.ToString("#,##0.00");
                            worksheets.Range["O" + cellNo].Value = "'" + result.data[i].pending_qty.ToString("#,##0.00");
                            worksheets.Range["P" + cellNo].Value = "'" + result.data[i].pending_qty_all.ToString("#,##0.00");
                            worksheets.Range["Q" + cellNo].Value = "'" + result.data[i].allowance.ToString("#,##0.00");
                            worksheets.Range["R" + cellNo].Value = result.data[i].dlv_complete;
                            worksheets.Range["S" + cellNo].Value = result.data[i].created_by;
                            worksheets.Range["T" + cellNo].Value = result.data[i].created_on.ToString("dd/MM/yyyy");
                            worksheets.Range["U" + cellNo].Value = result.data[i].created_time.ToString(@"hh\:mm\:ss");
                            worksheets.Range["V" + cellNo].Value = result.data[i].updated_by;
                            worksheets.Range["W" + cellNo].Value = result.data[i].updated_on.ToString("dd/MM/yyyy");
                            worksheets.Range["X" + cellNo].Value = result.data[i].updated_time.ToString(@"hh\:mm\:ss");
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
                return Json(new { status = Constants.Result.Success, path = excelFile, fileName = "document_sap_po.xlsx" });
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