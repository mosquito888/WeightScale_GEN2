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
using WeightScaleGen2.BGC.Models.ViewModels.ItemMaster;
using WeightScaleGen2.BGC.Models.ViewModels.SenderMapping;
using WeightScaleGen2.BGC.Models.ViewModels.WeightIn;
using ILogger = WeightScaleGen2.BGC.API.Common.Logger.ILogger;

namespace WeightScaleGen2.BGC.API.APIServices
{
    public class WeightInAPIService : BaseAPIService
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;
        ILogger _logger;
        private readonly IHttpContextAccessor _context;
        private readonly UserInfoModel _userInfo;
        private WeightInRepository _weightInRepository;
        private WeightOutRepository _weightOutRepository;
        private SenderMappingRepository _senderMappingRepository;
        private ItemMasterRepository _itemMasterRepository;
        private IdentNumberRepository _identNumberRepository;
        private ItemMasterRelationRepository _itemMasterRelationRepository;

        public WeightInAPIService(
            IDatabaseConnectionFactory db,
            ISecurityCommon securityCommon,
            ILogger logger,
            IHttpContextAccessor context,
            UserInfoModel userInfo,
            WeightInRepository weightInRepository,
            WeightOutRepository weightOutRepository,
            SenderMappingRepository senderMappingRepository,
            ItemMasterRepository itemMasterRepository,
            IdentNumberRepository identNumberRepository,
            ItemMasterRelationRepository itemMasterRelationRepository) : base(db, securityCommon)
        {
            _db = db;
            _securityCommon = securityCommon;
            _logger = logger;
            _context = context;
            _userInfo = _getUserInfo(_context.HttpContext.Session.GetString(Constants.Session.User)).Result;
            _weightInRepository = weightInRepository;
            _weightOutRepository = weightOutRepository;
            _senderMappingRepository = senderMappingRepository;
            _itemMasterRepository = itemMasterRepository;
            _identNumberRepository = identNumberRepository;
            _itemMasterRelationRepository = itemMasterRelationRepository;
        }

        public Task<ReturnObject<int>> PostWeightInInfo(ResultGetWeightInInfoViewModel param)
        {
            var result = new ReturnObject<int>();
            try
            {
                // Get Item Name
                param.item_name = _itemMasterRepository.Select_ItemMasterInfo(new ParamItemMasterInfo { product_code = param.item_code }, _userInfo).Result.item_name;
                // Get Company Code
                param.company = _userInfo.comp_code;

                // Create Weight In no
                string weightInNo = _identNumberRepository.Select_IdentNumber(new ParamGetIdentNumberViewModel { type = "I", company = _userInfo.comp_code }, _userInfo).Result;
                param.weight_in_no = weightInNo;

                var res = _weightInRepository.Insert_WeightInInfo(param, _userInfo).Result;

                _ = _senderMappingRepository.Insert_SenderMappingInfo(new ResultGetSenderMappingInfoViewModel { weight_in_no = weightInNo, sender_id = param.sender_id }, _userInfo).Result;

                var id = _weightInRepository.Select_SearchWeightInListData_By(new ParamSearchWeightInViewModel { weight_in_no = weightInNo, start = 0, draw = int.MaxValue, length = int.MaxValue }, _userInfo).Result.FirstOrDefault();

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

        public Task<ReturnObject<bool>> PutWeightInInfo(ResultGetWeightInInfoViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                param.item_name = _itemMasterRepository.Select_ItemMasterListData_All(_userInfo).Result.Where(i => i.item_code == param.item_code).FirstOrDefault().item_name;
                var res = _weightInRepository.Update_WeightInInfo(param, _userInfo).Result;
                if (param.status == "Complete")
                {
                    _ = _weightOutRepository.Update_WeightOutCarLicense(new Models.ViewModels.WeightOut.ResultGetWeightOutInfoViewModel { weight_in_no = param.weight_in_no, car_license = param.car_license }, _userInfo).Result;
                }

                if (param.sender_id > 0)
                {
                    _ = _senderMappingRepository.Update_SenderMappingSenderId(new Models.ViewModels.SenderMapping.ResultGetSenderMappingInfoViewModel { weight_in_no = param.weight_in_no, sender_id = param.sender_id }, _userInfo).Result;
                }

                result.data = res.is_success;
                result.isCompleted = res.is_success;
                result.message.Add(res.message);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.WeightIn.Service.PutWeightInInfo}");
                _logger.WriteError(errorCode: ErrorCodes.WeightIn.Service.PutWeightInInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnObject<bool>> PutWeightInStatus(ResultGetWeightInInfoViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var data = _weightInRepository.Select_WeightInInfo(new ParamWeightInInfo { id = param.id }, _userInfo).Result;
                if (data != null && data.status == "Complete")
                {
                    result.isCompleted = false;
                    result.message.Add("รายการชั่งได้ถูกชั่งออกไปแล้ว");
                    return Task.FromResult(result);
                }

                var res = _weightInRepository.Update_WeightInStatus(param, _userInfo).Result;

                result.data = res.is_success;
                result.isCompleted = res.is_success;
                result.message.Add(res.message);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.WeightIn.Service.PutWeightInInfo}");
                _logger.WriteError(errorCode: ErrorCodes.WeightIn.Service.PutWeightInInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnList<ResultSearchWeightInViewModel>> GetSearchListWeightIn(ParamSearchWeightInViewModel param)
        {
            var result = new ReturnList<ResultSearchWeightInViewModel>();
            try
            {
                var lsData = _weightInRepository.Select_SearchWeightInListData_By(param, _userInfo).Result;

                result.data = _initSearchWeightInListData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.WeightIn.Service.GetSearchListWeightIn}");
                _logger.WriteError(errorCode: ErrorCodes.WeightIn.Service.GetSearchListWeightIn, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnObject<ResultGetWeightInInfoViewModel>> GetWeightInInfo(ParamWeightInInfo param)
        {
            var result = new ReturnObject<ResultGetWeightInInfoViewModel>();
            try
            {
                var lsData = _weightInRepository.Select_WeightInInfo(param, _userInfo).Result;

                result.data = _initWeightInInfo(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.WeightIn.Service.GetWeightInInfo}");
                _logger.WriteError(errorCode: ErrorCodes.WeightIn.Service.GetWeightInInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnObject<ResultGetWeightInInfoViewModel>> GetWeightInInfoByCarLicense(ParamWeightInInfo param)
        {
            var result = new ReturnObject<ResultGetWeightInInfoViewModel>();
            try
            {
                // Check License Format
                if (!CheckLicenseFormat(param.car_type, param.car_license))
                {
                    result.isCompleted = false;
                    result.message.Add($"รูปแบบป้ายทะเบียนไม่ถูกต้อง");
                    return Task.FromResult(result);
                }

                param.company_code = _userInfo.comp_code;

                var lsData = _weightInRepository.Select_WeightInInfoByCarLicense(param, _userInfo).Result;

                if (lsData == null)
                {
                    result.isCompleted = false;
                    result.message.Add($"ไม่พบทะเบียนรถคันนี้ในระบบ");
                    return Task.FromResult(result);
                }

                result.data = _initWeightInInfo(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.WeightIn.Service.GetWeightInInfoByCarLicense}");
                _logger.WriteError(errorCode: ErrorCodes.WeightIn.Service.GetWeightInInfoByCarLicense, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        private List<ResultSearchWeightInViewModel> _initSearchWeightInListData(List<WeightInData> lsWI)
        {
            List<ResultSearchWeightInViewModel> result = new List<ResultSearchWeightInViewModel>();

            foreach (WeightInData objWI in lsWI)
            {
                result.Add(new ResultSearchWeightInViewModel
                {
                    id = objWI.id,
                    weight_in_no = objWI.weight_in_no,
                    weight_in_type = objWI.weight_in_type,
                    line_number = objWI.line_number,
                    item_code = objWI.item_code,
                    item_name = objWI.item_name,
                    supplier_code = objWI.supplier_code,
                    car_license = objWI.car_license,
                    car_type = objWI.car_type,
                    document_po = objWI.document_po,
                    doc_type_po = objWI.doc_type_po,
                    document_ref = objWI.document_ref,
                    weight_in = objWI.weight_in,
                    date = objWI.date,
                    user_id = objWI.user_id,
                    status = objWI.status,
                    user_edit_1 = objWI.user_edit_1,
                    user_edit_2 = objWI.user_edit_2,
                    user_edit_3 = objWI.user_edit_3,
                    remark_1 = objWI.remark_1,
                    remark_2 = objWI.remark_2,
                    reprint = objWI.reprint,
                    company = objWI.company,
                    doc_start = objWI.doc_start,
                    doc_stop = objWI.doc_stop,
                    doc_send = objWI.doc_send,
                    edi = objWI.edi,
                    edi_sand = objWI.edi_sand,
                    total_record = objWI.total_record
                });
            }

            return result;
        }

        private ResultGetWeightInInfoViewModel _initWeightInInfo(WeightInData weightInInfo)
        {
            var item = _itemMasterRelationRepository.Select_ItemMasterRelationListData_All(_userInfo).Result.Where(i => i.item_code == weightInInfo.item_code).FirstOrDefault();
            ResultGetWeightInInfoViewModel result = new ResultGetWeightInInfoViewModel();
            if (weightInInfo != null)
            {
                result.id = weightInInfo.id;
                result.weight_in_no = weightInInfo.weight_in_no;
                result.weight_in_type = weightInInfo.weight_in_type;
                result.line_number = weightInInfo.line_number;
                result.item_code = weightInInfo.item_code;
                result.item_name = weightInInfo.item_name;
                result.supplier_code = weightInInfo.supplier_code;
                result.car_license = weightInInfo.car_license;
                result.car_type = weightInInfo.car_type;
                result.document_po = weightInInfo.document_po;
                result.doc_type_po = weightInInfo.doc_type_po;
                result.document_ref = weightInInfo.document_ref;
                result.weight_in = weightInInfo.weight_in;
                result.date = weightInInfo.date;
                result.user_id = weightInInfo.user_id;
                result.status = weightInInfo.status;
                result.user_edit_1 = weightInInfo.user_edit_1;
                result.user_edit_2 = weightInInfo.user_edit_2;
                result.user_edit_3 = weightInInfo.user_edit_3;
                result.remark_1 = weightInInfo.remark_1;
                result.remark_2 = weightInInfo.remark_2;
                result.reprint = weightInInfo.reprint;
                result.company = weightInInfo.company;
                result.doc_start = weightInInfo.doc_start;
                result.doc_stop = weightInInfo.doc_stop;
                result.doc_send = weightInInfo.doc_send;
                result.edi = weightInInfo.edi;
                result.edi_sand = weightInInfo.edi_sand;
                result.sender_id = weightInInfo.sender_id;
                result.cal_in_out = weightInInfo.cal_in_out;
                result.percent_humidity_ok = item != null ? item.humidity : 0;
            }

            return result;
        }

        private bool CheckLicenseFormat(string carType, string carLicense)
        {
            int index = carLicense.IndexOf("-");
            string otherChar = carLicense.Substring(index + 1);
            int indexChar = otherChar.IndexOf("-");
            if ((index <= 2 && index == 0) && indexChar > -1)
            {
                return false;
            }
            else
            {
                return true;
            }
            //string singleCarPattern = @"^[ก-ฮ]{1}[ก-ฮ]{1}\s\d{4}$";          // Format for รถเดี่ยว (e.g., กก 1234)
            //string trailerPattern = @"^[ขค]{1}[บ-ฮ]{1}\s\d{4}$";             // Format for รถเทรลเลอร์ (e.g., ขบ 1234)
            //string towCarPattern = @"^[ก-ฮ]{1}[ก-ฮ]{1}\s\d{4}$";            // Format for รถพ่วง (e.g., กก 1234)
            //string otherVehiclePattern = @"^[ก-ฮ]{1}[ก-ฮ]{1}\s\d{4}(\s\(.*\))?$"; // Format for รถอื่นๆ (with optional region code)

            //switch (carType)
            //{
            //    case "รถเดี่ยว":
            //        return Regex.IsMatch(carLicense, singleCarPattern);
            //    case "รถเทรลเลอร์":
            //        return Regex.IsMatch(carLicense, trailerPattern);
            //    case "รถพ่วง":
            //        return Regex.IsMatch(carLicense, towCarPattern);
            //    case "รถอื่นๆ":
            //        return Regex.IsMatch(carLicense, otherVehiclePattern);
            //    default:
            //        return false;
            //}
        }

    }
}
