using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository;
using WeightScaleGen2.BGC.API.Common;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.SAPModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.ReturnData;
using ILogger = WeightScaleGen2.BGC.API.Common.Logger.ILogger;

namespace WeightScaleGen2.BGC.API.APIServices
{
    public class ReturnDataAPIService : BaseAPIService
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;
        ILogger _logger;
        private readonly IHttpContextAccessor _context;
        private readonly UserInfoModel _userInfo;
        private ReturnDataRepository _returnDataRepository;
        private SapAPIService _sapAPIService;

        public ReturnDataAPIService(
            IDatabaseConnectionFactory db,
            ISecurityCommon securityCommon,
            ILogger logger,
            IHttpContextAccessor context,
            UserInfoModel userInfo,
            ReturnDataRepository returnDataRepository,
            SapAPIService sapAPIService) : base(db, securityCommon)
        {
            _db = db;
            _securityCommon = securityCommon;
            _logger = logger;
            _context = context;
            _userInfo = _getUserInfo(_context.HttpContext.Session.GetString(Constants.Session.User)).Result;
            _returnDataRepository = returnDataRepository;
            _sapAPIService = sapAPIService;
        }

        public Task<ReturnList<ResultSearchReturnDataViewModel>> GetSearchListReturnData(ParamSearchReturnDataViewModel param)
        {
            var result = new ReturnList<ResultSearchReturnDataViewModel>();
            try
            {
                var lsData = _returnDataRepository.Select_SearchReturnDataListData_By(param, _userInfo).Result;

                result.data = _initSearchReturnDataListData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.ReturnData.Service.GetSearchListReturnData}");
                _logger.WriteError(errorCode: ErrorCodes.ReturnData.Service.GetSearchListReturnData, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnObject<bool>> PostDataToSAP(ParamSearchReturnDataViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var lsData = _returnDataRepository.Select_SearchReturnDataListData_By(param, _userInfo).Result;
                var sapList = new List<SapNcoModel>();
                if (lsData.Count > 0)
                {
                    foreach (var item in lsData)
                    {
                        if (item.send_data != "Y" && item.message_type != "S")
                        {
                            var res = new SapNcoModel();
                            res.ZWGDOC = item.weight_out_no;
                            res.ZWGDOC_SEQ = Convert.ToInt32(item.sequence);
                            res.GR_TYPE = item.gr_type.ToString();
                            res.DOC_DATE = item.doc_date.ToString("dd/MM/yyyy");
                            res.PSTNG_DATE = item.post_date.ToString("dd/MM/yyyy");
                            res.REF_DOC_NO = item.ref_doc;
                            res.GOODSMVT_CODE = item.good_movement;
                            res.MATERIAL = item.material;
                            res.PLANT = item.plant;
                            res.STGE_LOC = item.sloc;
                            res.STCK_TYPE = item.stock_type;
                            res.ITEM_TEXT = item.item_text;
                            res.PO_NO = item.po_number;
                            res.PO_ITEM = Convert.ToInt32(item.po_line_number);
                            res.TRUCK_NO = item.truck_no;
                            res.WEIGHT_IN = item.weight_in;
                            res.WEIGHT_OUT = item.weight_out;
                            res.WEIGHT_REC = item.weight_rec;
                            res.VEND_WEIGHT = item.weight_vendor;
                            res.REJ_WEIGHT = item.weight_reject;
                            res.WEIGHT_UNIT = item.weight_unit;
                            res.P_STDATE = item.doc_start.Value.ToString("dd/MM/yyyy");
                            res.P_ENDATE = item.doc_stop.Value.ToString("dd/MM/yyyy");
                            res.ZPERM = item.doc_send;
                            res.REV_MJAHR = item.document_year.ToString();
                            res.REV_MBLNR = item.material_document;
                            res.CREATE_UPD_BY = param.user;
                            sapList.Add(res);
                        }
                    }

                    var insSap = _sapAPIService.SubmissionData(sapList).Result;
                    if (!insSap.isCompleted)
                    {
                        result.isCompleted = false;
                        result.message.Add($"Insert Data to SAP Server Failed");
                        return Task.FromResult(result);
                    }
                }
                else
                {
                    result.isCompleted = false;
                    result.message.Add($"Data not found");
                    return Task.FromResult(result);
                }

                result.data = true;
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.ReturnData.Service.PostReturnDataInfo}");
                _logger.WriteError(errorCode: ErrorCodes.ReturnData.Service.PostReturnDataInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        private List<ResultSearchReturnDataViewModel> _initSearchReturnDataListData(List<ReturnData> lsRT)
        {
            List<ResultSearchReturnDataViewModel> result = new List<ResultSearchReturnDataViewModel>();

            foreach (ReturnData objRT in lsRT)
            {
                result.Add(new ResultSearchReturnDataViewModel
                {
                    weight_out_no = objRT.weight_out_no,
                    weight_in_no = objRT.weight_in_no,
                    sequence = objRT.sequence,
                    gr_type = objRT.gr_type,
                    doc_date = objRT.doc_date,
                    post_date = objRT.post_date,
                    ref_doc = objRT.ref_doc,
                    good_movement = objRT.good_movement,
                    material = objRT.material,
                    plant = objRT.plant,
                    sloc = objRT.sloc,
                    stock_type = objRT.stock_type,
                    item_text = objRT.item_text,
                    po_number = objRT.po_number,
                    po_line_number = objRT.po_line_number,
                    truck_no = objRT.truck_no,
                    weight_in = objRT.weight_in,
                    weight_out = objRT.weight_out,
                    weight_rec = objRT.weight_rec,
                    weight_vendor = objRT.weight_vendor,
                    weight_reject = objRT.weight_reject,
                    weight_unit = objRT.weight_unit,
                    doc_start = objRT.doc_start,
                    doc_stop = objRT.doc_stop,
                    doc_send = objRT.doc_send,
                    message_type = objRT.message_type,
                    message = objRT.message,
                    send_data = objRT.send_data,
                    material_document = objRT.material_document,
                    document_year = objRT.document_year,
                    total_record = objRT.total_record
                });
            }

            return result;
        }

    }
}
