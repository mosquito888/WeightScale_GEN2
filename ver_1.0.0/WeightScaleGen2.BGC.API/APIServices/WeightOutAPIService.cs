using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository;
using WeightScaleGen2.BGC.API.Common;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.IdentNumber;
using WeightScaleGen2.BGC.Models.ViewModels.WeightIn;
using WeightScaleGen2.BGC.Models.ViewModels.WeightOut;
using ILogger = WeightScaleGen2.BGC.API.Common.Logger.ILogger;

namespace WeightScaleGen2.BGC.API.APIServices
{
    public class WeightOutAPIService : BaseAPIService
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;
        ILogger _logger;
        private readonly IHttpContextAccessor _context;
        private readonly UserInfoModel _userInfo;
        private WeightOutRepository _weightOutRepository;
        private WeightInRepository _weightInRepository;
        private IdentNumberRepository _identNumberRepository;

        public WeightOutAPIService(
            IDatabaseConnectionFactory db,
            ISecurityCommon securityCommon,
            ILogger logger,
            IHttpContextAccessor context,
            UserInfoModel userInfo,
            WeightOutRepository weightOutRepository,
            WeightInRepository weightInRepository,
            IdentNumberRepository identNumberRepository) : base(db, securityCommon)
        {
            _db = db;
            _securityCommon = securityCommon;
            _logger = logger;
            _context = context;
            _userInfo = _getUserInfo(_context.HttpContext.Session.GetString(Constants.Session.User)).Result;
            _weightOutRepository = weightOutRepository;
            _weightInRepository = weightInRepository;
            _identNumberRepository = identNumberRepository;
        }

        public Task<ReturnObject<int>> PostWeightOutInfo(ResultGetWeightOutInfoViewModel param)
        {
            var result = new ReturnObject<int>();
            try
            {
                // Get Company Code
                param.company = _userInfo.comp_code;

                // Create Weight Out no
                string weightOutNo = _identNumberRepository.Select_IdentNumber(new ParamGetIdentNumberViewModel { type = "O", company = _userInfo.comp_code }, _userInfo).Result;
                param.weight_out_no = weightOutNo;


                var res = _weightOutRepository.Insert_WeightOutInfo(param, _userInfo).Result;

                // Update Weight In Status
                _ = _weightInRepository.Update_WeightInStatus(new ResultGetWeightInInfoViewModel { weight_in_no = param.weight_in_no, status = "Complete" }, _userInfo).Result;

                var id = _weightOutRepository.Select_SearchWeightOutListData_By(new ParamSearchWeightOutViewModel { weight_out_no = weightOutNo, start = 0, draw = int.MaxValue, length = int.MaxValue }, _userInfo).Result.FirstOrDefault();

                result.data = id != null ? id.id : 0;
                result.isCompleted = res.is_success;
                result.message.Add(res.message);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.WeightIn.Service.PostWeightInInfo}");
                _logger.WriteError(errorCode: ErrorCodes.WeightIn.Service.PostWeightInInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnObject<bool>> PutWeightOutInfo(ResultGetWeightOutInfoViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var res = _weightOutRepository.Update_WeightOutInfo(param, _userInfo).Result;

                result.data = res.is_success;
                result.isCompleted = res.is_success;
                result.message.Add(res.message);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.WeightOut.Service.PutWeightOutInfo}");
                _logger.WriteError(errorCode: ErrorCodes.WeightOut.Service.PutWeightOutInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnObject<bool>> PutWeightOutStatus(ResultGetWeightOutInfoViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var data = _weightOutRepository.Select_WeightOutInfo(new ParamWeightOutInfo { id = param.id }, _userInfo).Result;
                if (data != null && data.status == "Deactive")
                {
                    result.isCompleted = false;
                    result.message.Add("รายการชั่งออกนี้ได้ถูกยกเลิกไปเรียบร้อยแล้ว");
                    return Task.FromResult(result);
                }

                var res = _weightOutRepository.Update_WeightOutStatus(param, _userInfo).Result;
                _ = _weightInRepository.Update_WeightInStatus(new ResultGetWeightInInfoViewModel { weight_in_no = param.weight_in_no, status = "Active" }, _userInfo).Result;

                result.data = res.is_success;
                result.isCompleted = res.is_success;
                result.message.Add(res.message);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.WeightOut.Service.PutWeightOutInfo}");
                _logger.WriteError(errorCode: ErrorCodes.WeightOut.Service.PutWeightOutInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnList<ResultSearchWeightOutViewModel>> GetSearchListWeightOut(ParamSearchWeightOutViewModel param)
        {
            var result = new ReturnList<ResultSearchWeightOutViewModel>();
            try
            {
                var lsData = _weightOutRepository.Select_SearchWeightOutListData_By(param, _userInfo).Result;

                result.data = _initSearchWeightOutListData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.WeightOut.Service.GetSearchListWeightOut}");
                _logger.WriteError(errorCode: ErrorCodes.WeightOut.Service.GetSearchListWeightOut, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnObject<ResultGetWeightOutInfoViewModel>> GetWeightOutInfo(ParamWeightOutInfo param)
        {
            var result = new ReturnObject<ResultGetWeightOutInfoViewModel>();
            try
            {
                var lsData = _weightOutRepository.Select_WeightOutInfo(param, _userInfo).Result;

                result.data = _initWeightOutInfo(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.WeightOut.Service.GetWeightOutInfo}");
                _logger.WriteError(errorCode: ErrorCodes.WeightOut.Service.GetWeightOutInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnObject<ResultGetWeightOutInfoViewModel>> GetWeightOutInfoByCarLicense(ParamWeightOutInfo param)
        {
            var result = new ReturnObject<ResultGetWeightOutInfoViewModel>();
            try
            {
                var lsData = _weightOutRepository.Select_WeightOutInfoByCarLicense(param, _userInfo).Result;

                result.data = _initWeightOutInfo(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.WeightOut.Service.GetWeightOutInfoByCarLicense}");
                _logger.WriteError(errorCode: ErrorCodes.WeightOut.Service.GetWeightOutInfoByCarLicense, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        private List<ResultSearchWeightOutViewModel> _initSearchWeightOutListData(List<WeightOutData> lsWO)
        {
            List<ResultSearchWeightOutViewModel> result = new List<ResultSearchWeightOutViewModel>();

            foreach (WeightOutData objWO in lsWO)
            {
                result.Add(new ResultSearchWeightOutViewModel
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
                    total_record = objWO.total_record
                });
            }

            return result;
        }

        private ResultGetWeightOutInfoViewModel _initWeightOutInfo(WeightOutData weightOutInfo)
        {
            ResultGetWeightOutInfoViewModel result = new ResultGetWeightOutInfoViewModel();
            if (weightOutInfo != null)
            {
                result.id = weightOutInfo.id;
                result.weight_out_no = weightOutInfo.weight_out_no;
                result.weight_out_type = weightOutInfo.weight_out_type;
                result.car_license = weightOutInfo.car_license;
                result.weight_in_no = weightOutInfo.weight_in_no;
                result.base_unit = weightOutInfo.base_unit;
                result.unit_receive = weightOutInfo.unit_receive;
                result.gross_uom = weightOutInfo.gross_uom;
                result.net_uom = weightOutInfo.net_uom;
                result.status = weightOutInfo.status;
                result.date = weightOutInfo.date;
                result.before_weight_out = weightOutInfo.before_weight_out;
                result.weight_product = weightOutInfo.weight_in - weightOutInfo.weight_out;
                result.weight_out = weightOutInfo.weight_out;
                result.weight_receive = weightOutInfo.weight_receive;
                result.percent_humidity_out = weightOutInfo.percent_humidity_out;
                result.percent_humidity_ok = weightOutInfo.percent_humidity_ok;
                result.percent_humidity_diff = weightOutInfo.percent_humidity_diff;
                result.weight_bag = weightOutInfo.weight_bag;
                result.qty_bag = weightOutInfo.qty_bag;
                result.total_weight_bag = weightOutInfo.total_weight_bag;
                result.weight_pallet = weightOutInfo.weight_pallet;
                result.qty_pallet = weightOutInfo.qty_pallet;
                result.total_weight_pallet = weightOutInfo.total_weight_pallet;
                result.weight_by_supplier = weightOutInfo.weight_by_supplier;
                result.volume_by_supplier = weightOutInfo.volume_by_supplier;
                result.sg_supplier = weightOutInfo.sg_supplier;
                result.sg_bg = weightOutInfo.sg_bg;
                result.api_supplier = weightOutInfo.api_supplier;
                result.api_bg = weightOutInfo.api_bg;
                result.temp_supplier = weightOutInfo.temp_supplier;
                result.temp_bg = weightOutInfo.temp_bg;
                result.remark_1 = weightOutInfo.remark_1;
                result.remark_2 = weightOutInfo.remark_2;
                result.user_id = weightOutInfo.user_id;
                result.user_edit_1 = weightOutInfo.user_edit_1;
                result.user_edit_2 = weightOutInfo.user_edit_2;
                result.user_edit_3 = weightOutInfo.user_edit_3;
                result.reprint = weightOutInfo.reprint;
                result.company = weightOutInfo.company;
                result.supplier_code = weightOutInfo.supplier_code;
                result.item_code = weightOutInfo.item_code;
                result.document_ref = weightOutInfo.document_ref;
                result.weight_in = weightOutInfo.weight_in;
                result.weight_total = weightOutInfo.weight_total;
                result.cal_in_out = weightOutInfo.cal_in_out;
            }

            return result;
        }

    }
}
