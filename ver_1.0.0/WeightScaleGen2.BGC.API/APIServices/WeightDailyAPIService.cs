using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository;
using WeightScaleGen2.BGC.API.Common;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.WeightDaily;
using ILogger = WeightScaleGen2.BGC.API.Common.Logger.ILogger;

namespace WeightScaleGen2.BGC.API.APIServices
{
    public class WeightDailyAPIService : BaseAPIService
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;
        ILogger _logger;
        private readonly IHttpContextAccessor _context;
        private readonly UserInfoModel _userInfo;
        private WeightDailyRepository _weightDailyRepository;

        public WeightDailyAPIService(
            IDatabaseConnectionFactory db,
            ISecurityCommon securityCommon,
            ILogger logger,
            IHttpContextAccessor context,
            UserInfoModel userInfo,
            WeightDailyRepository weightDailyRepository) : base(db, securityCommon)
        {
            _db = db;
            _securityCommon = securityCommon;
            _logger = logger;
            _context = context;
            _userInfo = _getUserInfo(_context.HttpContext.Session.GetString(Constants.Session.User)).Result;
            _weightDailyRepository = weightDailyRepository;
        }

        public Task<ReturnList<ResultSearchWeightDailyViewModel>> GetSearchListWeightDaily(ParamSearchWeightDailyViewModel param)
        {
            var result = new ReturnList<ResultSearchWeightDailyViewModel>();
            try
            {
                var lsData = _weightDailyRepository.Select_SearchWeightDailyListData_By(param, _userInfo).Result;

                result.data = _initSearchWeightDailyListData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.WeightDaily.Service.GetSearchListWeightDaily}");
                _logger.WriteError(errorCode: ErrorCodes.WeightDaily.Service.GetSearchListWeightDaily, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        private List<ResultSearchWeightDailyViewModel> _initSearchWeightDailyListData(List<WeightDailyData> lsWD)
        {
            List<ResultSearchWeightDailyViewModel> result = new List<ResultSearchWeightDailyViewModel>();

            foreach (WeightDailyData objWD in lsWD)
            {
                result.Add(new ResultSearchWeightDailyViewModel
                {
                    weight_in_no = objWD.weight_in_no,
                    car_license = objWD.car_license,
                    weight_in_date = objWD.weight_in_date,
                    weight_in = objWD.weight_in,
                    weight_out_date = objWD.weight_out_date,
                    before_weight_out = objWD.before_weight_out,
                    weight_receive = objWD.weight_receive,
                    user_id = objWD.user_id,
                    document_ref = objWD.document_ref,
                    weight_out_no = objWD.weight_out_no,
                    status = objWD.status,
                    item_code = objWD.item_code,
                    item_name = objWD.item_name,
                    supplier_code = objWD.supplier_code,
                    supplier_name = objWD.supplier_name,
                    weight_diff = objWD.weight_diff,
                    remark_1 = objWD.remark_1,
                    total_record = objWD.total_record
                });
            }

            return result;
        }

    }
}
