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
using WeightScaleGen2.BGC.Models.ViewModels.Company;
using WeightScaleGen2.BGC.Web.Common;
using WeightScaleGen2.BGC.Web.Controllers.Base;
using WeightScaleGen2.BGC.Web.Services;

namespace WeightScaleGen2.BGC.Web.Controllers
{
    //[Authorize]
    public class CompanyController : BaseController
    {
        private readonly ILogger<CompanyController> _logger;
        private readonly IExcelUtilitiesCommon _excel;
        private readonly UserService _userService;
        private readonly LogService _logService;
        private readonly CompanyService _companyService;
        private readonly PlantService _plantService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public CompanyController(
            ILogger<CompanyController> logger,
            IExcelUtilitiesCommon excel,
            UserService userService,
            LogService logService,
            CompanyService companyService,
            PlantService plantService,
            IWebHostEnvironment hostingEnvironment) : base(userService)
        {
            _logger = logger;
            _excel = excel;
            _userService = userService;
            _logService = logService;
            _companyService = companyService;
            _plantService = plantService;
            _hostingEnvironment = hostingEnvironment;
        }

        private void SetPermission()
        {
            this._SetViewBagCurrentUserMenu((long)BaseConst.MENU_DEFINITION.COMPANY);
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

            ParamSearchCompViewModel model = new ParamSearchCompViewModel();
            return View(model);
        }

        public IActionResult SearchData(ParamSearchCompViewModel param)
        {
            SetPermission();

            if (!GetAction(Constants.Action.View))
            {
                return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
            }

            var result = _companyService.GetSearchCompanyListData(this._Username(), param);
            if (result.isCompleted && result.data.Count > 0)
            {
                return Json(new ResultJqueryDataTable<ResultSearchCompViewModel>()
                {
                    draw = param.draw,
                    data = result.data,
                    recordsTotal = result.data[0].total_record,
                    recordsFiltered = result.data[0].total_record
                });
            }
            else
            {
                return Json(new ResultJqueryDataTable<ResultSearchCompViewModel>());
            }
        }

        public IActionResult ActionItem(string mode, string itemId)
        {
            SetPermission();
            ResultGetCompInfoViewModel resultObj = new ResultGetCompInfoViewModel();
            try
            {
                if (mode != Constants.Mode.Created && mode != Constants.Mode.Updated)
                {
                    return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
                }

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
                        var compInfo = _companyService.GetCompanyListData(this._Username()).data
                            .Where(i => i.comp_code == itemId).FirstOrDefault();
                        if (compInfo != null)
                        {
                            resultObj.comp_code = compInfo.comp_code;
                            resultObj.name_th_line1 = compInfo.name_th_line1;
                            resultObj.addr_th_line1 = compInfo.addr_th_line1;
                            resultObj.addr_th_line2 = compInfo.addr_th_line2;
                            resultObj.name_en_line1 = compInfo.name_en_line1;
                            resultObj.addr_en_line1 = compInfo.addr_en_line1;
                            resultObj.addr_en_line2 = compInfo.addr_en_line2;
                        }
                        resultObj.mode = Constants.Mode.Updated;
                        break;
                    default:
                        return Json(new { status = Constants.Result.Invalid, message = Constants.Message.Invalid });
                }

                return PartialView("_ModalAction", resultObj);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult CompanyInfo(string mode, string itemId)
        {
            SetPermission();
            ResultGetCompInfoViewModel resultObj = new ResultGetCompInfoViewModel();
            try
            {
                if (mode != Constants.Mode.Created && mode != Constants.Mode.Updated)
                {
                    return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
                }

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
                        var compInfo = _companyService.GetCompanyListData(this._Username()).data
                            .Where(i => i.comp_code == itemId).FirstOrDefault();
                        if (compInfo != null)
                        {
                            resultObj.comp_code = compInfo.comp_code;
                            resultObj.name_th_line1 = compInfo.name_th_line1;
                            resultObj.addr_th_line1 = compInfo.addr_th_line1;
                            resultObj.addr_th_line2 = compInfo.addr_th_line2;
                            resultObj.name_en_line1 = compInfo.name_en_line1;
                            resultObj.addr_en_line1 = compInfo.addr_en_line1;
                            resultObj.addr_en_line2 = compInfo.addr_en_line2;
                        }
                        resultObj.mode = Constants.Mode.Updated;
                        break;
                    default:
                        return Json(new { status = Constants.Result.Invalid, message = Constants.Message.Invalid });
                }

                return View(resultObj);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult DeleteItem(string mode, string itemId)
        {
            SetPermission();
            try
            {
                if (!GetAction(Constants.Action.Deleted))
                {
                    return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
                }

                string checkMsg = "";
                if (CheckDel(itemId, out checkMsg))
                {
                    return Json(new { status = Constants.Result.Invalid, message = checkMsg });
                }
                else
                {
                    var resDelete = _companyService.Delete(this._Username(), new ResultGetCompInfoViewModel() { comp_code = itemId });
                    if (resDelete.data)
                    {
                        return Json(new { status = Constants.Result.Success });
                    }
                    else
                    {
                        return Json(new { status = Constants.Result.Invalid, message = resDelete.message });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = Constants.Result.Error, message = ex.Message });
            }
        }

        public IActionResult SaveItem(ResultGetCompInfoViewModel param)
        {
            SetPermission();
            try
            {
                if (param.mode != Constants.Mode.Created && param.mode != Constants.Mode.Updated)
                {
                    return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
                }

                if (!GetAction(Constants.Action.Created) && param.mode == Constants.Mode.Created)
                {
                    return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
                }

                if (!GetAction(Constants.Action.Edit) && param.mode == Constants.Mode.Updated)
                {
                    return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
                }

                switch (param.mode)
                {
                    case Constants.Mode.Created:
                        string checkMsg = "";
                        if (CheckAdd(param.comp_code, out checkMsg))
                        {
                            return Json(new { status = Constants.Result.Invalid, message = checkMsg });
                        }
                        else
                        {
                            var resCreate = _companyService.Create(this._Username(), param);
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
                        var resUpdate = _companyService.Update(this._Username(), param);
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

        private bool CheckAdd(string itemId, out string checkMsg)
        {
            bool res = false;
            checkMsg = "";

            var dupComp = _companyService.GetCompanyListData(this._Username()).data
                .Where(i => i.comp_code == itemId).FirstOrDefault();

            if (dupComp != null)
            {
                res = true;
                checkMsg = "ขออภัยมีรหัสบริษัทนี้อยู่แล้วในระบบ";
            }

            return res;
        }

        private bool CheckDel(string itemId, out string checkMsg)
        {
            bool res = false;
            checkMsg = "";

            var usingPlant = _plantService.GetPlantListData(this._Username()).data
                .Where(i => i.comp_code == itemId).FirstOrDefault();

            if (usingPlant != null)
            {
                res = true;
                checkMsg = "ขออภัยไม่สามารถลบได้เนื่องจากมีการใช้งานรหัสบริษัทนี้อยู่ในระบบ";
            }

            return res;
        }

        public IActionResult ActionCreateExcelFile(ParamSearchCompViewModel param)
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
                var result = _companyService.GetSearchCompanyListData(this._Username(), param);
                if (result.isCompleted && result.data.Count > 0)
                {
                    var excelFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "_Export\\Company");
                    if (!Directory.Exists(excelFolder))
                    {
                        Directory.CreateDirectory(excelFolder);
                    }
                    excelFile = Path.Combine(excelFolder, "company.xlsx");
                    Workbook workbook = new Workbook();
                    Worksheet worksheets = workbook.Worksheets.Add("Company");
                    try
                    {
                        worksheets.Range["A1"].Value = "Company Code";
                        worksheets.Range["B1"].Value = "Name (Thai)";
                        worksheets.Range["C1"].Value = "Name (English)";

                        worksheets.Range["A1:C1"].Style.Font.Color = Color.White;
                        worksheets.Range["A1:C1"].Style.Color = Color.Gray;
                        worksheets.Range["A1:C1"].Style.HorizontalAlignment = HorizontalAlignType.Center;
                        worksheets.Range["A1:C1"].Style.VerticalAlignment = VerticalAlignType.Center;

                        for (int i = 0; i < result.data.Count; i++)
                        {
                            var cellNo = i + 2;
                            worksheets.Range["A" + cellNo].Value = result.data[i].comp_code;
                            worksheets.Range["B" + cellNo].Value = result.data[i].name_th_line1;
                            worksheets.Range["C" + cellNo].Value = result.data[i].name_en_line1;
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
                return Json(new { status = Constants.Result.Success, path = excelFile, fileName = "company.xlsx" });
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
