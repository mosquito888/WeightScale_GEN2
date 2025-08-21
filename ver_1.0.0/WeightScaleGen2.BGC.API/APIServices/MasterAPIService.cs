using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository;
using WeightScaleGen2.BGC.API.Common;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.Master;
using ILogger = WeightScaleGen2.BGC.API.Common.Logger.ILogger;

namespace WeightScaleGen2.BGC.API.APIServices
{
    public class MasterAPIService : BaseAPIService
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;
        ILogger _logger;
        private readonly IHttpContextAccessor _context;
        private readonly UserInfoModel _userInfo;
        private MasterRepository _masterRepository;

        public MasterAPIService(
            IDatabaseConnectionFactory db,
            ISecurityCommon securityCommon,
            ILogger logger,
            IHttpContextAccessor context,
            UserInfoModel userInfo,
            MasterRepository masterRepository) : base(db, securityCommon)
        {
            _db = db;
            _securityCommon = securityCommon;
            _logger = logger;
            _context = context;
            _userInfo = _getUserInfo(_context.HttpContext.Session.GetString(Constants.Session.User)).Result;
            _masterRepository = masterRepository;
        }

        public Task<ReturnObject<bool>> PostInfo(ResultGetMasterInfoViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var res = _masterRepository.Insert_Info(param, _userInfo).Result;

                result.data = res.is_success;
                result.isCompleted = res.is_success;
                result.message.Add(res.message);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Master.Service.PostInfo}");
                _logger.WriteError(errorCode: ErrorCodes.Master.Service.PostInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }

            return Task.FromResult(result);
        }

        public Task<ReturnObject<bool>> PutInfo(ResultGetMasterInfoViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var res = _masterRepository.Update_Info(param, _userInfo).Result;

                result.data = res.is_success;
                result.isCompleted = res.is_success;
                result.message.Add(res.message);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Master.Service.PutInfo}");
                _logger.WriteError(errorCode: ErrorCodes.Master.Service.PutInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }

            return Task.FromResult(result);
        }

        public Task<ReturnObject<bool>> DeleteInfo(ResultGetMasterInfoViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var res = _masterRepository.Delete_Info(param, _userInfo).Result;

                result.data = res.is_success;
                result.isCompleted = res.is_success;
                result.message.Add(res.message);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Master.Service.DeleteInfo}");
                _logger.WriteError(errorCode: ErrorCodes.Master.Service.DeleteInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }

            return Task.FromResult(result);
        }

        public Task<ReturnList<ResultGetMasterTypeViewModel>> GetListMasterType()
        {
            var result = new ReturnList<ResultGetMasterTypeViewModel>();
            try
            {
                var lsData = _getMasterDataType().Result;

                result.data = _initMasterListDataType(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Master.Service.GetListMasterType}");
                _logger.WriteError(errorCode: ErrorCodes.Master.Service.GetListMasterType, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }

            return Task.FromResult(result);
        }

        public Task<ReturnList<ResultGetMasterViewModel>> GetListMaster()
        {
            var result = new ReturnList<ResultGetMasterViewModel>();
            try
            {
                var lsData = _getMasterDataAll().Result;

                result.data = _initMasterListData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Master.Service.GetListMaster}");
                _logger.WriteError(errorCode: ErrorCodes.Master.Service.GetListMaster, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }

            return Task.FromResult(result);
        }

        public Task<ReturnList<ResultSearchMasterViewModel>> GetSearchListMaster(ParamSearchMasterViewModel param)
        {
            var result = new ReturnList<ResultSearchMasterViewModel>();
            try
            {
                var lsData = _masterRepository.Select_MasterData(param, _userInfo).Result;

                result.data = _initSearchMasterListData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Master.Service.GetSearchListMaster}");
                _logger.WriteError(errorCode: ErrorCodes.Master.Service.GetSearchListMaster, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }

            return Task.FromResult(result);
        }

        private List<ResultGetMasterViewModel> _initMasterListData(List<MasterData> lsMaster)
        {
            List<ResultGetMasterViewModel> result = new List<ResultGetMasterViewModel>();

            foreach (MasterData objMaster in lsMaster)
            {
                result.Add(new ResultGetMasterViewModel
                {
                    comp_code = objMaster.comp_code,
                    plant_code = objMaster.plant_code,
                    master_type = objMaster.master_type,
                    master_code = objMaster.master_code,
                    master_value1 = objMaster.master_value1,
                    master_value2 = objMaster.master_value2,
                    master_value3 = objMaster.master_value3,
                    master_desc_th = objMaster.master_desc_th,
                    master_desc_en = objMaster.master_desc_en,
                    is_active = objMaster.is_active,
                    is_deleted = objMaster.is_deleted,
                    is_all = objMaster.is_all
                });
            }

            return result;
        }

        private List<ResultGetMasterTypeViewModel> _initMasterListDataType(List<MasterDataType> lsMaster)
        {
            List<ResultGetMasterTypeViewModel> result = new List<ResultGetMasterTypeViewModel>();

            foreach (MasterDataType objMaster in lsMaster)
            {
                result.Add(new ResultGetMasterTypeViewModel
                {
                    master_type = objMaster.master_type,
                    master_type_desc = objMaster.master_type_desc,
                    is_add = objMaster.is_add,
                    is_not_edit = objMaster.is_not_edit,
                    is_not_del = objMaster.is_not_del
                });
            }

            return result;
        }

        private List<ResultSearchMasterViewModel> _initSearchMasterListData(List<MasterData> lsMaster)
        {
            List<ResultSearchMasterViewModel> result = new List<ResultSearchMasterViewModel>();

            foreach (MasterData objMaster in lsMaster)
            {
                result.Add(new ResultSearchMasterViewModel
                {
                    comp_code = objMaster.comp_code,
                    plant_code = objMaster.plant_code,
                    master_type = objMaster.master_type,
                    master_code = objMaster.master_code,
                    master_value1 = objMaster.master_value1,
                    master_value2 = objMaster.master_value2,
                    master_value3 = objMaster.master_value3,
                    master_desc_th = objMaster.master_desc_th,
                    master_desc_en = objMaster.master_desc_en,
                    is_active = objMaster.is_active,
                    is_deleted = objMaster.is_deleted,
                    is_all = objMaster.is_all,
                    total_record = objMaster.total_record
                });
            }

            return result;
        }
    }
}
