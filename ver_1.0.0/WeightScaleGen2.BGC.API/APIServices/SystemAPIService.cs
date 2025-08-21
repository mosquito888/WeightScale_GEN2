using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository;
using WeightScaleGen2.BGC.API.Common;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.System;
using ILogger = WeightScaleGen2.BGC.API.Common.Logger.ILogger;

namespace WeightScaleGen2.BGC.API.APIServices
{
    public class SystemAPIService : BaseAPIService
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;
        ILogger _logger;
        private readonly IHttpContextAccessor _context;
        private readonly UserInfoModel _userInfo;
        private SystemRepository _systemRepository;

        public SystemAPIService(
            IDatabaseConnectionFactory db,
            ISecurityCommon securityCommon,
            ILogger logger,
            IHttpContextAccessor context,
            UserInfoModel userInfo,
            SystemRepository systemRepository) : base(db, securityCommon)
        {
            _db = db;
            _securityCommon = securityCommon;
            _logger = logger;
            _context = context;
            _userInfo = _getUserInfo(_context.HttpContext.Session.GetString(Constants.Session.User)).Result;
            _systemRepository = systemRepository;
        }

        public Task<ReturnList<ResultGetSystem>> GetListSystem()
        {
            var result = new ReturnList<ResultGetSystem>();
            try
            {
                var lsData = _systemRepository.Select_SystemData(_userInfo).Result;

                result.data = _initSystemList(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.System.Service.GetListSystem}");
                _logger.WriteError(errorCode: ErrorCodes.System.Service.GetListSystem, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }

            return Task.FromResult(result);
        }

        public List<ResultGetSystem> _initSystemList(List<SystemData> masterData)
        {
            List<ResultGetSystem> result = new List<ResultGetSystem>();

            foreach (SystemData p in masterData)
            {
                result.Add(new ResultGetSystem
                {
                    sys_code = p.sys_code,
                    sys_type = p.sys_type,
                    sys_value = p.sys_value
                });
            }

            return result;
        }

    }
}
