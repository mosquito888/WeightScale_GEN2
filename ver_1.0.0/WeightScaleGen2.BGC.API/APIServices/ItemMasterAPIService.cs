using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository;
using WeightScaleGen2.BGC.API.Common;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.ItemMaster;
using ILogger = WeightScaleGen2.BGC.API.Common.Logger.ILogger;

namespace WeightScaleGen2.BGC.API.APIServices
{
    public class ItemMasterAPIService : BaseAPIService
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;
        ILogger _logger;
        private readonly IHttpContextAccessor _context;
        private readonly UserInfoModel _userInfo;
        private PrefixDocRepository _prefixDocRepository;
        private ItemMasterRepository _itemMasterRepository;

        public ItemMasterAPIService(
            IDatabaseConnectionFactory db,
            ISecurityCommon securityCommon,
            ILogger logger,
            IHttpContextAccessor context,
            UserInfoModel userInfo,
            PrefixDocRepository prefixDocRepository,
            ItemMasterRepository itemMasterRepository) : base(db, securityCommon)
        {
            _db = db;
            _securityCommon = securityCommon;
            _logger = logger;
            _context = context;
            _userInfo = _getUserInfo(_context.HttpContext.Session.GetString(Constants.Session.User)).Result;
            _prefixDocRepository = prefixDocRepository;
            _itemMasterRepository = itemMasterRepository;
        }

        public Task<ReturnObject<bool>> PostItemMasterInfo(ResultGetItemMasterInfoViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var res = _itemMasterRepository.Insert_ItemMasterInfo(param, _userInfo).Result;

                result.data = res.is_success;
                result.isCompleted = res.is_success;
                result.message.Add(res.message);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.ItemMaster.Service.PostItemMasterInfo}");
                _logger.WriteError(errorCode: ErrorCodes.ItemMaster.Service.PostItemMasterInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnObject<bool>> PutItemMasterInfo(ResultGetItemMasterInfoViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var res = _itemMasterRepository.Update_ItemMasterInfo(param, _userInfo).Result;

                result.data = res.is_success;
                result.isCompleted = res.is_success;
                result.message.Add(res.message);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.ItemMaster.Service.PutItemMasterInfo}");
                _logger.WriteError(errorCode: ErrorCodes.ItemMaster.Service.PutItemMasterInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnList<ResultSearchItemMasterViewModel>> GetListItemMaster()
        {
            var result = new ReturnList<ResultSearchItemMasterViewModel>();
            try
            {
                var lsData = _itemMasterRepository.Select_ItemMasterListData_All(_userInfo).Result;

                result.data = _initSearchItemMasterListData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.ItemMaster.Service.GetListItemMaster}");
                _logger.WriteError(errorCode: ErrorCodes.ItemMaster.Service.GetListItemMaster, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnList<ResultSearchItemMasterViewModel>> GetSearchListItemMaster(ParamSearchItemMasterViewModel param)
        {
            var result = new ReturnList<ResultSearchItemMasterViewModel>();
            try
            {
                var lsData = _itemMasterRepository.Select_SearchItemMasterListData_By(param, _userInfo).Result;

                result.data = _initSearchItemMasterListData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.ItemMaster.Service.GetSearchListItemMaster}");
                _logger.WriteError(errorCode: ErrorCodes.ItemMaster.Service.GetSearchListItemMaster, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnObject<ResultGetItemMasterInfoViewModel>> GetItemMasterInfo(ParamItemMasterInfo param)
        {
            var result = new ReturnObject<ResultGetItemMasterInfoViewModel>();
            try
            {
                var lsData = _itemMasterRepository.Select_ItemMasterInfo(param, _userInfo).Result;

                result.data = _initItemMasterInfo(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.ItemMaster.Service.GetItemMasterInfo}");
                _logger.WriteError(errorCode: ErrorCodes.ItemMaster.Service.GetItemMasterInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnObject<bool>> DeleteInfo(ResultGetItemMasterInfoViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var res = _itemMasterRepository.Delete_Info(param, _userInfo).Result;

                result.data = res.is_success;
                result.isCompleted = res.is_success;
                result.message.Add(res.message);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.ItemMaster.Service.DeleteItemMasterInfo}");
                _logger.WriteError(errorCode: ErrorCodes.ItemMaster.Service.DeleteItemMasterInfo, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        private List<ResultSearchItemMasterViewModel> _initSearchItemMasterListData(List<ItemMasterData> lsItM)
        {
            List<ResultSearchItemMasterViewModel> result = new List<ResultSearchItemMasterViewModel>();

            foreach (ItemMasterData objItM in lsItM)
            {
                result.Add(new ResultSearchItemMasterViewModel
                {
                    item_shot = objItM.item_shot,
                    item_code = objItM.item_code,
                    item_name = objItM.item_name,
                    item_group = objItM.item_group,
                    status = objItM.status,
                    remark_1 = objItM.remark_1,
                    remark_2 = objItM.remark_2,
                    created_by = objItM.created_by,
                    created_date = objItM.created_date,
                    modified_by = objItM.modified_by,
                    modified_date = objItM.modified_date,
                    is_active = objItM.is_active,
                    is_deleted = objItM.is_deleted,
                    total_record = objItM.total_record
                });
            }

            return result;
        }

        private ResultGetItemMasterInfoViewModel _initItemMasterInfo(ItemMasterData itemMasterInfo)
        {
            ResultGetItemMasterInfoViewModel result = new ResultGetItemMasterInfoViewModel();
            if (itemMasterInfo != null)
            {
                result.item_shot = itemMasterInfo.item_shot;
                result.item_code = itemMasterInfo.item_code;
                result.item_name = itemMasterInfo.item_name;
                result.item_group = itemMasterInfo.item_group;
                result.status = itemMasterInfo.status;
                result.remark_1 = itemMasterInfo.remark_1;
                result.remark_2 = itemMasterInfo.remark_2;
                result.created_by = itemMasterInfo.created_by;
                result.created_date = itemMasterInfo.created_date;
                result.modified_by = itemMasterInfo.modified_by;
                result.modified_date = itemMasterInfo.modified_date;
                result.is_active = itemMasterInfo.is_active;
                result.is_deleted = itemMasterInfo.is_deleted;
            }

            return result;
        }

    }
}
