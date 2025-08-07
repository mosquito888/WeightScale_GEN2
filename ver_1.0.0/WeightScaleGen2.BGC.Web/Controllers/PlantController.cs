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
using WeightScaleGen2.BGC.Models.ViewModels.Plant;
using WeightScaleGen2.BGC.Web.Common;
using WeightScaleGen2.BGC.Web.Controllers.Base;
using WeightScaleGen2.BGC.Web.Services;

namespace WeightScaleGen2.BGC.Web.Controllers
{
    //[Authorize]
    public class PlantController : BaseController
    {
        private readonly ILogger<PlantController> _logger;
        private readonly IExcelUtilitiesCommon _excel;
        private readonly UserService _userService;
        private readonly LogService _logService;
        private readonly PlantService _plantService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public PlantController(
            ILogger<PlantController> logger,
            IExcelUtilitiesCommon excel,
            UserService userService,
            LogService logService,
            PlantService plantService,
            IWebHostEnvironment hostingEnvironment) : base(userService)
        {
            _logger = logger;
            _excel = excel;
            _userService = userService;
            _logService = logService;
            _plantService = plantService;
            _hostingEnvironment = hostingEnvironment;
        }

        private void SetPermission()
        {
            this._SetViewBagCurrentUserMenu((long)BaseConst.MENU_DEFINITION.PLANT);
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

            ParamSearchPlantViewModel model = new ParamSearchPlantViewModel();
            return View(model);
        }

        public IActionResult SearchData(ParamSearchPlantViewModel param)
        {
            SetPermission();

            if (!GetAction(Constants.Action.View))
            {
                return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
            }

            var result = _plantService.GetSearchPlantListData(this._Username(), param);
            if (result.isCompleted && result.data.Count > 0)
            {
                return Json(new ResultJqueryDataTable<ResultSearchPlantViewModel>()
                {
                    draw = param.draw,
                    data = result.data,
                    recordsTotal = result.data[0].total_record,
                    recordsFiltered = result.data[0].total_record
                });
            }
            else
            {
                return Json(new ResultJqueryDataTable<ResultSearchPlantViewModel>());
            }
        }

        public IActionResult ActionItem(string mode, string itemId)
        {
            SetPermission();
            ResultGetPlantInfoViewModel resultObj = new ResultGetPlantInfoViewModel();
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
                        string[] paramArray = Split(itemId).ToArray();
                        var plantInfo = _plantService.GetPlantListData(this._Username()).data
                            .Where(i => i.plant_code == paramArray[0])
                            .Where(i => i.comp_code == paramArray[1]).FirstOrDefault();
                        if (plantInfo != null)
                        {
                            resultObj.comp_code = plantInfo.comp_code;
                            resultObj.plant_code = plantInfo.plant_code;
                            resultObj.short_code = plantInfo.short_code;
                            resultObj.name_th = plantInfo.name_th;
                            resultObj.name_en = plantInfo.name_en;
                            resultObj.head_report = plantInfo.head_report;
                        }
                        resultObj.mode = Constants.Mode.Updated;
                        break;
                }

                return PartialView("_ModalAction", resultObj);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult PlantInfo(string mode, string itemId)
        {
            SetPermission();
            ResultGetPlantInfoViewModel resultObj = new ResultGetPlantInfoViewModel();
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
                        string[] paramArray = Split(itemId).ToArray();
                        var plantInfo = _plantService.GetPlantListData(this._Username()).data
                            .Where(i => i.plant_code == paramArray[0])
                            .Where(i => i.comp_code == paramArray[1]).FirstOrDefault();
                        if (plantInfo != null)
                        {
                            resultObj.comp_code = plantInfo.comp_code;
                            resultObj.plant_code = plantInfo.plant_code;
                            resultObj.short_code = plantInfo.short_code;
                            resultObj.name_th = plantInfo.name_th;
                            resultObj.name_en = plantInfo.name_en;
                            resultObj.head_report = plantInfo.head_report;
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

        public IActionResult DeleteItem(string mode, string itemId)
        {
            SetPermission();
            try
            {
                if (!GetAction(Constants.Action.Deleted))
                {
                    return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
                }

                string[] arrayParam = Split(itemId).ToArray();
                string checkMsg = "";
                if (CheckDel(plantCode: arrayParam[0], compCode: arrayParam[1], out checkMsg))
                {
                    return Json(new { status = Constants.Result.Invalid, message = checkMsg });
                }
                else
                {
                    var resDelete = _plantService.Delete(this._Username(), new ResultGetPlantInfoViewModel() { plant_code = arrayParam[0], comp_code = arrayParam[1] });
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

        public IActionResult SaveItem(ResultGetPlantInfoViewModel param)
        {
            SetPermission();
            try
            {
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
                        if (CheckAdd(param.plant_code, param.comp_code, out checkMsg))
                        {
                            return Json(new { status = Constants.Result.Invalid, message = checkMsg });
                        }
                        else
                        {
                            var resCreate = _plantService.Create(this._Username(), param);
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
                        var resUpdate = _plantService.Update(this._Username(), param);
                        if (resUpdate.data)
                        {
                            return Json(new { status = Constants.Result.Success });
                        }
                        else
                        {
                            return Json(new { status = Constants.Result.Invalid, message = resUpdate.message });
                        }
                }

                return Json(new { status = Constants.Result.Invalid, message = "Invalid." });
            }
            catch (Exception ex)
            {
                return Json(new { status = Constants.Result.Error, message = ex.Message });
            }
        }

        private bool CheckAdd(string plantCode, string compCode, out string checkMsg)
        {
            bool res = false;
            checkMsg = "";

            var dupPlant = _plantService.GetPlantListData(this._Username()).data
                .Where(i => i.plant_code == plantCode)
                .Where(i => i.comp_code == compCode).FirstOrDefault();

            if (dupPlant != null)
            {
                res = true;
                checkMsg = "ขออภัยมีรหัส Plant นี้อยู่แล้วในระบบ";
            }

            return res;
        }

        private bool CheckDel(string plantCode, string compCode, out string checkMsg)
        {
            bool res = true;
            checkMsg = "";
            return res;
        }

        private string[] Split(string item)
        {
            string[] arrayItem = item.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return arrayItem;
        }

        public IActionResult ActionCreateExcelFile(ParamSearchPlantViewModel param)
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
                var result = _plantService.GetSearchPlantListData(this._Username(), param);
                if (result.isCompleted && result.data.Count > 0)
                {
                    var excelFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "_Export\\PlanSaleOrders");
                    if (!Directory.Exists(excelFolder))
                    {
                        Directory.CreateDirectory(excelFolder);
                    }
                    excelFile = Path.Combine(excelFolder, "plan_sale_orders.xlsx");
                    Workbook workbook = new Workbook();
                    Worksheet worksheets = workbook.Worksheets.Add("Plan Sale Orders");
                    try
                    {
                        worksheets.Range["A1"].Value = "Company Code";
                        worksheets.Range["B1"].Value = "Plant Code";
                        worksheets.Range["C1"].Value = "Plant Short Code";
                        worksheets.Range["D1"].Value = "Plant Name (Thai)";
                        worksheets.Range["E1"].Value = "Plant Name (English)";

                        worksheets.Range["A1:E1"].Style.Font.Color = Color.White;
                        worksheets.Range["A1:E1"].Style.Color = Color.Gray;
                        worksheets.Range["A1:E1"].Style.HorizontalAlignment = HorizontalAlignType.Center;
                        worksheets.Range["A1:E1"].Style.VerticalAlignment = VerticalAlignType.Center;

                        for (int i = 0; i < result.data.Count; i++)
                        {
                            var cellNo = i + 2;
                            worksheets.Range["A" + cellNo].Value = result.data[i].comp_code;
                            worksheets.Range["B" + cellNo].Value = result.data[i].plant_code;
                            worksheets.Range["C" + cellNo].Value = result.data[i].short_code;
                            worksheets.Range["D" + cellNo].Value = result.data[i].name_th;
                            worksheets.Range["E" + cellNo].Value = result.data[i].name_en;
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
                return Json(new { status = Constants.Result.Success, path = excelFile, fileName = "plant.xlsx" });
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
