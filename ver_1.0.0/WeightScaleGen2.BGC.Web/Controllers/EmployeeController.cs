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
using WeightScaleGen2.BGC.Models.ViewModels.Employee;
using WeightScaleGen2.BGC.Web.Common;
using WeightScaleGen2.BGC.Web.Controllers.Base;
using WeightScaleGen2.BGC.Web.Services;

namespace WeightScaleGen2.BGC.Web.Controllers
{
    //[Authorize]
    public class EmployeeController : BaseController
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IExcelUtilitiesCommon _excel;
        private readonly UserService _userService;
        private readonly LogService _logService;
        private readonly EmployeeService _employeeService;
        private readonly PlantService _plantService;
        private readonly CompanyService _companyService;
        private readonly DepartmentService _departmentService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public EmployeeController(ILogger<EmployeeController> logger,
            IExcelUtilitiesCommon excel,
            UserService userService,
            LogService logService,
            EmployeeService employeeService,
            PlantService plantService,
            CompanyService companyService,
            DepartmentService departmentService,
            IWebHostEnvironment hostingEnvironment) : base(userService)
        {
            _logger = logger;
            _excel = excel;
            _userService = userService;
            _logService = logService;
            _employeeService = employeeService;
            _plantService = plantService;
            _companyService = companyService;
            _departmentService = departmentService;
            _hostingEnvironment = hostingEnvironment;
        }

        private void SetPermission()
        {
            this._SetViewBagCurrentUserMenu((long)BaseConst.MENU_DEFINITION.EMPLOYEE);
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

            ParamSearchEmpViewModel model = new ParamSearchEmpViewModel();
            return View(model);
        }

        public IActionResult SearchData(ParamSearchEmpViewModel param)
        {
            SetPermission();

            if (!GetAction(Constants.Action.View))
            {
                return Json(new { status = Constants.Result.Invalid, message = Constants.Message.NotFoundPermission });
            }

            var result = _employeeService.GetSearchEmployeeListData(this._Username(), param);
            if (result.isCompleted && result.data.Count > 0)
            {
                foreach (var item in result.data)
                {
                    item.plant_code = this.getPlantNameByPlantCode(item.plant_code);
                    item.comp_code = this.getCompNameByCompCode(item.comp_code);
                    item.dept_code = this.getDeptNameByDeptCode(item.dept_code);
                }

                return Json(new ResultJqueryDataTable<ResultSearchEmpViewModel>()
                {
                    draw = param.draw,
                    data = result.data,
                    recordsTotal = result.data[0].total_record,
                    recordsFiltered = result.data[0].total_record
                });
            }
            else
            {
                return Json(new ResultJqueryDataTable<ResultSearchEmpViewModel>());
            }
        }

        public IActionResult EmployeeInfo(string empCode, string mode)
        {
            SetPermission();
            ResultGetEmpInfoViewModel resultObj = new ResultGetEmpInfoViewModel();
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
                        var resData = _employeeService.GetEmployeeInfo(this._Username(), new ParamEmpInfo() { emp_code = empCode });
                        if (resData.isCompleted || resData.data != null)
                        {
                            resultObj = resData.data;
                        }
                        resultObj.mode = Constants.Mode.Updated;
                        break;
                    default:
                        return Json(new { status = Constants.Result.Invalid, message = Constants.Message.Invalid });
                }
                return View(resultObj);
            }
            catch (Exception ex)
            {
                return Json(new { status = Constants.Result.Error, message = ex.Message });
            }
        }

        public IActionResult SaveEmployeeInfo(ResultGetEmpInfoViewModel model)
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
                        if (CheckDupEmpcode(model.emp_code))
                        {
                            return Json(new { status = Constants.Result.Invalid, message = "ขออภัยมีรหัสพนักงานนี้อยู่แล้วในระบบ" });
                        }
                        else
                        {
                            InitModelSave(ref model);
                            var resCreate = _employeeService.CreateEmployeeInfo(this._Username(), model);
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
                        InitModelSave(ref model);
                        var resUpdate = _employeeService.UpdateEmployeeInfo(this._Username(), model);
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

        private bool CheckDupEmpcode(string empCode)
        {
            var empInfo = _employeeService.GetEmployeeListData(this._Username()).data
                .Where(i => i.emp_code == empCode).FirstOrDefault();
            if (empInfo != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void InitModelSave(ref ResultGetEmpInfoViewModel model)
        {
            string plantCode = model.plant_code;
            var plantInfo = _plantService.GetPlantListData(this._Username()).data
                .Where(i => i.plant_code == plantCode).FirstOrDefault();
            if (plantInfo != null)
            {
                model.comp_code = plantInfo.comp_code;
            }
        }

        private string getCompNameByCompCode(string compCode)
        {
            string compName = "";
            var compInfo = _companyService.GetCompanyListData(this._Username()).data
                .Where(i => i.comp_code == compCode).FirstOrDefault();
            if (compInfo != null)
            {
                compName = compInfo.name_th_line1;
            }

            return compName;
        }

        private string getPlantNameByPlantCode(string plantCode)
        {
            string plantName = "";
            var plantInfo = _plantService.GetPlantListData(this._Username()).data
                .Where(i => i.plant_code == plantCode).FirstOrDefault();
            if (plantInfo != null)
            {
                plantName = plantInfo.name_th;
            }

            return plantName;
        }

        private string getDeptNameByDeptCode(string deptCode)
        {
            string deptName = "";
            var deptInfo = _departmentService.GetDepartmentListData(this._Username()).data
                .Where(i => i.dept_code == deptCode).FirstOrDefault();
            if (deptInfo != null)
            {
                deptName = deptInfo.name_th;
            }

            return deptName;
        }

        public IActionResult ActionCreateExcelFile(ParamSearchEmpViewModel param)
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
                var result = _employeeService.GetSearchEmployeeListData(this._Username(), param);
                if (result.isCompleted && result.data.Count > 0)
                {
                    foreach (var item in result.data)
                    {
                        item.plant_code = this.getPlantNameByPlantCode(item.plant_code);
                        item.comp_code = this.getCompNameByCompCode(item.comp_code);
                        item.dept_code = this.getDeptNameByDeptCode(item.dept_code);
                    }

                    var excelFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "_Export\\Employee");
                    if (!Directory.Exists(excelFolder))
                    {
                        Directory.CreateDirectory(excelFolder);
                    }
                    excelFile = Path.Combine(excelFolder, "employee.xlsx");
                    Workbook workbook = new Workbook();
                    Worksheet worksheets = workbook.Worksheets.Add("Employee");
                    try
                    {
                        worksheets.Range["A1"].Value = "Employee Code";
                        worksheets.Range["B1"].Value = "Full Name (Thai)";
                        worksheets.Range["C1"].Value = "Department";
                        worksheets.Range["D1"].Value = "Company";

                        worksheets.Range["A1:D1"].Style.Font.Color = Color.White;
                        worksheets.Range["A1:D1"].Style.Color = Color.Gray;
                        worksheets.Range["A1:D1"].Style.HorizontalAlignment = HorizontalAlignType.Center;
                        worksheets.Range["A1:D1"].Style.VerticalAlignment = VerticalAlignType.Center;

                        for (int i = 0; i < result.data.Count; i++)
                        {
                            var cellNo = i + 2;
                            worksheets.Range["A" + cellNo].Value = result.data[i].emp_code;
                            worksheets.Range["B" + cellNo].Value = result.data[i].name;
                            worksheets.Range["C" + cellNo].Value = result.data[i].dept_code;
                            worksheets.Range["D" + cellNo].Value = result.data[i].plant_code;
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
                return Json(new { status = Constants.Result.Success, path = excelFile, fileName = "employee.xlsx" });
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
