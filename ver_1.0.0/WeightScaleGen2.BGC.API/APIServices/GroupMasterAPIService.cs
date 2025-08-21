using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository;
using WeightScaleGen2.BGC.API.Common;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.GroupMaster;
using ILogger = WeightScaleGen2.BGC.API.Common.Logger.ILogger;

namespace WeightScaleGen2.BGC.API.APIServices
{
    public class GroupMasterAPIService : BaseAPIService
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;
        ILogger _logger;
        private readonly IHttpContextAccessor _context;
        private readonly UserInfoModel _userInfo;
        private GroupMasterRepository _groupMasterRepository;

        public GroupMasterAPIService(
            IDatabaseConnectionFactory db,
            ISecurityCommon securityCommon,
            ILogger logger,
            IHttpContextAccessor context,
            UserInfoModel userInfo,
            GroupMasterRepository groupMasterRepository) : base(db, securityCommon)
        {
            _db = db;
            _securityCommon = securityCommon;
            _logger = logger;
            _context = context;
            _userInfo = _getUserInfo(_context.HttpContext.Session.GetString(Constants.Session.User)).Result;
            _groupMasterRepository = groupMasterRepository;
        }

        public Task<ReturnList<ResultGetGroupMasterViewModel>> GetListGroupMaster()
        {
            var result = new ReturnList<ResultGetGroupMasterViewModel>();
            try
            {
                var lsData = _groupMasterRepository.Select_GroupMasterListData_All(_userInfo).Result;

                result.data = _initGroupMasterListData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.GroupMaster.Service.GetListGroupMaster}");
                _logger.WriteError(errorCode: ErrorCodes.GroupMaster.Service.GetListGroupMaster, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }

            return Task.FromResult(result);
        }

        private List<ResultGetGroupMasterViewModel> _initGroupMasterListData(List<GroupMasterData> lsGroupMaster)
        {
            List<ResultGetGroupMasterViewModel> result = new List<ResultGetGroupMasterViewModel>();

            foreach (GroupMasterData objGroupMaster in lsGroupMaster)
            {
                result.Add(new ResultGetGroupMasterViewModel
                {
                    group_code = objGroupMaster.group_code,
                    group_name = objGroupMaster.group_name
                });
            }

            return result;
        }
    }
}
