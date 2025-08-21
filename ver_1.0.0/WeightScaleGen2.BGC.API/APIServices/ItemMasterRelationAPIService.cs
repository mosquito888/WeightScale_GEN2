using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository;
using WeightScaleGen2.BGC.API.Common;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.ItemMasterRelation;
using ILogger = WeightScaleGen2.BGC.API.Common.Logger.ILogger;

namespace WeightScaleGen2.BGC.API.APIServices
{
    public class ItemMasterRelationAPIService : BaseAPIService
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;
        ILogger _logger;
        private readonly IHttpContextAccessor _context;
        private readonly UserInfoModel _userInfo;
        private ItemMasterRelationRepository _itemMasterRelationRepository;

        public ItemMasterRelationAPIService(
            IDatabaseConnectionFactory db,
            ISecurityCommon securityCommon,
            ILogger logger,
            IHttpContextAccessor context,
            UserInfoModel userInfo,
            ItemMasterRelationRepository itemMasterRelationRepository) : base(db, securityCommon)
        {
            _db = db;
            _securityCommon = securityCommon;
            _logger = logger;
            _context = context;
            _userInfo = _getUserInfo(_context.HttpContext.Session.GetString(Constants.Session.User)).Result;
            _itemMasterRelationRepository = itemMasterRelationRepository;
        }

        public Task<ReturnObject<bool>> PostItemMasterRelationInfo(ResultGetItemMasterRelationInfoViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var res = _itemMasterRelationRepository.Insert_ItemMasterRelationInfo(param, _userInfo).Result;

                result.data = res.is_success;
                result.isCompleted = res.is_success;
                result.message.Add(res.message);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.ItemMasterRelation.Service.PostItemMasterRelationInfo}");
                _logger.WriteError(errorCode: ErrorCodes.ItemMasterRelation.Service.PostItemMasterRelationInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnObject<bool>> PutItemMasterRelationInfo(ResultGetItemMasterRelationInfoViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var res = _itemMasterRelationRepository.Update_ItemMasterRelationInfo(param, _userInfo).Result;

                result.data = res.is_success;
                result.isCompleted = res.is_success;
                result.message.Add(res.message);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.ItemMasterRelation.Service.PutItemMasterRelationInfo}");
                _logger.WriteError(errorCode: ErrorCodes.ItemMasterRelation.Service.PutItemMasterRelationInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnList<ResultSearchItemMasterRelationViewModel>> GetListItemMasterRelation()
        {
            var result = new ReturnList<ResultSearchItemMasterRelationViewModel>();
            try
            {
                var lsData = _itemMasterRelationRepository.Select_ItemMasterRelationListData_All(_userInfo).Result;

                result.data = _initSearchItemMasterRelationListData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.ItemMasterRelation.Service.GetListItemMasterRelation}");
                _logger.WriteError(errorCode: ErrorCodes.ItemMasterRelation.Service.GetListItemMasterRelation, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnList<ResultSearchItemMasterRelationViewModel>> GetSearchListItemMasterRelation(ParamSearchItemMasterRelationViewModel param)
        {
            var result = new ReturnList<ResultSearchItemMasterRelationViewModel>();
            try
            {
                var lsData = _itemMasterRelationRepository.Select_SearchItemMasterRelationListData_By(param, _userInfo).Result;

                result.data = _initSearchItemMasterRelationListData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.ItemMasterRelation.Service.GetSearchListItemMasterRelation}");
                _logger.WriteError(errorCode: ErrorCodes.ItemMasterRelation.Service.GetSearchListItemMasterRelation, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnObject<ResultGetItemMasterRelationInfoViewModel>> GetItemMasterRelationInfo(ParamItemMasterRelationInfo param)
        {
            var result = new ReturnObject<ResultGetItemMasterRelationInfoViewModel>();
            try
            {
                var lsData = _itemMasterRelationRepository.Select_ItemMasterRelationInfo(param, _userInfo).Result;

                result.data = _initItemMasterRelationInfo(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.ItemMasterRelation.Service.GetItemMasterRelationInfo}");
                _logger.WriteError(errorCode: ErrorCodes.ItemMasterRelation.Service.GetItemMasterRelationInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnObject<bool>> DeleteInfo(ResultGetItemMasterRelationInfoViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var res = _itemMasterRelationRepository.Delete_Info(param, _userInfo).Result;

                result.data = res.is_success;
                result.isCompleted = res.is_success;
                result.message.Add(res.message);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.ItemMasterRelation.Service.DeleteItemMasterRelationInfo}");
                _logger.WriteError(errorCode: ErrorCodes.ItemMasterRelation.Service.DeleteItemMasterRelationInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        private List<ResultSearchItemMasterRelationViewModel> _initSearchItemMasterRelationListData(List<ItemMasterRelationData> lsItMR)
        {
            List<ResultSearchItemMasterRelationViewModel> result = new List<ResultSearchItemMasterRelationViewModel>();

            foreach (ItemMasterRelationData objItMR in lsItMR)
            {
                result.Add(new ResultSearchItemMasterRelationViewModel
                {
                    id = objItMR.id,
                    supplier_code = objItMR.supplier_code,
                    supplier_name = objItMR.supplier_name,
                    item_code = objItMR.item_code,
                    item_name = objItMR.item_name,
                    humidity = objItMR.humidity,
                    gravity = objItMR.gravity,
                    status = objItMR.status,
                    remark_1 = objItMR.remark_1,
                    remark_2 = objItMR.remark_2,
                    created_by = objItMR.created_by,
                    created_date = objItMR.created_date,
                    modified_by = objItMR.modified_by,
                    modified_date = objItMR.modified_date,
                    is_active = objItMR.is_active,
                    is_deleted = objItMR.is_deleted,
                    total_record = objItMR.total_record
                });
            }

            return result;
        }

        private ResultGetItemMasterRelationInfoViewModel _initItemMasterRelationInfo(ItemMasterRelationData itemMasterRelationInfo)
        {
            ResultGetItemMasterRelationInfoViewModel result = new ResultGetItemMasterRelationInfoViewModel();
            if (itemMasterRelationInfo != null)
            {
                result.id = itemMasterRelationInfo.id;
                result.supplier_code = itemMasterRelationInfo.supplier_code;
                result.supplier_name = itemMasterRelationInfo.supplier_name;
                result.item_code = itemMasterRelationInfo.item_code;
                result.item_name = itemMasterRelationInfo.item_name;
                result.humidity = itemMasterRelationInfo.humidity;
                result.gravity = itemMasterRelationInfo.gravity;
                result.status = itemMasterRelationInfo.status;
                result.remark_1 = itemMasterRelationInfo.remark_1;
                result.remark_2 = itemMasterRelationInfo.remark_2;
                result.created_by = itemMasterRelationInfo.created_by;
                result.created_date = itemMasterRelationInfo.created_date;
                result.modified_by = itemMasterRelationInfo.modified_by;
                result.modified_date = itemMasterRelationInfo.modified_date;
                result.is_active = itemMasterRelationInfo.is_active;
                result.is_deleted = itemMasterRelationInfo.is_deleted;
            }

            return result;
        }

    }
}
