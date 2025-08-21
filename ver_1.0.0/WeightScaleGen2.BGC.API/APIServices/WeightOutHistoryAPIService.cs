using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository;
using WeightScaleGen2.BGC.API.Common;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.WeightOutHistory;
using ILogger = WeightScaleGen2.BGC.API.Common.Logger.ILogger;

namespace WeightScaleGen2.BGC.API.APIServices
{
    public class WeightOutHistoryAPIService : BaseAPIService
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;
        ILogger _logger;
        private readonly IHttpContextAccessor _context;
        private readonly UserInfoModel _userInfo;
        private WeightOutHistoryRepository _weightOutHistoryRepository;

        public WeightOutHistoryAPIService(
            IDatabaseConnectionFactory db,
            ISecurityCommon securityCommon,
            ILogger logger,
            IHttpContextAccessor context,
            UserInfoModel userInfo,
            WeightOutHistoryRepository weightOutHistoryRepository) : base(db, securityCommon)
        {
            _db = db;
            _securityCommon = securityCommon;
            _logger = logger;
            _context = context;
            _userInfo = _getUserInfo(_context.HttpContext.Session.GetString(Constants.Session.User)).Result;
            _weightOutHistoryRepository = weightOutHistoryRepository;
        }

        public Task<ReturnList<ResultSearchWeightOutHistoryViewModel>> GetListWeightOutHistory()
        {
            var result = new ReturnList<ResultSearchWeightOutHistoryViewModel>();
            try
            {
                var lsData = _weightOutHistoryRepository.Select_SearchWeightOutHistoryListData(_userInfo).Result;

                result.data = _initSearchWeightOutHistoryListData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.WeightOutHistory.Service.GetListWeightOutHistory}");
                _logger.WriteError(errorCode: ErrorCodes.WeightOutHistory.Service.GetListWeightOutHistory, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnList<ResultSearchWeightOutHistoryViewModel>> GetSearchListWeightOutHistory(ParamSearchWeightOutHistoryViewModel param)
        {
            var result = new ReturnList<ResultSearchWeightOutHistoryViewModel>();
            try
            {
                var lsData = _weightOutHistoryRepository.Select_SearchWeightOutHistoryListData_By(param, _userInfo).Result;

                result.data = _initSearchWeightOutHistoryListData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.WeightOutHistory.Service.GetSearchListWeightOutHistory}");
                _logger.WriteError(errorCode: ErrorCodes.WeightOutHistory.Service.GetSearchListWeightOutHistory, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        private List<ResultSearchWeightOutHistoryViewModel> _initSearchWeightOutHistoryListData(List<WeightOutHistoryData> lsWO)
        {
            List<ResultSearchWeightOutHistoryViewModel> result = new List<ResultSearchWeightOutHistoryViewModel>();

            foreach (WeightOutHistoryData objWO in lsWO)
            {
                result.Add(new ResultSearchWeightOutHistoryViewModel
                {
                    id = objWO.id,
                    weight_out_no = objWO.weight_out_no,
                    weight_out_type = objWO.weight_out_type,
                    car_license = objWO.car_license,
                    weight_in_no = objWO.weight_in_no,
                    base_unit = objWO.base_unit,
                    unit_receive = objWO.unit_receive,
                    gross_uom = objWO.gross_uom,
                    net_uom = objWO.net_uom,
                    status = objWO.status,
                    date = objWO.date,
                    before_weight_out = objWO.before_weight_out,
                    weight_out = objWO.weight_out,
                    weight_receive = objWO.weight_receive,
                    percent_humidity_out = objWO.percent_humidity_out,
                    percent_humidity_ok = objWO.percent_humidity_ok,
                    percent_humidity_diff = objWO.percent_humidity_diff,
                    weight_bag = objWO.weight_bag,
                    qty_bag = objWO.qty_bag,
                    total_weight_bag = objWO.total_weight_bag,
                    weight_pallet = objWO.weight_pallet,
                    qty_pallet = objWO.qty_pallet,
                    total_weight_pallet = objWO.total_weight_pallet,
                    weight_by_supplier = objWO.weight_by_supplier,
                    volume_by_supplier = objWO.volume_by_supplier,
                    sg_supplier = objWO.sg_supplier,
                    sg_bg = objWO.sg_bg,
                    api_supplier = objWO.api_supplier,
                    api_bg = objWO.api_bg,
                    temp_supplier = objWO.temp_supplier,
                    temp_bg = objWO.temp_bg,
                    remark_1 = objWO.remark_1,
                    remark_2 = objWO.remark_2,
                    user_edit_1 = objWO.user_edit_1,
                    user_edit_2 = objWO.user_edit_2,
                    user_edit_3 = objWO.user_edit_3,
                    reprint = objWO.reprint,
                    company = objWO.company,
                    maintenance_no = objWO.maintenance_no,
                    edi = objWO.edi,
                    edi_send = objWO.edi_send,
                });
            }

            return result;
        }

    }
}
