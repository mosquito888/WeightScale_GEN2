using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository;
using WeightScaleGen2.BGC.API.Common;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.Plant;
using ILogger = WeightScaleGen2.BGC.API.Common.Logger.ILogger;

namespace WeightScaleGen2.BGC.API.APIServices
{
    public class PlantAPIService : BaseAPIService
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;
        ILogger _logger;
        private readonly IHttpContextAccessor _context;
        private readonly UserInfoModel _userInfo;
        private PlantRepository _plantRepository;

        public PlantAPIService(
            IDatabaseConnectionFactory db,
            ISecurityCommon securityCommon,
            ILogger logger,
            IHttpContextAccessor context,
            UserInfoModel userInfo,
            PlantRepository plantRepository) : base(db, securityCommon)
        {
            _db = db;
            _securityCommon = securityCommon;
            _logger = logger;
            _context = context;
            _userInfo = _getUserInfo(_context.HttpContext.Session.GetString(Constants.Session.User)).Result;
            _plantRepository = plantRepository;
        }

        public Task<ReturnObject<bool>> PostInfo(ResultGetPlantInfoViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var res = _plantRepository.Insert_Info(param, _userInfo).Result;

                result.data = true;
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Plant.Service.PostInfo}");
                _logger.WriteError(errorCode: ErrorCodes.Plant.Service.PostInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }

            return Task.FromResult(result);
        }

        public Task<ReturnObject<bool>> PutInfo(ResultGetPlantInfoViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var res = _plantRepository.Update_Info(param, _userInfo).Result;

                result.data = res.is_success;
                result.isCompleted = res.is_success;
                result.message.Add(res.message);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Plant.Service.PutInfo}");
                _logger.WriteError(errorCode: ErrorCodes.Plant.Service.PutInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }

            return Task.FromResult(result);
        }

        public Task<ReturnObject<bool>> DeleteInfo(ResultGetPlantInfoViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var res = _plantRepository.Delete_Info(param, _userInfo).Result;

                result.data = res.is_success;
                result.isCompleted = res.is_success;
                result.message.Add(res.message);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Plant.Service.DeleteInfo}");
                _logger.WriteError(errorCode: ErrorCodes.Plant.Service.DeleteInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }

            return Task.FromResult(result);
        }

        public Task<ReturnList<ResultGetPlantViewModel>> GetListPlant()
        {
            var result = new ReturnList<ResultGetPlantViewModel>();
            try
            {
                var lsData = _plantRepository.Select_Plant_All(_userInfo).Result;

                result.data = _initPlantListData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Plant.Service.GetListPlant}");
                _logger.WriteError(errorCode: ErrorCodes.Plant.Service.GetListPlant, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }

            return Task.FromResult(result);
        }

        public Task<ReturnList<ResultSearchPlantViewModel>> GetSearchListPlant(ParamSearchPlantViewModel param)
        {
            var result = new ReturnList<ResultSearchPlantViewModel>();
            try
            {
                var lsData = _plantRepository.Select_Plant_By(param, _userInfo).Result;

                result.data = _initSearchPlantListData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Plant.Service.GetSearchListPlant}");
                _logger.WriteError(errorCode: ErrorCodes.Plant.Service.GetSearchListPlant, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }

            return Task.FromResult(result);
        }

        private List<ResultGetPlantViewModel> _initPlantListData(List<PlantData> lsPlant)
        {
            List<ResultGetPlantViewModel> result = new List<ResultGetPlantViewModel>();

            foreach (PlantData pt in lsPlant)
            {
                result.Add(new ResultGetPlantViewModel
                {
                    plant_code = pt.plant_code,
                    comp_code = pt.comp_code,
                    short_code = pt.short_code,
                    province_code = pt.province_code,
                    name_th = pt.name_th,
                    name_en = pt.name_en,
                    addr_en_line1 = pt.addr_en_line1,
                    addr_en_line2 = pt.addr_en_line2,
                    addr_th_line1 = pt.addr_th_line1,
                    addr_th_line2 = pt.addr_th_line2,
                    head_report = pt.head_report,
                    report_type = pt.report_type,
                    created_by = pt.created_by,
                    created_date = pt.created_date,
                    modified_by = pt.modified_by,
                    modified_date = pt.modified_date,
                    is_active = pt.is_active,
                    is_deleted = pt.is_deleted
                });
            }

            return result;
        }

        private List<ResultSearchPlantViewModel> _initSearchPlantListData(List<PlantData> lsPlant)
        {
            List<ResultSearchPlantViewModel> result = new List<ResultSearchPlantViewModel>();

            foreach (PlantData pt in lsPlant)
            {
                result.Add(new ResultSearchPlantViewModel
                {
                    plant_code = pt.plant_code,
                    comp_code = pt.comp_code,
                    short_code = pt.short_code,
                    province_code = pt.province_code,
                    name_th = pt.name_th,
                    name_en = pt.name_en,
                    addr_en_line1 = pt.addr_en_line1,
                    addr_en_line2 = pt.addr_en_line2,
                    addr_th_line1 = pt.addr_th_line1,
                    addr_th_line2 = pt.addr_th_line2,
                    head_report = pt.head_report,
                    created_by = pt.created_by,
                    created_date = pt.created_date,
                    modified_by = pt.modified_by,
                    modified_date = pt.modified_date,
                    is_active = pt.is_active,
                    is_deleted = pt.is_deleted,
                    total_record = pt.total_record,
                });
            }

            return result;
        }
    }
}
