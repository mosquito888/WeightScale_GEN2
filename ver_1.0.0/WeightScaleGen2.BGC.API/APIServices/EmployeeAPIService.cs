using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository;
using WeightScaleGen2.BGC.API.Common;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.Employee;
using ILogger = WeightScaleGen2.BGC.API.Common.Logger.ILogger;

namespace WeightScaleGen2.BGC.API.APIServices
{
    public class EmployeeAPIService : BaseAPIService
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;
        ILogger _logger;
        private readonly IHttpContextAccessor _context;
        private readonly UserInfoModel _userInfo;
        private EmployeeRepository _employeeRepository;

        public EmployeeAPIService(
            IDatabaseConnectionFactory db,
            ISecurityCommon securityCommon,
            ILogger logger,
            IHttpContextAccessor context,
            UserInfoModel userInfo,
            EmployeeRepository employeeRepository) : base(db, securityCommon)
        {
            _db = db;
            _securityCommon = securityCommon;
            _logger = logger;
            _context = context;
            _userInfo = _getUserInfo(_context.HttpContext.Session.GetString(Constants.Session.User)).Result;
            _employeeRepository = employeeRepository;
        }

        public Task<ReturnObject<bool>> PostEmployeeInfo(ResultGetEmpInfoViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var res = _employeeRepository.Insert_EmployeeInfo(param, _userInfo).Result;

                result.data = res.is_success;
                result.isCompleted = res.is_success;
                result.message.Add(res.message);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Employee.Service.PostEmployeeInfo}");
                _logger.WriteError(errorCode: ErrorCodes.Employee.Service.PostEmployeeInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnObject<bool>> PutEmployeeInfo(ResultGetEmpInfoViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var res = _employeeRepository.Update_EmployeeInfo(param, _userInfo).Result;

                result.data = res.is_success;
                result.isCompleted = res.is_success;
                result.message.Add(res.message);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Employee.Service.PutEmployeeInfo}");
                _logger.WriteError(errorCode: ErrorCodes.Employee.Service.PutEmployeeInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnList<ResultSearchEmpViewModel>> GetSearchListEmp(ParamSearchEmpViewModel param)
        {
            var result = new ReturnList<ResultSearchEmpViewModel>();
            try
            {
                var lsData = _employeeRepository.Select_SearchEmployeeListData_By(param, _userInfo).Result;

                result.data = _initSearchEmployeeListData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Employee.Service.GetSearchListEmp}");
                _logger.WriteError(errorCode: ErrorCodes.Employee.Service.GetSearchListEmp, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnObject<ResultGetEmpInfoViewModel>> GetEmpInfo(ParamEmpInfo param)
        {
            var result = new ReturnObject<ResultGetEmpInfoViewModel>();
            try
            {
                var lsData = _employeeRepository.Select_EmployeeInfo(param, _userInfo).Result;

                result.data = _initEmployeeInfo(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Employee.Service.GetEmpInfo}");
                _logger.WriteError(errorCode: ErrorCodes.Employee.Service.GetEmpInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnList<ResultGetEmpViewModel>> GetListEmp()
        {
            var result = new ReturnList<ResultGetEmpViewModel>();
            try
            {
                var lsData = _employeeRepository.Select_EmployeeListData_All(_userInfo).Result;

                result.data = _initEmployeeListData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Employee.Service.GetListEmp}");
                _logger.WriteError(errorCode: ErrorCodes.Employee.Service.GetListEmp, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        private List<ResultGetEmpViewModel> _initEmployeeListData(List<EmployeeData> lsEmp)
        {
            List<ResultGetEmpViewModel> result = new List<ResultGetEmpViewModel>();

            foreach (EmployeeData objEmp in lsEmp)
            {
                result.Add(new ResultGetEmpViewModel
                {
                    emp_code = objEmp.emp_code,
                    comp_code = objEmp.comp_code,
                    plant_code = objEmp.plant_code,
                    dept_code = objEmp.dept_code,
                    name = objEmp.name,
                    pay_type = objEmp.pay_type,
                    work_start_date = objEmp.work_start_date.HasValue ? DateOnly.FromDateTime(objEmp.work_start_date.Value) : null,
                    addr_line1 = objEmp.addr_line1,
                    addr_line2 = objEmp.addr_line2,
                    phone = objEmp.phone,
                    email = objEmp.email,
                    img_name = objEmp.img_name,
                    img_type = objEmp.img_type,
                    img_byte = objEmp.img_byte,
                    created_by = objEmp.created_by,
                    created_date = objEmp.created_date,
                    modified_by = objEmp.modified_by,
                    modified_date = objEmp.modified_date,
                    is_active = objEmp.is_active,
                    is_deleted = objEmp.is_deleted
                });
            }

            return result;
        }

        private List<ResultSearchEmpViewModel> _initSearchEmployeeListData(List<EmployeeData> lsEmp)
        {
            List<ResultSearchEmpViewModel> result = new List<ResultSearchEmpViewModel>();

            foreach (EmployeeData objEmp in lsEmp)
            {
                result.Add(new ResultSearchEmpViewModel
                {
                    emp_code = objEmp.emp_code,
                    comp_code = objEmp.comp_code,
                    plant_code = objEmp.plant_code,
                    dept_code = objEmp.dept_code,
                    name = objEmp.name,
                    pay_type = objEmp.pay_type,
                    work_start_date = objEmp.work_start_date.HasValue ? DateOnly.FromDateTime(objEmp.work_start_date.Value) : null,
                    addr_line1 = objEmp.addr_line1,
                    addr_line2 = objEmp.addr_line2,
                    phone = objEmp.phone,
                    email = objEmp.email,
                    img_name = objEmp.img_name,
                    img_type = objEmp.img_type,
                    img_byte = objEmp.img_byte,
                    created_by = objEmp.created_by,
                    created_date = objEmp.created_date,
                    modified_by = objEmp.modified_by,
                    modified_date = objEmp.modified_date,
                    is_active = objEmp.is_active,
                    is_deleted = objEmp.is_deleted,
                    total_record = objEmp.total_record
                });
            }

            return result;
        }

        private ResultGetEmpInfoViewModel _initEmployeeInfo(EmployeeData empInfo)
        {
            ResultGetEmpInfoViewModel result = new ResultGetEmpInfoViewModel();
            if (empInfo != null)
            {
                result.emp_code = empInfo.emp_code;
                result.comp_code = empInfo.comp_code;
                result.plant_code = empInfo.plant_code;
                result.dept_code = empInfo.dept_code;
                result.name = empInfo.name;
                result.pay_type = empInfo.pay_type;
                result.work_start_date = empInfo.work_start_date.HasValue ? DateOnly.FromDateTime(empInfo.work_start_date.Value) : null;
                result.addr_line1 = empInfo.addr_line1;
                result.addr_line2 = empInfo.addr_line2;
                result.phone = empInfo.phone;
                result.email = empInfo.email;
                result.img_name = empInfo.img_name;
                result.img_type = empInfo.img_type;
                result.img_byte = empInfo.img_byte;
                result.created_by = empInfo.created_by;
                result.created_date = empInfo.created_date;
                result.modified_by = empInfo.modified_by;
                result.modified_date = empInfo.modified_date;
                result.is_active = empInfo.is_active;
                result.is_deleted = empInfo.is_deleted;
            }

            return result;
        }

    }
}
