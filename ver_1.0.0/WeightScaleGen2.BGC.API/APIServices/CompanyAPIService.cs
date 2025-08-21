using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository;
using WeightScaleGen2.BGC.API.Common;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.Company;
using ILogger = WeightScaleGen2.BGC.API.Common.Logger.ILogger;

namespace WeightScaleGen2.BGC.API.APIServices
{
    public class CompanyAPIService : BaseAPIService
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;
        ILogger _logger;
        private readonly IHttpContextAccessor _context;
        private readonly UserInfoModel _userInfo;
        private CompanyRepository _companyRepository;

        public CompanyAPIService(
            IDatabaseConnectionFactory db,
            ISecurityCommon securityCommon,
            ILogger logger,
            IHttpContextAccessor context,
            UserInfoModel userInfo,
            CompanyRepository companyRepository) : base(db, securityCommon)
        {
            _db = db;
            _logger = logger;
            _context = context;
            _userInfo = _getUserInfo(_context.HttpContext.Session.GetString(Constants.Session.User)).Result;
            _companyRepository = companyRepository;
        }

        public Task<ReturnObject<bool>> PostInfo(ResultGetCompInfoViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var res = _companyRepository.Insert_Info(param, _userInfo).Result;

                result.data = res.is_success;
                result.isCompleted = res.is_success;
                result.message.Add(res.message);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Company.Service.PostInfo}");
                _logger.WriteError(errorCode: ErrorCodes.Company.Service.PostInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnObject<bool>> PutInfo(ResultGetCompInfoViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var res = _companyRepository.Update_Info(param, _userInfo).Result;

                result.data = res.is_success;
                result.isCompleted = res.is_success;
                result.message.Add(res.message);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Company.Service.PutInfo}");
                _logger.WriteError(errorCode: ErrorCodes.Company.Service.PutInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnObject<bool>> DeleteInfo(ResultGetCompInfoViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var res = _companyRepository.Delete_Info(param, _userInfo).Result;

                result.data = res.is_success;
                result.isCompleted = res.is_success;
                result.message.Add(res.message);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Company.Service.DeleteInfo}");
                _logger.WriteError(errorCode: ErrorCodes.Company.Service.DeleteInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnList<ResultGetCompViewModel>> GetListComp()
        {
            var result = new ReturnList<ResultGetCompViewModel>();
            try
            {
                var lsData = _companyRepository.Select_Company_All(_userInfo).Result;

                result.data = _initCompanyListData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Company.Service.GetListComp}");
                _logger.WriteError(errorCode: ErrorCodes.Company.Service.GetListComp, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnList<ResultSearchCompViewModel>> GetSearchListComp(ParamSearchCompViewModel param)
        {
            var result = new ReturnList<ResultSearchCompViewModel>();
            try
            {
                var lsData = _companyRepository.Select_Company_By(param, _userInfo).Result;

                result.data = _initSearchCompanyListData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Company.Service.GetSearchListComp}");
                _logger.WriteError(errorCode: ErrorCodes.Company.Service.GetSearchListComp, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        private List<ResultGetCompViewModel> _initCompanyListData(List<CompanyData> lsComp)
        {
            List<ResultGetCompViewModel> result = new List<ResultGetCompViewModel>();

            foreach (var item in lsComp)
            {
                ResultGetCompViewModel data = new ResultGetCompViewModel
                {
                    comp_code = item.comp_code,
                    name_th_line1 = item.name_th_line1,
                    name_th_line2 = item.name_th_line2,
                    name_en_line1 = item.name_en_line1,
                    name_en_line2 = item.name_en_line2,
                    addr_th_line1 = item.addr_th_line1,
                    addr_th_line2 = item.addr_th_line2,
                    addr_en_line1 = item.addr_en_line1,
                    addr_en_line2 = item.addr_en_line2,
                    phone = item.phone,
                    province_code = item.province_code,
                    head_report = item.head_report,
                    approve_name = item.approve_name,
                    edit_after_print = item.edit_after_print,
                    created_by = item.created_by,
                    created_date = item.created_date,
                    modified_by = item.modified_by,
                    modified_date = item.modified_date,
                    is_active = item.is_active,
                    is_deleted = item.is_deleted
                };
                result.Add(data);
            }

            return result;
        }

        private List<ResultSearchCompViewModel> _initSearchCompanyListData(List<CompanyData> lsComp)
        {
            List<ResultSearchCompViewModel> result = new List<ResultSearchCompViewModel>();

            foreach (var item in lsComp)
            {
                ResultSearchCompViewModel data = new ResultSearchCompViewModel
                {
                    comp_code = item.comp_code,
                    name_th_line1 = item.name_th_line1,
                    name_th_line2 = item.name_th_line2,
                    name_en_line1 = item.name_en_line1,
                    name_en_line2 = item.name_en_line2,
                    addr_th_line1 = item.addr_th_line1,
                    addr_th_line2 = item.addr_th_line2,
                    addr_en_line1 = item.addr_en_line1,
                    addr_en_line2 = item.addr_en_line2,
                    phone = item.phone,
                    province_code = item.province_code,
                    head_report = item.head_report,
                    approve_name = item.approve_name,
                    edit_after_print = item.edit_after_print,
                    created_by = item.created_by,
                    created_date = item.created_date,
                    modified_by = item.modified_by,
                    modified_date = item.modified_date,
                    is_active = item.is_active,
                    is_deleted = item.is_deleted,
                    total_record = item.total_record
                };
                result.Add(data);
            }

            return result;
        }
    }
}
