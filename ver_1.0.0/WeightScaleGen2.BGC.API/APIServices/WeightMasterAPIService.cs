using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository;
using WeightScaleGen2.BGC.API.Common;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.ServicesModels;
using ILogger = WeightScaleGen2.BGC.API.Common.Logger.ILogger;

namespace WeightScaleGen2.BGC.API.APIServices
{
    public class WeightMasterAPIService : BaseAPIService
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;
        ILogger _logger;
        private readonly IHttpContextAccessor _context;
        private readonly UserInfoModel _userInfo;
        private WeightMasterRepository _weightMasterRepository;
        private ReturnDataRepository _returnDataRepository;

        public WeightMasterAPIService(
            IDatabaseConnectionFactory db,
            ISecurityCommon securityCommon,
            ILogger logger,
            IHttpContextAccessor context,
            UserInfoModel userInfo,
            WeightMasterRepository weightMasterRepository,
            ReturnDataRepository returnDataRepository) : base(db, securityCommon)
        {
            _db = db;
            _securityCommon = securityCommon;
            _logger = logger;
            _context = context;
            _userInfo = _getUserInfo(_context.HttpContext.Session.GetString(Constants.Session.User)).Result;
            _weightMasterRepository = weightMasterRepository;
            _returnDataRepository = returnDataRepository;
        }

        public Task<ReturnObject<bool>> CopyDeleteWeightMaster()
        {
            var result = new ReturnObject<bool>();
            try
            {
                _ = _returnDataRepository.Insert_ReturnData_By_Weight(_userInfo.comp_code, _userInfo).Result;
                _ = _weightMasterRepository.Copy_Delete_WeightMaster(_userInfo.comp_code, _userInfo).Result;

                result.data = true;
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.WeightMaster.Service.CopyDeleteWeightMaster}");
                _logger.WriteError(errorCode: ErrorCodes.WeightMaster.Service.CopyDeleteWeightMaster, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }
    }
}
