using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.ViewModels.Company;
using WeightScaleGen2.BGC.Models.ViewModels.Department;
using WeightScaleGen2.BGC.Models.ViewModels.Employee;
using WeightScaleGen2.BGC.Models.ViewModels.GroupMaster;
using WeightScaleGen2.BGC.Models.ViewModels.ItemMaster;
using WeightScaleGen2.BGC.Models.ViewModels.Master;
using WeightScaleGen2.BGC.Models.ViewModels.Plant;
using WeightScaleGen2.BGC.Models.ViewModels.Sender;
using WeightScaleGen2.BGC.Models.ViewModels.Supplier;
using WeightScaleGen2.BGC.Web.Services;

namespace WeightScaleGen2.BGC.Web.Common
{
    public static class SelectListMethods
    {
        public static IEnumerable<SelectListItem> GetEmpCreatedName(string empCode = null)
        {
            List<ResultGetEmpViewModel> modelEmp = new List<ResultGetEmpViewModel>();
            var datas = SelectListService.GetEmpListData();
            var obj = new List<SelectListItem>();

            if (datas.isCompleted && datas.data != null)
            {
                modelEmp = datas.data;
                if (empCode == null)
                {
                    var result = modelEmp.Select(x => new SelectListItem
                    {
                        Text = $"รหัสพนักงาน : {x.emp_code} | ชื่อพนักงาน : {x.name} | PlantCode : {x.plant_code}",
                        Value = x.emp_code
                    });
                    return result;
                }
                else
                {
                    var result = modelEmp.Select(x => new SelectListItem
                    {
                        Text = $"รหัสพนักงาน : {x.emp_code} | ชื่อพนักงาน : {x.name} | PlantCode : {x.plant_code}",
                        Value = x.emp_code,
                        Selected = (x.emp_code == empCode)
                    });
                    return result;
                }
            }

            return obj;
        }

        public static IEnumerable<SelectListItem> GetWorkShift(string code = null)
        {
            List<ResultGetMasterViewModel> modelMaster = new List<ResultGetMasterViewModel>();
            var datas = SelectListService.GetMasterListData();
            var obj = new List<SelectListItem>();

            if (datas.isCompleted && datas.data != null)
            {
                modelMaster = datas.data
                    .Where(i => i.master_type == Constants.MasterType.WorkShift).ToList();

                if (code == null)
                {
                    var result = modelMaster.Select(x => new SelectListItem
                    {
                        Text = x.master_value1,
                        Value = x.master_code
                    });
                    return result;
                }
                else
                {
                    var result = modelMaster.Select(x => new SelectListItem
                    {
                        Text = x.master_value1,
                        Value = x.master_code,
                        Selected = (x.master_code == code)
                    });
                    return result;
                }
            }

            return obj;
        }

        public static IEnumerable<SelectListItem> GetMasterType(string code = null)
        {
            List<ResultGetMasterTypeViewModel> modelMaster = new List<ResultGetMasterTypeViewModel>();
            var datas = SelectListService.GetMasterListDataType();
            var obj = new List<SelectListItem>();

            if (datas.isCompleted && datas.data != null)
            {
                modelMaster = datas.data
                    .Where(i => i.is_add == true).ToList();

                if (code == null)
                {
                    var result = modelMaster.Select(x => new SelectListItem
                    {
                        Text = x.master_type_desc,
                        Value = x.master_type
                    });
                    return result;
                }
                else
                {
                    var result = modelMaster.Select(x => new SelectListItem
                    {
                        Text = x.master_type_desc,
                        Value = x.master_type,
                        Selected = (x.master_type == code)
                    });
                    return result;
                }
            }

            return obj;
        }

        public static IEnumerable<SelectListItem> GetAssetType(string code = null)
        {
            List<ResultGetMasterViewModel> modelMaster = new List<ResultGetMasterViewModel>();
            var datas = SelectListService.GetMasterListData();
            var obj = new List<SelectListItem>();

            if (datas.isCompleted && datas.data != null)
            {
                modelMaster = datas.data
                    .Where(i => i.master_type == Constants.MasterType.AssetType).ToList();

                if (code == null)
                {
                    var result = modelMaster.Select(x => new SelectListItem
                    {
                        Text = x.master_value1,
                        Value = x.master_code
                    });
                    return result;
                }
                else
                {
                    var result = modelMaster.Select(x => new SelectListItem
                    {
                        Text = x.master_value1,
                        Value = x.master_code,
                        Selected = (x.master_code == code)
                    });
                    return result;
                }
            }

            return obj;
        }

        public static IEnumerable<SelectListItem> GetUnitType(string code = null)
        {
            List<ResultGetMasterViewModel> modelMaster = new List<ResultGetMasterViewModel>();
            var datas = SelectListService.GetMasterListData();
            var obj = new List<SelectListItem>();

            if (datas.isCompleted && datas.data != null)
            {
                modelMaster = datas.data
                    .Where(i => i.master_type == Constants.MasterType.UnitType).ToList();

                if (code == null)
                {
                    var result = modelMaster.Select(x => new SelectListItem
                    {
                        Text = x.master_value1,
                        Value = x.master_code
                    });
                    return result;
                }
                else
                {
                    var result = modelMaster.Select(x => new SelectListItem
                    {
                        Text = x.master_value1,
                        Value = x.master_code,
                        Selected = (x.master_code == code)
                    });
                    return result;
                }
            }

            return obj;
        }

        public static IEnumerable<SelectListItem> GetReceivedType(string code = null)
        {
            List<ResultGetMasterViewModel> modelMaster = new List<ResultGetMasterViewModel>();
            var datas = SelectListService.GetMasterListData();
            var obj = new List<SelectListItem>();

            if (datas.isCompleted && datas.data != null)
            {
                modelMaster = datas.data
                    .Where(i => i.master_type == Constants.MasterType.ReceivedType).ToList();

                if (code == null)
                {
                    var result = modelMaster.Select(x => new SelectListItem
                    {
                        Text = x.master_value1,
                        Value = x.master_code
                    });
                    return result;
                }
                else
                {
                    var result = modelMaster.Select(x => new SelectListItem
                    {
                        Text = x.master_value1,
                        Value = x.master_code,
                        Selected = (x.master_code == code)
                    });
                    return result;
                }
            }

            return obj;
        }

        public static IEnumerable<SelectListItem> GetPayType(string code = null)
        {
            List<ResultGetMasterViewModel> modelMaster = new List<ResultGetMasterViewModel>();
            var datas = SelectListService.GetMasterListData();
            var obj = new List<SelectListItem>();

            if (datas.isCompleted && datas.data != null)
            {
                modelMaster = datas.data
                    .Where(i => i.master_type == Constants.MasterType.PayType).ToList();

                if (code == null)
                {
                    var result = modelMaster.Select(x => new SelectListItem
                    {
                        Text = x.master_value1,
                        Value = x.master_code
                    });
                    return result;
                }
                else
                {
                    var result = modelMaster.Select(x => new SelectListItem
                    {
                        Text = x.master_value1,
                        Value = x.master_code,
                        Selected = (x.master_code == code)
                    });
                    return result;
                }
            }

            return obj;
        }

        public static IEnumerable<SelectListItem> GetReportTypeForReport(string username)
        {
            List<ResultGetMasterViewModel> modelMaster = new List<ResultGetMasterViewModel>();
            var datas = SelectListService.GetMasterListData(username);
            var obj = new List<SelectListItem>();

            if (datas.isCompleted && datas.data != null)
            {
                modelMaster = datas.data
                    .Where(i => i.master_type == Constants.MasterType.ReportType).ToList();

                var result = modelMaster.Select(x => new SelectListItem
                {
                    Text = x.master_value1,
                    Value = x.master_code
                });
                return result;
            }

            return obj;
        }

        public static IEnumerable<SelectListItem> GetInfoDeptCode(string code = null)
        {
            List<ResultGetDeptViewModel> modelDept = new List<ResultGetDeptViewModel>();
            var datas = SelectListService.GetDepartmentListData();
            var obj = new List<SelectListItem>();

            if (datas.isCompleted && datas.data != null)
            {
                modelDept = datas.data;

                if (code == null)
                {
                    var result = modelDept.Select(x => new SelectListItem
                    {
                        Text = $"Dept. Code : {x.dept_code} | Dept. Name : {x.name_th} | Dept. ShortCode : {x.short_code}",
                        Value = x.dept_code
                    });
                    return result;
                }
                else
                {
                    var result = modelDept.Select(x => new SelectListItem
                    {
                        Text = $"Dept. Code : {x.dept_code} | Dept. Name : {x.name_th} | Dept. ShortCode : {x.short_code}",
                        Value = x.dept_code,
                        Selected = (x.dept_code == code)
                    });
                    return result;
                }
            }

            return obj;
        }

        public static IEnumerable<SelectListItem> GetIssuedDeptCode(string code = null)
        {
            List<ResultGetDeptViewModel> modelDept = new List<ResultGetDeptViewModel>();
            var datas = SelectListService.GetDepartmentListData();
            var obj = new List<SelectListItem>();

            if (datas.isCompleted && datas.data != null)
            {
                modelDept = datas.data;

                if (code == null)
                {
                    var result = modelDept.Select(x => new SelectListItem
                    {
                        Text = $"รหัสหน่วยงาน : {x.dept_code} | ชื่อหน่วยงาน : {x.name_th}",
                        Value = x.dept_code
                    });
                    return result;
                }
                else
                {
                    var result = modelDept.Select(x => new SelectListItem
                    {
                        Text = $"รหัสหน่วยงาน : {x.dept_code} | ชื่อหน่วยงาน : {x.name_th}",
                        Value = x.dept_code,
                        Selected = (x.dept_code == code)
                    });
                    return result;
                }
            }

            return obj;
        }

        public static IEnumerable<SelectListItem> GetIssuedDeptCodeForReport(string username)
        {
            List<ResultGetDeptViewModel> modelDept = new List<ResultGetDeptViewModel>();
            var datas = SelectListService.GetDepartmentListData(username);
            var obj = new List<SelectListItem>();

            if (datas.isCompleted && datas.data != null)
            {
                modelDept = datas.data;

                var result = modelDept.Select(x => new SelectListItem
                {
                    Text = $"{x.name_th}",
                    Value = x.dept_code
                });
                return result;
            }

            return obj;
        }

        public static IEnumerable<SelectListItem> GetPlantCode(string code = null)
        {
            List<ResultGetPlantViewModel> modelPlant = new List<ResultGetPlantViewModel>();
            var datas = SelectListService.GetPlantListData();
            var obj = new List<SelectListItem>();

            if (datas.isCompleted && datas.data != null)
            {
                modelPlant = datas.data;

                if (code == null)
                {
                    var result = modelPlant.Select(x => new SelectListItem
                    {
                        Text = $"Plant Code : {x.plant_code} | {x.name_th}",
                        Value = x.plant_code
                    });
                    return result;
                }
                else
                {
                    var result = modelPlant.Select(x => new SelectListItem
                    {
                        Text = $"Plant Code : {x.plant_code} | {x.name_th}",
                        Value = x.plant_code,
                        Selected = (x.plant_code == code)
                    });
                    return result;
                }
            }

            return obj;
        }

        public static IEnumerable<SelectListItem> GetCompanyCode(string code = null)
        {
            List<ResultGetCompViewModel> modelComp = new List<ResultGetCompViewModel>();
            var datas = SelectListService.GetCompanyListData();
            var obj = new List<SelectListItem>();

            if (datas.isCompleted && datas.data != null)
            {
                modelComp = datas.data;

                if (code == null)
                {
                    var result = modelComp.Select(x => new SelectListItem
                    {
                        Text = $"Company Code : {x.comp_code} | {x.name_th_line1}",
                        Value = x.comp_code
                    });
                    return result;
                }
                else
                {
                    var result = modelComp.Select(x => new SelectListItem
                    {
                        Text = $"Company Code : {x.comp_code} | {x.name_th_line1}",
                        Value = x.comp_code,
                        Selected = (x.comp_code == code)
                    });
                    return result;
                }
            }

            return obj;
        }

        public static IEnumerable<SelectListItem> GetPlantCodeForReport(string username)
        {
            List<ResultGetPlantViewModel> modelPlant = new List<ResultGetPlantViewModel>();
            var datas = SelectListService.GetPlantListData(username);
            var obj = new List<SelectListItem>();

            if (datas.isCompleted && datas.data != null)
            {
                modelPlant = datas.data;

                var result = modelPlant.Select(x => new SelectListItem
                {
                    Text = $"{x.name_th}",
                    Value = x.plant_code
                });
                return result;
            }

            return obj;
        }

        public static IEnumerable<SelectListItem> GetYear(int? code = null)
        {
            var obj = new List<SelectListItem>();
            int currentYear = DateTime.Now.Year;
            for (int year = currentYear - 5; year <= currentYear; year++)
            {
                obj.Add(new SelectListItem
                {
                    Value = year.ToString(),
                    Text = year.ToString(),
                    Selected = (code.HasValue && code == year)
                });
            }
            return obj;
        }

        public static IEnumerable<SelectListItem> GetMonth(int? code = null)
        {
            var obj = new List<SelectListItem>();
            for (int month = 1; month <= 12; month++)
            {
                DateTime monthDate = new DateTime(1, month, 1);
                obj.Add(new SelectListItem
                {
                    Value = month.ToString("00"),
                    Text = monthDate.ToString("MMMM"),
                    Selected = (code.HasValue && code == month)
                });
            }
            return obj;
        }

        public static IEnumerable<SelectListItem> GetStatus(string? status = null)
        {
            var obj = new List<SelectListItem>
            {
                new SelectListItem { Value = "A", Text = "Normal", Selected = (status == "A") },
                new SelectListItem { Value = "I", Text = "Hidden", Selected = (status == "I") },
                new SelectListItem { Value = "D", Text = "Cancel", Selected = (status == "D") }
            };
            return obj;
        }

        public static IEnumerable<SelectListItem> GetGroupMaster(string? groupMasterCode = null)
        {
            List<ResultGetGroupMasterViewModel> modelGroupMaster = new List<ResultGetGroupMasterViewModel>();
            var datas = SelectListService.GetGroupMasterListData();
            var obj = new List<SelectListItem>();

            if (datas.isCompleted && datas.data != null)
            {
                modelGroupMaster = datas.data.ToList();

                if (groupMasterCode == null)
                {
                    var result = modelGroupMaster.Select(x => new SelectListItem
                    {
                        Text = x.group_name,
                        Value = x.group_code
                    });
                    return result;
                }
                else
                {
                    var result = modelGroupMaster.Select(x => new SelectListItem
                    {
                        Text = x.group_name,
                        Value = x.group_code,
                        Selected = (x.group_code == groupMasterCode)
                    });
                    return result;
                }
            }

            return obj;
        }

        public static IEnumerable<SelectListItem> GetItemMaster(string? itemMasterCode = null)
        {
            List<ResultSearchItemMasterViewModel> modelItemMaster = new List<ResultSearchItemMasterViewModel>();
            var datas = SelectListService.GetItemMasterListData();
            var obj = new List<SelectListItem>();

            if (datas.isCompleted && datas.data != null)
            {
                modelItemMaster = datas.data.ToList();

                if (itemMasterCode == null)
                {
                    var result = modelItemMaster.Select(x => new SelectListItem
                    {
                        Text = x.item_name,
                        Value = x.item_code
                    });
                    return result;
                }
                else
                {
                    var result = modelItemMaster.Select(x => new SelectListItem
                    {
                        Text = x.item_name,
                        Value = x.item_code,
                        Selected = (x.item_code == itemMasterCode)
                    });
                    return result;
                }
            }

            return obj;
        }

        public static IEnumerable<SelectListItem> GetSupplier(int? supplierCode = null)
        {
            List<ResultSearchSupplierViewModel> modelSupplier = new List<ResultSearchSupplierViewModel>();
            var datas = SelectListService.GetSupplierListData();
            var obj = new List<SelectListItem>();

            if (datas.isCompleted && datas.data != null)
            {
                modelSupplier = datas.data.ToList();

                if (supplierCode == null)
                {
                    var result = modelSupplier.Select(x => new SelectListItem
                    {
                        Text = x.supplier_name,
                        Value = x.supplier_code.ToString()
                    });
                    return result;
                }
                else
                {
                    var result = modelSupplier.Select(x => new SelectListItem
                    {
                        Text = x.supplier_name,
                        Value = x.supplier_code.ToString(),
                        Selected = (x.supplier_code == supplierCode)
                    });
                    return result;
                }
            }

            return obj;
        }

        public static IEnumerable<SelectListItem> GetSender(int? senderId = null)
        {
            List<ResultSearchSenderViewModel> modelSender = new List<ResultSearchSenderViewModel>();
            var datas = SelectListService.GetSenderListData();
            var obj = new List<SelectListItem>();

            if (datas.isCompleted && datas.data != null)
            {
                modelSender = datas.data.ToList();

                if (senderId == null)
                {
                    var result = modelSender.Select(x => new SelectListItem
                    {
                        Text = x.sender_name,
                        Value = x.id.ToString()
                    });
                    return result;
                }
                else
                {
                    var result = modelSender.Select(x => new SelectListItem
                    {
                        Text = x.sender_name,
                        Value = x.id.ToString(),
                        Selected = (x.id == senderId)
                    });
                    return result;
                }
            }

            return obj;
        }

        public static IEnumerable<SelectListItem> GetWeightOutStatus(string? status = null)
        {
            var obj = new List<SelectListItem>
            {
                new SelectListItem { Value = "ชั่งออก", Text = "ชั่งออก", Selected = (status == "ชั่งออก") },
                new SelectListItem { Value = "รับบางส่วน", Text = "รับบางส่วน", Selected = (status == "รับบางส่วน") },
                new SelectListItem { Value = "ไม่รับสินค้า", Text = "ไม่รับสินค้า", Selected = (status == "ไม่รับสินค้า") }
            };
            return obj;
        }

        public static IEnumerable<SelectListItem> GetCarType(string? type = null)
        {
            var obj = new List<SelectListItem>
            {
                new SelectListItem { Value = "รถเดี่ยว", Text = "รถเดี่ยว", Selected = (type == "รถเดี่ยว") },
                new SelectListItem { Value = "รถเทรลเลอร์", Text = "รถเทรลเลอร์", Selected = (type == "รถเทรลเลอร์") },
                new SelectListItem { Value = "รถพ่วง", Text = "รถพ่วง", Selected = (type == "รถพ่วง") },
                new SelectListItem { Value = "รถอื่นๆ", Text = "รถอื่นๆ", Selected = (type == "รถอื่นๆ") }
            };
            return obj;
        }
    }
}
