using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository;
using WeightScaleGen2.BGC.API.Common;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.WeightHistory;
using ILogger = WeightScaleGen2.BGC.API.Common.Logger.ILogger;

namespace WeightScaleGen2.BGC.API.APIServices
{
    public class WeightHistoryAPIService : BaseAPIService
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;
        ILogger _logger;
        private readonly IHttpContextAccessor _context;
        private readonly UserInfoModel _userInfo;
        private WeightHistoryRepository _weightHistoryRepository;

        public WeightHistoryAPIService(
            IDatabaseConnectionFactory db,
            ISecurityCommon securityCommon,
            ILogger logger,
            IHttpContextAccessor context,
            UserInfoModel userInfo,
            WeightHistoryRepository weightHistoryRepository) : base(db, securityCommon)
        {
            _db = db;
            _securityCommon = securityCommon;
            _logger = logger;
            _context = context;
            _userInfo = _getUserInfo(_context.HttpContext.Session.GetString(Constants.Session.User)).Result;
            _weightHistoryRepository = weightHistoryRepository;
        }

        //public Task<ReturnList<ResultSearchItemMasterViewModel>> GetListItemMaster()
        //{
        //    var result = new ReturnList<ResultSearchItemMasterViewModel>();
        //    try
        //    {
        //        var lsData = _itemMasterRepository.Select_ItemMasterListData_All(_userInfo).Result;

        //        result.data = _initSearchItemMasterListData(lsData);
        //        result.isCompleted = true;
        //        result.message.Add(Constants.Result.Success);
        //    }
        //    catch (Exception ex)
        //    {
        //        result.isCompleted = false;
        //        result.message.Add($"Error Code: {ErrorCodes.ItemMaster.Service.GetListItemMaster}");
        //        _logger.WriteError(errorCode: ErrorCodes.ItemMaster.Service.GetListItemMaster, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
        //    }
        //    return Task.FromResult(result);
        //}

        public Task<ReturnList<ResultSearchWeightHistoryViewModel>> GetSearchListWeightHistory(ParamSearchWeightHistoryViewModel param)
        {
            var result = new ReturnList<ResultSearchWeightHistoryViewModel>();
            try
            {
                var lsData = _weightHistoryRepository.Select_SearchWeightHistoryListData_By(param, _userInfo).Result;

                result.data = _initSearchWeightHistoryListData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.WeightHistory.Service.GetSearchListWeightHistory}");
                _logger.WriteError(errorCode: ErrorCodes.WeightHistory.Service.GetSearchListWeightHistory, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnObject<ResultGetWeightHistoryInfoViewModel>> GetWeightHistoryInfo(ParamWeightHistoryInfo param)
        {
            var result = new ReturnObject<ResultGetWeightHistoryInfoViewModel>();
            try
            {
                var lsData = _weightHistoryRepository.Select_WeightHistoryInfo(param, _userInfo).Result;

                result.data = _initWeightHistoryInfo(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.WeightHistory.Service.GetWeightHistoryInfo}");
                _logger.WriteError(errorCode: ErrorCodes.WeightHistory.Service.GetWeightHistoryInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        private List<ResultSearchWeightHistoryViewModel> _initSearchWeightHistoryListData(List<WeightHistoryData> lsWH)
        {
            List<ResultSearchWeightHistoryViewModel> result = new List<ResultSearchWeightHistoryViewModel>();

            foreach (WeightHistoryData objWH in lsWH)
            {
                result.Add(new ResultSearchWeightHistoryViewModel
                {
                    id = objWH.id,
                    weight_out_no = objWH.weight_out_no,
                    weight_in_no = objWH.weight_in_no,
                    before_weight_out = objWH.before_weight_out,
                    weight_in = objWH.weight_in,
                    weight_receive = objWH.weight_receive,
                    supplier_code = objWH.supplier_code,
                    supplier_name = objWH.supplier_name,
                    item_code = objWH.item_code,
                    item_name = objWH.item_name,
                    item_remark = objWH.item_remark,
                    weight_out_type = objWH.weight_out_type,
                    car_license = objWH.car_license,
                    weight_out_date = objWH.weight_out_date,
                    company = objWH.company,
                    user_edit_1 = objWH.user_edit_1,
                    user_edit_2 = objWH.user_edit_2,
                    user_edit_3 = objWH.user_edit_3,
                    sg_bg = objWH.sg_bg,
                    sg_supplier = objWH.sg_supplier,
                    api_bg = objWH.api_bg,
                    api_supplier = objWH.api_supplier,
                    temp_bg = objWH.temp_bg,
                    temp_supplier = objWH.temp_supplier,
                    remark_1 = objWH.remark_1,
                    remark_2 = objWH.remark_2,
                    document_def = objWH.document_def,
                    weight_in_date = objWH.weight_in_date,
                    user_id = objWH.user_id,
                    document_po = objWH.document_po,
                    doc_type_po = objWH.doc_type_po,
                    weight_by_supplier = objWH.weight_by_supplier,
                    total_record = objWH.total_record
                });
            }

            return result;
        }

        private ResultGetWeightHistoryInfoViewModel _initWeightHistoryInfo(WeightHistoryData weightHisInfo)
        {
            ResultGetWeightHistoryInfoViewModel result = new ResultGetWeightHistoryInfoViewModel();
            if (weightHisInfo != null)
            {
                result.id = weightHisInfo.id;
                result.weight_out_no = weightHisInfo.weight_out_no;
                result.weight_in_no = weightHisInfo.weight_in_no;
                result.before_weight_out = weightHisInfo.before_weight_out;
                result.weight_in = weightHisInfo.weight_in;
                result.weight_receive = weightHisInfo.weight_receive;
                result.supplier_code = weightHisInfo.supplier_code;
                result.supplier_name = weightHisInfo.supplier_name;
                result.item_code = weightHisInfo.item_code;
                result.item_name = weightHisInfo.item_name;
                result.item_remark = weightHisInfo.item_remark;
                result.weight_out_type = weightHisInfo.weight_out_type;
                result.car_license = weightHisInfo.car_license;
                result.weight_out_date = weightHisInfo.weight_out_date;
                result.company = weightHisInfo.company;
                result.user_edit_1 = weightHisInfo.user_edit_1;
                result.user_edit_2 = weightHisInfo.user_edit_2;
                result.user_edit_3 = weightHisInfo.user_edit_3;
                result.sg_bg = weightHisInfo.sg_bg;
                result.sg_supplier = weightHisInfo.sg_supplier;
                result.api_bg = weightHisInfo.api_bg;
                result.api_supplier = weightHisInfo.api_supplier;
                result.temp_bg = weightHisInfo.temp_bg;
                result.temp_supplier = weightHisInfo.temp_supplier;
                result.remark_1 = weightHisInfo.remark_1;
                result.remark_2 = weightHisInfo.remark_2;
                result.document_def = weightHisInfo.document_def;
                result.weight_in_date = weightHisInfo.weight_in_date;
                result.user_id = weightHisInfo.user_id;
                result.document_po = weightHisInfo.document_po;
                result.doc_type_po = weightHisInfo.doc_type_po;
                result.weight_by_supplier = weightHisInfo.weight_by_supplier;
            }

            return result;
        }

    }
}
