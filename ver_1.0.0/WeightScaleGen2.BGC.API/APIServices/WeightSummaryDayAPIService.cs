using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository;
using WeightScaleGen2.BGC.API.Common;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.WeightSummaryDay;
using ILogger = WeightScaleGen2.BGC.API.Common.Logger.ILogger;

namespace WeightScaleGen2.BGC.API.APIServices
{
    public class WeightSummaryDayAPIService : BaseAPIService
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;
        ILogger _logger;
        private readonly IHttpContextAccessor _context;
        private readonly UserInfoModel _userInfo;
        private WeightSummaryDayRepository _weightSummaryDayRepository;

        public WeightSummaryDayAPIService(
            IDatabaseConnectionFactory db,
            ISecurityCommon securityCommon,
            ILogger logger,
            IHttpContextAccessor context,
            UserInfoModel userInfo,
            WeightSummaryDayRepository weightSummaryDayRepository) : base(db, securityCommon)
        {
            _db = db;
            _securityCommon = securityCommon;
            _logger = logger;
            _context = context;
            _userInfo = _getUserInfo(_context.HttpContext.Session.GetString(Constants.Session.User)).Result;
            _weightSummaryDayRepository = weightSummaryDayRepository;
        }

        public Task<ReturnList<ResultSearchWeightSummaryDayViewModel>> GetSearchListWeightSummaryDay(ParamSearchWeightSummaryDayViewModel param)
        {
            var result = new ReturnList<ResultSearchWeightSummaryDayViewModel>();
            try
            {
                var lsData = _weightSummaryDayRepository.Select_SearchWeightSummaryDayListData_By(param, _userInfo).Result;

                result.data = _initSearchWeightSummaryDayListData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.WeightSummaryDay.Service.GetSearchListWeightSummaryDay}");
                _logger.WriteError(errorCode: ErrorCodes.WeightSummaryDay.Service.GetSearchListWeightSummaryDay, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        private List<ResultSearchWeightSummaryDayViewModel> _initSearchWeightSummaryDayListData(List<WeightSummaryDayData> lsWSD)
        {
            List<ResultSearchWeightSummaryDayViewModel> result = new List<ResultSearchWeightSummaryDayViewModel>();

            foreach (WeightSummaryDayData objWSD in lsWSD)
            {
                result.Add(new ResultSearchWeightSummaryDayViewModel
                {
                    item_code = objWSD.item_code,
                    item_name = objWSD.item_name,
                    supplier_code = objWSD.supplier_code,
                    supplier_name = objWSD.supplier_name,
                    weight_count = objWSD.weight_count,
                    sum_weight_out = objWSD.sum_weight_out,
                    group_name = objWSD.group_name,
                });
            }

            return result;
        }

    }
}
