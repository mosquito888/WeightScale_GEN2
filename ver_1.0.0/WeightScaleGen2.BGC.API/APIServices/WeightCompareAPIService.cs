using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository;
using WeightScaleGen2.BGC.API.Common;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.WeightCompare;
using ILogger = WeightScaleGen2.BGC.API.Common.Logger.ILogger;

namespace WeightScaleGen2.BGC.API.APIServices
{
    public class WeightCompareAPIService : BaseAPIService
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;
        ILogger _logger;
        private readonly IHttpContextAccessor _context;
        private readonly UserInfoModel _userInfo;
        private WeightCompareRepository _weightCompareRepository;

        public WeightCompareAPIService(
            IDatabaseConnectionFactory db,
            ISecurityCommon securityCommon,
            ILogger logger,
            IHttpContextAccessor context,
            UserInfoModel userInfo,
            WeightCompareRepository weightCompareRepository) : base(db, securityCommon)
        {
            _db = db;
            _securityCommon = securityCommon;
            _logger = logger;
            _context = context;
            _userInfo = _getUserInfo(_context.HttpContext.Session.GetString(Constants.Session.User)).Result;
            _weightCompareRepository = weightCompareRepository;
        }

        public Task<ReturnList<ResultSearchWeightCompareViewModel>> GetSearchListWeightCompare(ParamSearchWeightCompareViewModel param)
        {
            var result = new ReturnList<ResultSearchWeightCompareViewModel>();
            try
            {
                var lsData = _weightCompareRepository.Select_SearchWeightCompareListData_By(param, _userInfo).Result;

                result.data = _initSearchWeightCompareListData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.WeightCompare.Service.GetSearchListWeightCompare}");
                _logger.WriteError(errorCode: ErrorCodes.WeightCompare.Service.GetSearchListWeightCompare, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        private List<ResultSearchWeightCompareViewModel> _initSearchWeightCompareListData(List<WeightCompareData> lsWC)
        {
            List<ResultSearchWeightCompareViewModel> result = new List<ResultSearchWeightCompareViewModel>();

            foreach (WeightCompareData objWC in lsWC)
            {
                result.Add(new ResultSearchWeightCompareViewModel
                {
                    date = objWC.date,
                    car_license = objWC.car_license,
                    document_ref = objWC.document_ref,
                    weight_out = objWC.weight_out,
                    weight_cal = objWC.weight_cal,
                    weight_receive = objWC.weight_receive,
                    weight_by_supplier = objWC.weight_by_supplier,
                    weight_diff = objWC.weight_diff,
                    weight_percent = objWC.weight_percent,
                    remark_1 = objWC.remark_1
                });
            }

            return result;
        }

    }
}
