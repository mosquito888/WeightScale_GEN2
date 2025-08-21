using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository;
using WeightScaleGen2.BGC.API.Common;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.Base;
using WeightScaleGen2.BGC.Models.ViewModels.Log;
using ILogger = WeightScaleGen2.BGC.API.Common.Logger.ILogger;

namespace WeightScaleGen2.BGC.API.APIServices
{
    public class LogAPIService : BaseAPIService
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;
        ILogger _logger;
        private readonly IHttpContextAccessor _context;
        private readonly UserInfoModel _userInfo;
        private LogRepository _logRepository;

        public LogAPIService(
            IDatabaseConnectionFactory db,
            ISecurityCommon securityCommon,
            ILogger logger,
            IHttpContextAccessor context,
            UserInfoModel userInfo,
            LogRepository logRepository) : base(db, securityCommon)
        {
            _db = db;
            _securityCommon = securityCommon;
            _logger = logger;
            _context = context;
            _userInfo = _getUserInfo(_context.HttpContext.Session.GetString(Constants.Session.User)).Result;
            _logRepository = logRepository;
        }

        public Task<ReturnObject<ResultSearchLogCriteriaViewModel>> GetSearchLogCriteria()
        {
            var result = new ReturnObject<ResultSearchLogCriteriaViewModel>();
            try
            {
                ResultSearchLogCriteriaViewModel res = new ResultSearchLogCriteriaViewModel();
                var lsItemLevel = _logRepository.Select_ListLogLevelDll_All(_userInfo).Result;

                res.level_item = _initRoleToLogLevel(lsItemLevel);
                result.data = res;
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Log.Service.GetSearchLogCriteria}");
                _logger.WriteError(errorCode: ErrorCodes.Log.Service.GetSearchLogCriteria, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnList<ResultSearchLogViewModel>> SearchLogData(ParamSearchLogViewModel param)
        {
            var result = new ReturnList<ResultSearchLogViewModel>();
            try
            {
                var searchData = _logRepository.Select_ListLogDataDll_By(param, _userInfo).Result;

                result.data = _initSearchLog(searchData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Log.Service.SearchLogData}");
                _logger.WriteError(errorCode: ErrorCodes.Log.Service.SearchLogData, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        private List<ResultSearchLogViewModel> _initSearchLog(List<LogData> searchData)
        {
            List<ResultSearchLogViewModel> result = new List<ResultSearchLogViewModel>();

            foreach (LogData p in searchData)
            {
                result.Add(new ResultSearchLogViewModel
                {
                    level = p.log_level,
                    log_date = p.log_date,
                    message = p.log_message,
                    username = p.log_user,
                    exception_message = p.log_exception_message,
                    log_caller_file_path = p.log_caller_file_path,
                    log_source_line_number = p.log_source_line_number,
                    total_record = p.total_record
                });
            }

            return result;
        }

        private IEnumerable<BaseDLLViewModel> _initRoleToLogLevel(List<LogLevelData> data)
        {
            List<BaseDLLViewModel> result = new List<BaseDLLViewModel>();

            foreach (LogLevelData p in data)
            {
                result.Add(new BaseDLLViewModel
                {
                    text = p.level_value,
                    value = this._securityCommon.EncryptDataUrlEncoder(p.level_code.ToString()),
                    is_active = true
                });
            }

            return result;
        }
    }
}
