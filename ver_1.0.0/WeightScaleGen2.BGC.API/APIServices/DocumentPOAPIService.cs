using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository;
using WeightScaleGen2.BGC.API.Common;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.DocumentPO;
using ILogger = WeightScaleGen2.BGC.API.Common.Logger.ILogger;

namespace WeightScaleGen2.BGC.API.APIServices
{
    public class DocumentPOAPIService : BaseAPIService
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;
        ILogger _logger;
        private readonly IHttpContextAccessor _context;
        private readonly UserInfoModel _userInfo;
        private DocumentPORepository _documentPORepository;

        public DocumentPOAPIService(
            IDatabaseConnectionFactory db,
            ISecurityCommon securityCommon,
            ILogger logger,
            IHttpContextAccessor context,
            UserInfoModel userInfo,
            DocumentPORepository documentPORepository) : base(db, securityCommon)
        {
            _db = db;
            _securityCommon = securityCommon;
            _logger = logger;
            _context = context;
            _userInfo = _getUserInfo(_context.HttpContext.Session.GetString(Constants.Session.User)).Result;
            _documentPORepository = documentPORepository;
        }

        public Task<ReturnList<ResultSearchDocumentPOViewModel>> GetSearchListDocumentPO(ParamSearchDocumentPOViewModel param)
        {
            var result = new ReturnList<ResultSearchDocumentPOViewModel>();
            try
            {
                var lsData = _documentPORepository.Select_SearchDocumentPOListData_By(param, _userInfo).Result;

                result.data = _initSearchDocumentPOListData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.DocumentPO.Service.GetSearchListDocumentPO}");
                _logger.WriteError(errorCode: ErrorCodes.DocumentPO.Service.GetSearchListDocumentPO, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnObject<ResultSearchDocumentPOViewModel>> GetDocumentPOInfo(string purchase_number)
        {
            var result = new ReturnObject<ResultSearchDocumentPOViewModel>();
            try
            {
                var lsData = _documentPORepository.Select_SearchDocumentPOListData_By_PurchaseNumber(purchase_number, _userInfo).Result;

                if (lsData == null)
                {
                    result.isCompleted = false;
                    result.message.Add($"ไม่พบเลขเอกสาร PO ในระบบ");
                    return Task.FromResult(result);
                }

                result.data = _initDocumentPOInfo(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.DocumentPO.Service.GetDocumentPOInfo}");
                _logger.WriteError(errorCode: ErrorCodes.DocumentPO.Service.GetDocumentPOInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        private List<ResultSearchDocumentPOViewModel> _initSearchDocumentPOListData(List<DocumentPOData> lsPO)
        {
            List<ResultSearchDocumentPOViewModel> result = new List<ResultSearchDocumentPOViewModel>();

            foreach (DocumentPOData objPO in lsPO)
            {
                result.Add(new ResultSearchDocumentPOViewModel
                {
                    purchase_number = objPO.purchase_number,
                    num_of_rec = objPO.num_of_rec,
                    company_code = objPO.company_code,
                    plant = objPO.plant,
                    storage_loc = objPO.storage_loc,
                    status = objPO.status,
                    vender_code = objPO.vender_code,
                    vender_name = objPO.vender_name,
                    material_code = objPO.material_code,
                    material_desc = objPO.material_desc,
                    order_qty = objPO.order_qty,
                    uom = objPO.uom,
                    uom_in = objPO.uom_in,
                    good_received = objPO.good_received,
                    pending_qty = objPO.pending_qty,
                    pending_qty_all = objPO.pending_qty_all,
                    allowance = objPO.allowance,
                    dlv_complete = objPO.dlv_complete,
                    created_by = objPO.created_by,
                    created_date = objPO.created_date,
                    created_time = objPO.created_time,
                    modified_by = objPO.modified_by,
                    modified_date = objPO.modified_date,
                    modified_time = objPO.modified_time,
                    is_active = objPO.is_active,
                    is_deleted = objPO.is_deleted,
                    total_record = objPO.total_record
                });
            }

            return result;
        }

        private ResultSearchDocumentPOViewModel _initDocumentPOInfo(DocumentPOData documentPOInfo)
        {
            ResultSearchDocumentPOViewModel result = new ResultSearchDocumentPOViewModel();
            if (documentPOInfo != null)
            {
                result.purchase_number = documentPOInfo.purchase_number;
                result.num_of_rec = documentPOInfo.num_of_rec;
                result.company_code = documentPOInfo.company_code;
                result.plant = documentPOInfo.plant;
                result.storage_loc = documentPOInfo.storage_loc;
                result.status = documentPOInfo.status;
                result.vender_code = documentPOInfo.vender_code;
                result.vender_name = documentPOInfo.vender_name;
                result.material_code = documentPOInfo.material_code;
                result.material_desc = documentPOInfo.material_desc;
                result.order_qty = documentPOInfo.order_qty;
                result.uom = documentPOInfo.uom;
                result.uom_in = documentPOInfo.uom_in;
                result.good_received = documentPOInfo.good_received;
                result.pending_qty = documentPOInfo.pending_qty;
                result.pending_qty_all = documentPOInfo.pending_qty_all;
                result.allowance = documentPOInfo.allowance;
                result.dlv_complete = documentPOInfo.dlv_complete;
                result.created_by = documentPOInfo.created_by;
                result.created_date = documentPOInfo.created_date;
                result.created_time = documentPOInfo.created_time;
                result.modified_by = documentPOInfo.modified_by;
                result.modified_date = documentPOInfo.modified_date;
                result.modified_time = documentPOInfo.modified_time;
                result.is_active = documentPOInfo.is_active;
                result.is_deleted = documentPOInfo.is_deleted;
            }

            return result;
        }

    }
}
