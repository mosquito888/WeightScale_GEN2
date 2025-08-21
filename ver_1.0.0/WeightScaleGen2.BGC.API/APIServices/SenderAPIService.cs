using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository;
using WeightScaleGen2.BGC.API.Common;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.Sender;
using ILogger = WeightScaleGen2.BGC.API.Common.Logger.ILogger;

namespace WeightScaleGen2.BGC.API.APIServices
{
    public class SenderAPIService : BaseAPIService
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;
        ILogger _logger;
        private readonly IHttpContextAccessor _context;
        private readonly UserInfoModel _userInfo;
        private SenderRepository _senderRepository;

        public SenderAPIService(
            IDatabaseConnectionFactory db,
            ISecurityCommon securityCommon,
            ILogger logger,
            IHttpContextAccessor context,
            UserInfoModel userInfo,
            SenderRepository senderRepository) : base(db, securityCommon)
        {
            _db = db;
            _securityCommon = securityCommon;
            _logger = logger;
            _context = context;
            _userInfo = _getUserInfo(_context.HttpContext.Session.GetString(Constants.Session.User)).Result;
            _senderRepository = senderRepository;
        }

        public Task<ReturnObject<bool>> PostSenderInfo(ResultGetSenderInfoViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var res = _senderRepository.Insert_SenderInfo(param, _userInfo).Result;

                result.data = res.is_success;
                result.isCompleted = res.is_success;
                result.message.Add(res.message);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Sender.Service.PostSenderInfo}");
                _logger.WriteError(errorCode: ErrorCodes.Sender.Service.PostSenderInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnObject<bool>> PutSenderInfo(ResultGetSenderInfoViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var res = _senderRepository.Update_SenderInfo(param, _userInfo).Result;

                result.data = res.is_success;
                result.isCompleted = res.is_success;
                result.message.Add(res.message);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Sender.Service.PutSenderInfo}");
                _logger.WriteError(errorCode: ErrorCodes.Sender.Service.PutSenderInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnList<ResultSearchSenderViewModel>> GetSenderListData()
        {
            var result = new ReturnList<ResultSearchSenderViewModel>();
            try
            {
                var lsData = _senderRepository.Select_SenderListData_All(_userInfo).Result;

                result.data = _initSearchSenderListData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Sender.Service.GetListSender}");
                _logger.WriteError(errorCode: ErrorCodes.Sender.Service.GetListSender, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnList<ResultSearchSenderViewModel>> GetSearchListSender(ParamSearchSenderViewModel param)
        {
            var result = new ReturnList<ResultSearchSenderViewModel>();
            try
            {
                var lsData = _senderRepository.Select_SearchSenderListData_By(param, _userInfo).Result;

                result.data = _initSearchSenderListData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Sender.Service.GetSearchListSender}");
                _logger.WriteError(errorCode: ErrorCodes.Sender.Service.GetSearchListSender, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnObject<ResultGetSenderInfoViewModel>> GetSenderInfo(ParamSenderInfo param)
        {
            var result = new ReturnObject<ResultGetSenderInfoViewModel>();
            try
            {
                var lsData = _senderRepository.Select_SenderInfo(param, _userInfo).Result;

                result.data = _initSenderInfo(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Sender.Service.GetSenderInfo}");
                _logger.WriteError(errorCode: ErrorCodes.Sender.Service.GetSenderInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnObject<bool>> DeleteInfo(ResultGetSenderInfoViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var res = _senderRepository.Delete_Info(param, _userInfo).Result;

                result.data = res.is_success;
                result.isCompleted = res.is_success;
                result.message.Add(res.message);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Sender.Service.DeleteSenderInfo}");
                _logger.WriteError(errorCode: ErrorCodes.Sender.Service.DeleteSenderInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        private List<ResultSearchSenderViewModel> _initSearchSenderListData(List<SenderData> lsSen)
        {
            List<ResultSearchSenderViewModel> result = new List<ResultSearchSenderViewModel>();

            foreach (SenderData objSen in lsSen)
            {
                result.Add(new ResultSearchSenderViewModel
                {
                    id = objSen.id,
                    sender_name = objSen.sender_name,
                    flag_delete = objSen.flag_delete,
                    created_by = objSen.created_by,
                    created_date = objSen.created_date,
                    modified_by = objSen.modified_by,
                    modified_date = objSen.modified_date,
                    is_active = objSen.is_active,
                    is_deleted = objSen.is_deleted,
                    total_record = objSen.total_record
                });
            }

            return result;
        }

        private ResultGetSenderInfoViewModel _initSenderInfo(SenderData senderInfo)
        {
            ResultGetSenderInfoViewModel result = new ResultGetSenderInfoViewModel();
            if (senderInfo != null)
            {
                result.id = senderInfo.id;
                result.sender_name = senderInfo.sender_name;
                result.flag_delete = senderInfo.flag_delete;
                result.created_by = senderInfo.created_by;
                result.created_date = senderInfo.created_date;
                result.modified_by = senderInfo.modified_by;
                result.modified_date = senderInfo.modified_date;
                result.is_active = senderInfo.is_active;
                result.is_deleted = senderInfo.is_deleted;
            }

            return result;
        }

    }
}
