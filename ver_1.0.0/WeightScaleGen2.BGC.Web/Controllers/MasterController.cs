using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.ViewModels.Base;
using WeightScaleGen2.BGC.Models.ViewModels.Master;
using WeightScaleGen2.BGC.Web.Common;
using WeightScaleGen2.BGC.Web.Controllers.Base;
using WeightScaleGen2.BGC.Web.Services;

namespace WeightScaleGen2.BGC.Web.Controllers
{
    //[Authorize]
    public class MasterController : BaseController
    {
        private readonly ILogger<MasterController> _logger;
        private readonly IExcelUtilitiesCommon _excel;
        private readonly UserService _userService;
        private readonly LogService _logService;
        private readonly MasterService _masterService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public MasterController(
            ILogger<MasterController> logger,
            IExcelUtilitiesCommon excel,
            UserService userService,
            LogService logService,
            MasterService masterService,
            IWebHostEnvironment hostingEnvironment) : base(userService)
        {
            _logger = logger;
            _excel = excel;
            _userService = userService;
            _logService = logService;
            _masterService = masterService;
            _hostingEnvironment = hostingEnvironment;
        }

        private void SetPermission()
        {
            this._SetViewBagCurrentUserMenu((long)BaseConst.MENU_DEFINITION.NOTSET);
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

            ParamSearchMasterViewModel model = new ParamSearchMasterViewModel();
            return View(model);
        }

        public IActionResult SearchData(ParamSearchMasterViewModel param)
        {
            SetPermission();

            if (!GetAction(Constants.Action.View))
            {
                return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
            }

            var result = _masterService.GetSearchMastertListData(this._Username(), param);
            if (result.isCompleted && result.data.Count > 0)
            {
                var msType = _getMasterTypeInfo();
                foreach (var item in result.data)
                {
                    item.master_type_desc = msType.Where(i => i.master_type == item.master_type).FirstOrDefault().master_type_desc;
                    item.is_add = msType.Where(i => i.master_type == item.master_type).FirstOrDefault().is_add == true ? "Y" : "N";
                    item.is_not_edit = msType.Where(i => i.master_type == item.master_type).FirstOrDefault().is_not_edit == true ? "Y" : "N";
                    item.is_not_del = msType.Where(i => i.master_type == item.master_type).FirstOrDefault().is_not_del == true ? "Y" : "N";
                }

                return Json(new ResultJqueryDataTable<ResultSearchMasterViewModel>()
                {
                    draw = param.draw,
                    data = result.data,
                    recordsTotal = result.data[0].total_record,
                    recordsFiltered = result.data[0].total_record
                });
            }
            else
            {
                return Json(new ResultJqueryDataTable<ResultSearchMasterViewModel>());
            }
        }

        public IActionResult ActionItem(string mode, string itemId)
        {
            SetPermission();
            ResultGetMasterInfoViewModel resultObj = new ResultGetMasterInfoViewModel();
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
                        string[] arrayParam = Split(itemId).ToArray();
                        var masterInfo = _masterService.GetMasterListData(this._Username()).data
                            .Where(i => i.master_type == arrayParam[0])
                            .Where(i => i.master_code == arrayParam[1]).FirstOrDefault();
                        if (masterInfo != null)
                        {
                            resultObj.master_type = masterInfo.master_type;
                            resultObj.master_code = masterInfo.master_code;
                            resultObj.master_value1 = masterInfo.master_value1;
                            resultObj.master_desc_th = masterInfo.master_desc_th;
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

        public IActionResult MasterInfo(string mode, string itemId)
        {
            SetPermission();
            ResultGetMasterInfoViewModel resultObj = new ResultGetMasterInfoViewModel();
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
                        string[] arrayParam = Split(itemId).ToArray();
                        var masterInfo = _masterService.GetMasterListData(this._Username()).data
                            .Where(i => i.master_type == arrayParam[0])
                            .Where(i => i.master_code == arrayParam[1]).FirstOrDefault();
                        if (masterInfo != null)
                        {
                            resultObj.master_type = masterInfo.master_type;
                            resultObj.master_code = masterInfo.master_code;
                            resultObj.master_value1 = masterInfo.master_value1;
                            resultObj.master_desc_th = masterInfo.master_desc_th;
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
                var resDelete = _masterService.Delete(this._Username(), new ResultGetMasterInfoViewModel() { master_type = arrayParam[0], master_code = arrayParam[1] });
                if (resDelete.data)
                {
                    return Json(new { status = Constants.Result.Success });
                }
                else
                {
                    return Json(new { status = Constants.Result.Invalid, message = resDelete.message });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = Constants.Result.Error, message = ex.Message });
            }
        }

        public IActionResult SaveItem(ResultGetMasterInfoViewModel param)
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
                        if (CheckDup(param.master_type, param.master_code))
                        {
                            return Json(new { status = Constants.Result.Invalid, message = "ขออภัยมี Master นี้อยู่แล้วในระบบ" });
                        }
                        else
                        {
                            var resCreate = _masterService.Create(this._Username(), param);
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
                        var resUpdate = _masterService.Update(this._Username(), param);
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

        private bool CheckDup(string itemType, string itemId)
        {
            var masterInfo = _masterService.GetMasterListData(this._Username()).data
                .Where(i => i.master_type == itemType)
                .Where(i => i.master_code == itemId).FirstOrDefault();
            if (masterInfo != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string[] Split(string item)
        {
            string[] arrayItem = item.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return arrayItem;
        }

        private string _getMasterTypeDesc(string masterType)
        {
            string masterTypeDesc = "";

            var masterTypeInfo = _masterService.GetMasterListDataType(this._Username()).data
                .Where(i => i.master_type == masterType).FirstOrDefault();

            if (masterTypeInfo != null)
            {
                masterTypeDesc = masterTypeInfo.master_type_desc;
            }

            return masterTypeDesc;
        }

        private List<ResultGetMasterTypeViewModel> _getMasterTypeInfo()
        {
            List<ResultGetMasterTypeViewModel> res = new List<ResultGetMasterTypeViewModel>();

            var masterTypeInfo = _masterService.GetMasterListDataType(this._Username()).data.ToList();
            if (masterTypeInfo.Count > 0)
            {
                res = masterTypeInfo;
            }

            return res;
        }

        public IActionResult ActionCreateExcelFile(ParamSearchMasterViewModel param)
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
                var result = _masterService.GetSearchMastertListData(this._Username(), param);
                if (result.isCompleted && result.data.Count > 0)
                {
                    var msType = _getMasterTypeInfo();
                    foreach (var item in result.data)
                    {
                        item.master_type_desc = msType.Where(i => i.master_type == item.master_type).FirstOrDefault().master_type_desc;
                        item.is_add = msType.Where(i => i.master_type == item.master_type).FirstOrDefault().is_add == true ? "Y" : "N";
                        item.is_not_edit = msType.Where(i => i.master_type == item.master_type).FirstOrDefault().is_not_edit == true ? "Y" : "N";
                        item.is_not_del = msType.Where(i => i.master_type == item.master_type).FirstOrDefault().is_not_del == true ? "Y" : "N";
                    }

                    var excelFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "_Export\\Master");
                    if (!Directory.Exists(excelFolder))
                    {
                        Directory.CreateDirectory(excelFolder);
                    }
                    excelFile = Path.Combine(excelFolder, "master.xlsx");
                    Workbook workbook = new Workbook();
                    Worksheet worksheets = workbook.Worksheets.Add("Master");
                    try
                    {
                        worksheets.Range["A1"].Value = "Master Type";
                        worksheets.Range["B1"].Value = "Master Code";
                        worksheets.Range["C1"].Value = "Master Value";

                        worksheets.Range["A1:C1"].Style.Font.Color = Color.White;
                        worksheets.Range["A1:C1"].Style.Color = Color.Gray;
                        worksheets.Range["A1:C1"].Style.HorizontalAlignment = HorizontalAlignType.Center;
                        worksheets.Range["A1:C1"].Style.VerticalAlignment = VerticalAlignType.Center;

                        for (int i = 0; i < result.data.Count; i++)
                        {
                            var cellNo = i + 2;
                            worksheets.Range["A" + cellNo].Value = result.data[i].master_type_desc;
                            worksheets.Range["B" + cellNo].Value = result.data[i].master_code;
                            worksheets.Range["C" + cellNo].Value = result.data[i].master_value1;
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
                return Json(new { status = Constants.Result.Success, path = excelFile, fileName = "master.xlsx" });
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
