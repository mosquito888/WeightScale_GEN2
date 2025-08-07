using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository;
using WeightScaleGen2.BGC.API.Common;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.WeightInHistory;
using ILogger = WeightScaleGen2.BGC.API.Common.Logger.ILogger;

namespace WeightScaleGen2.BGC.API.APIServices
{
    public class WeightInHistoryAPIService : BaseAPIService
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;
        ILogger _logger;
        private readonly IHttpContextAccessor _context;
        private readonly UserInfoModel _userInfo;
        private WeightInHistoryRepository _weightInHistoryRepository;

        public WeightInHistoryAPIService(
            IDatabaseConnectionFactory db,
            ISecurityCommon securityCommon,
            ILogger logger,
            IHttpContextAccessor context,
            UserInfoModel userInfo,
            WeightInHistoryRepository weightInHistoryRepository) : base(db, securityCommon)
        {
            _db = db;
            _securityCommon = securityCommon;
            _logger = logger;
            _context = context;
            _userInfo = _getUserInfo(_context.HttpContext.Session.GetString(Constants.Session.User)).Result;
            _weightInHistoryRepository = weightInHistoryRepository;
        }

        public Task<ReturnList<ResultSearchWeightInHistoryViewModel>> GetListWeightInHistory()
        {
            var result = new ReturnList<ResultSearchWeightInHistoryViewModel>();
            try
            {
                var lsData = _weightInHistoryRepository.Select_SearchWeightInHistoryListData(_userInfo).Result;

                result.data = _initSearchWeightInHistoryListData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.WeightInHistory.Service.GetListWeightInHistory}");
                _logger.WriteError(errorCode: ErrorCodes.WeightInHistory.Service.GetListWeightInHistory, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnList<ResultSearchWeightInHistoryViewModel>> GetSearchListWeightInHistory(ParamSearchWeightInHistoryViewModel param)
        {
            var result = new ReturnList<ResultSearchWeightInHistoryViewModel>();
            try
            {
                var lsData = _weightInHistoryRepository.Select_SearchWeightInHistoryListData_By(param, _userInfo).Result;

                result.data = _initSearchWeightInHistoryListData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.WeightInHistory.Service.GetSearchListWeightInHistory}");
                _logger.WriteError(errorCode: ErrorCodes.WeightInHistory.Service.GetSearchListWeightInHistory, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        private List<ResultSearchWeightInHistoryViewModel> _initSearchWeightInHistoryListData(List<WeightInHistoryData> lsWO)
        {
            List<ResultSearchWeightInHistoryViewModel> result = new List<ResultSearchWeightInHistoryViewModel>();

            foreach (WeightInHistoryData objWO in lsWO)
            {
                result.Add(new ResultSearchWeightInHistoryViewModel
                {
                    id = objWO.id,
                    weight_in_no = objWO.weight_in_no,
                    weight_in_type = objWO.weight_in_type,
                    line_number = objWO.line_number,
                    item_code = objWO.item_code,
                    item_name = objWO.item_name,
                    supplier_code = objWO.supplier_code,
                    car_license = objWO.car_license,
                    car_type = objWO.car_type,
                    document_po = objWO.document_po,
                    doc_type_po = objWO.doc_type_po,
                    document_ref = objWO.document_ref,
                    weight_in = objWO.weight_in,
                    date = objWO.date,
                    user_id = objWO.user_id,
                    status = objWO.status,
                    user_edit_1 = objWO.user_edit_1,
                    user_edit_2 = objWO.user_edit_2,
                    user_edit_3 = objWO.user_edit_3,
                    remark_1 = objWO.remark_1,
                    remark_2 = objWO.remark_2,
                    reprint = objWO.reprint,
                    company = objWO.company,
                    maintenance_no = objWO.maintenance_no,
                    doc_start = objWO.doc_start,
                    doc_stop = objWO.doc_stop,
                    doc_send = objWO.doc_send,
                    edi = objWO.edi,
                    edi_sand = objWO.edi_sand,
                    sender_id = objWO.sender_id,
                });
            }

            return result;
        }

    }
}
