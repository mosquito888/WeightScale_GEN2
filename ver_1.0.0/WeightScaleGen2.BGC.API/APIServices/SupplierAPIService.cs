using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository;
using WeightScaleGen2.BGC.API.Common;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.Supplier;
using ILogger = WeightScaleGen2.BGC.API.Common.Logger.ILogger;

namespace WeightScaleGen2.BGC.API.APIServices
{
    public class SupplierAPIService : BaseAPIService
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;
        ILogger _logger;
        private readonly IHttpContextAccessor _context;
        private readonly UserInfoModel _userInfo;
        private SupplierRepository _supplierRepository;

        public SupplierAPIService(
            IDatabaseConnectionFactory db,
            ISecurityCommon securityCommon,
            ILogger logger,
            IHttpContextAccessor context,
            UserInfoModel userInfo,
            SupplierRepository supplierRepository) : base(db, securityCommon)
        {
            _db = db;
            _securityCommon = securityCommon;
            _logger = logger;
            _context = context;
            _userInfo = _getUserInfo(_context.HttpContext.Session.GetString(Constants.Session.User)).Result;
            _supplierRepository = supplierRepository;
        }

        public Task<ReturnObject<bool>> PostSupplierInfo(ResultGetSupplierInfoViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var res = _supplierRepository.Insert_SupplierInfo(param, _userInfo).Result;

                result.data = res.is_success;
                result.isCompleted = res.is_success;
                result.message.Add(res.message);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Supplier.Service.PostSupplierInfo}");
                _logger.WriteError(errorCode: ErrorCodes.Supplier.Service.PostSupplierInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnObject<bool>> PutSupplierInfo(ResultGetSupplierInfoViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var res = _supplierRepository.Update_SupplierInfo(param, _userInfo).Result;

                result.data = res.is_success;
                result.isCompleted = res.is_success;
                result.message.Add(res.message);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Supplier.Service.PutSupplierInfo}");
                _logger.WriteError(errorCode: ErrorCodes.Supplier.Service.PutSupplierInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnList<ResultSearchSupplierViewModel>> GetSupplierListData()
        {
            var result = new ReturnList<ResultSearchSupplierViewModel>();
            try
            {
                var lsData = _supplierRepository.Select_SupplierListData_All(_userInfo).Result;

                result.data = _initSearchSupplierListData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Supplier.Service.GetListSupplier}");
                _logger.WriteError(errorCode: ErrorCodes.Supplier.Service.GetListSupplier, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnList<ResultSearchSupplierViewModel>> GetSearchListSupplier(ParamSearchSupplierViewModel param)
        {
            var result = new ReturnList<ResultSearchSupplierViewModel>();
            try
            {
                var lsData = _supplierRepository.Select_SearchSupplierListData_By(param, _userInfo).Result;

                result.data = _initSearchSupplierListData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Supplier.Service.GetSearchListSupplier}");
                _logger.WriteError(errorCode: ErrorCodes.Supplier.Service.GetSearchListSupplier, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnObject<ResultGetSupplierInfoViewModel>> GetSupplierInfo(ParamSupplierInfo param)
        {
            var result = new ReturnObject<ResultGetSupplierInfoViewModel>();
            try
            {
                var lsData = _supplierRepository.Select_SupplierInfo(param, _userInfo).Result;

                result.data = _initSupplierInfo(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Supplier.Service.GetSupplierInfo}");
                _logger.WriteError(errorCode: ErrorCodes.Supplier.Service.GetSupplierInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnObject<bool>> DeleteInfo(ResultGetSupplierInfoViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var res = _supplierRepository.Delete_Info(param, _userInfo).Result;

                result.data = res.is_success;
                result.isCompleted = res.is_success;
                result.message.Add(res.message);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Supplier.Service.DeleteSupplierInfo}");
                _logger.WriteError(errorCode: ErrorCodes.Supplier.Service.DeleteSupplierInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        private List<ResultSearchSupplierViewModel> _initSearchSupplierListData(List<SupplierData> lsSup)
        {
            List<ResultSearchSupplierViewModel> result = new List<ResultSearchSupplierViewModel>();

            foreach (SupplierData objSup in lsSup)
            {
                result.Add(new ResultSearchSupplierViewModel
                {
                    supplier_code = objSup.supplier_code,
                    supplier_name = objSup.supplier_name,
                    status = objSup.status,
                    remark_1 = objSup.remark_1,
                    remark_2 = objSup.remark_2,
                    created_by = objSup.created_by,
                    created_date = objSup.created_date,
                    modified_by = objSup.modified_by,
                    modified_date = objSup.modified_date,
                    is_active = objSup.is_active,
                    is_deleted = objSup.is_deleted,
                    total_record = objSup.total_record
                });
            }

            return result;
        }

        private ResultGetSupplierInfoViewModel _initSupplierInfo(SupplierData supplierInfo)
        {
            ResultGetSupplierInfoViewModel result = new ResultGetSupplierInfoViewModel();
            if (supplierInfo != null)
            {
                result.supplier_code = supplierInfo.supplier_code;
                result.supplier_name = supplierInfo.supplier_name;
                result.status = supplierInfo.status;
                result.remark_1 = supplierInfo.remark_1;
                result.remark_2 = supplierInfo.remark_2;
                result.created_by = supplierInfo.created_by;
                result.created_date = supplierInfo.created_date;
                result.modified_by = supplierInfo.modified_by;
                result.modified_date = supplierInfo.modified_date;
                result.is_active = supplierInfo.is_active;
                result.is_deleted = supplierInfo.is_deleted;
            }

            return result;
        }

    }
}
