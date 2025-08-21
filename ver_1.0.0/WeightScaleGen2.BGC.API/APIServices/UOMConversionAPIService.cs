using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository;
using WeightScaleGen2.BGC.API.Common;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.UOMConversion;
using ILogger = WeightScaleGen2.BGC.API.Common.Logger.ILogger;

namespace WeightScaleGen2.BGC.API.APIServices
{
    public class UOMConversionAPIService : BaseAPIService
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;
        ILogger _logger;
        private readonly IHttpContextAccessor _context;
        private readonly UserInfoModel _userInfo;
        private UOMConversionRepository _uomConversionRepository;

        public UOMConversionAPIService(
            IDatabaseConnectionFactory db,
            ISecurityCommon securityCommon,
            ILogger logger,
            IHttpContextAccessor context,
            UserInfoModel userInfo,
            UOMConversionRepository uomConversionRepository) : base(db, securityCommon)
        {
            _db = db;
            _securityCommon = securityCommon;
            _logger = logger;
            _context = context;
            _userInfo = _getUserInfo(_context.HttpContext.Session.GetString(Constants.Session.User)).Result;
            _uomConversionRepository = uomConversionRepository;
        }

        public Task<ReturnList<ResultSearchUOMConversionViewModel>> GetUOMConversionListData()
        {
            var result = new ReturnList<ResultSearchUOMConversionViewModel>();
            try
            {
                var lsData = _uomConversionRepository.Select_SearchUOMConversionListData_All(_userInfo).Result;

                result.data = _initSearchUOMConversionListData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.UOMConversion.Service.GetListUOMConversion}");
                _logger.WriteError(errorCode: ErrorCodes.UOMConversion.Service.GetListUOMConversion, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnList<ResultSearchUOMConversionViewModel>> GetUOMConversionList_By(ParamSearchUOMConversionViewModel param)
        {
            var result = new ReturnList<ResultSearchUOMConversionViewModel>();
            try
            {
                var lsData = _uomConversionRepository.Select_SearchUOMConversionListData_By(param, _userInfo).Result;

                result.data = _initSearchUOMConversionListData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.UOMConversion.Service.GetListUOMConversion_By}");
                _logger.WriteError(errorCode: ErrorCodes.UOMConversion.Service.GetListUOMConversion_By, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnList<ResultSearchUOMConversionViewModel>> GetUOMConversionListByMaterialCode(string materialCode)
        {
            var result = new ReturnList<ResultSearchUOMConversionViewModel>();
            try
            {
                var lsData = _uomConversionRepository.Select_SearchUOMConversionListData_By_MaterialCode(materialCode, _userInfo).Result;

                result.data = _initSearchUOMConversionListData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.UOMConversion.Service.GetListUOMConversion_By_MaterialCode}");
                _logger.WriteError(errorCode: ErrorCodes.UOMConversion.Service.GetListUOMConversion_By_MaterialCode, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        private List<ResultSearchUOMConversionViewModel> _initSearchUOMConversionListData(List<UOMConversionData> lsUOM)
        {
            List<ResultSearchUOMConversionViewModel> result = new List<ResultSearchUOMConversionViewModel>();

            foreach (UOMConversionData objUOM in lsUOM)
            {
                result.Add(new ResultSearchUOMConversionViewModel
                {
                    material_code = objUOM.material_code,
                    alter_uom = objUOM.alter_uom,
                    base_uom = objUOM.base_uom,
                    alter_uom_in = objUOM.alter_uom_in,
                    base_uom_in = objUOM.base_uom_in,
                    conv_weight_n = objUOM.conv_weight_n,
                    conv_weight_d = objUOM.conv_weight_d,
                    net_weight = objUOM.net_weight,
                    gross_weight = objUOM.gross_weight,
                    weight_unit = objUOM.weight_unit,
                    created_by = objUOM.created_by,
                    created_on = objUOM.created_on,
                    created_time = objUOM.created_time,
                    updated_by = objUOM.updated_by,
                    updated_on = objUOM.updated_on,
                    updated_time = objUOM.updated_time,
                    total_record = objUOM.total_record
                });
            }

            return result;
        }

    }
}
