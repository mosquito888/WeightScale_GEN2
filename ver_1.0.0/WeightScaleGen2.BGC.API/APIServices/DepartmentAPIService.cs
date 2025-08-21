using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository;
using WeightScaleGen2.BGC.API.Common;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.Department;
using ILogger = WeightScaleGen2.BGC.API.Common.Logger.ILogger;

namespace WeightScaleGen2.BGC.API.APIServices
{
    public class DepartmentAPIService : BaseAPIService
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;
        ILogger _logger;
        private readonly IHttpContextAccessor _context;
        private readonly UserInfoModel _userInfo;
        private DepartmentRepository _departmentRepository;

        public DepartmentAPIService(
            IDatabaseConnectionFactory db,
            ISecurityCommon securityCommon,
            ILogger logger,
            IHttpContextAccessor context,
            UserInfoModel userInfo,
            DepartmentRepository departmentRepository) : base(db, securityCommon)
        {
            _db = db;
            _securityCommon = securityCommon;
            _logger = logger;
            _context = context;
            _userInfo = _getUserInfo(_context.HttpContext.Session.GetString(Constants.Session.User)).Result;
            _departmentRepository = departmentRepository;
        }

        public Task<ReturnObject<bool>> PostInfo(ResultGetDeptInfoViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var res = _departmentRepository.Insert_Info(param, _userInfo).Result;

                result.data = res.is_success;
                result.isCompleted = res.is_success;
                result.message.Add(res.message);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Department.Service.PostInfo}");
                _logger.WriteError(errorCode: ErrorCodes.Department.Service.PostInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnObject<bool>> PutInfo(ResultGetDeptInfoViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var res = _departmentRepository.Update_Info(param, _userInfo).Result;

                result.data = res.is_success;
                result.isCompleted = res.is_success;
                result.message.Add(res.message);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Department.Service.PutInfo}");
                _logger.WriteError(errorCode: ErrorCodes.Department.Service.PutInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnObject<bool>> DeleteInfo(ResultGetDeptInfoViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var res = _departmentRepository.Delete_Info(param, _userInfo).Result;

                result.data = res.is_success;
                result.isCompleted = res.is_success;
                result.message.Add(res.message);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Department.Service.DeleteInfo}");
                _logger.WriteError(errorCode: ErrorCodes.Department.Service.DeleteInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnList<ResultGetDeptViewModel>> GetListDept()
        {
            var result = new ReturnList<ResultGetDeptViewModel>();
            try
            {
                var lsData = _departmentRepository.Select_DepartmentData_All().Result;

                result.data = _initDepartmentListData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Department.Service.GetListDept}");
                _logger.WriteError(errorCode: ErrorCodes.Department.Service.GetListDept, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnList<ResultSearchDeptViewModel>> GetSearchListDept(ParamSearchDeptViewModel param)
        {
            var result = new ReturnList<ResultSearchDeptViewModel>();
            try
            {
                var lsData = _departmentRepository.Select_DepartmentData_By(param, _userInfo).Result;

                result.data = _initSearchDepartmentListData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Department.Service.GetSearchListDept}");
                _logger.WriteError(errorCode: ErrorCodes.Department.Service.GetSearchListDept, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        private List<ResultGetDeptViewModel> _initDepartmentListData(List<DepartmentData> lsDept)
        {
            List<ResultGetDeptViewModel> result = new List<ResultGetDeptViewModel>();

            foreach (var item in lsDept)
            {
                ResultGetDeptViewModel data = new ResultGetDeptViewModel()
                {
                    comp_code = item.comp_code,
                    plant_code = item.plant_code,
                    dept_code = item.dept_code,
                    short_code = item.short_code,
                    name_th = item.name_th,
                    name_en = item.name_en,
                    created_by = item.created_by,
                    created_date = item.created_date,
                    modified_by = item.modified_by,
                    modified_date = item.modified_date,
                    is_active = item.is_active,
                    is_deleted = item.is_deleted,
                    is_all = item.is_all
                };
                result.Add(data);
            }

            return result;
        }

        private List<ResultSearchDeptViewModel> _initSearchDepartmentListData(List<DepartmentData> lsDept)
        {
            List<ResultSearchDeptViewModel> result = new List<ResultSearchDeptViewModel>();

            foreach (var item in lsDept)
            {
                ResultSearchDeptViewModel data = new ResultSearchDeptViewModel()
                {
                    comp_code = item.comp_code,
                    plant_code = item.plant_code,
                    dept_code = item.dept_code,
                    short_code = item.short_code,
                    name_th = item.name_th,
                    name_en = item.name_en,
                    created_by = item.created_by,
                    created_date = item.created_date,
                    modified_by = item.modified_by,
                    modified_date = item.modified_date,
                    is_active = item.is_active,
                    is_deleted = item.is_deleted,
                    is_all = item.is_all,
                    total_record = item.total_record
                };
                result.Add(data);
            }

            return result;
        }
    }
}
