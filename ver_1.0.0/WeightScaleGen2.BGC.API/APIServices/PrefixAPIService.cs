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
    public class PrefixAPIService : BaseAPIService
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;
        ILogger _logger;
        private readonly IHttpContextAccessor _context;
        private readonly UserInfoModel _userInfo;
        private PrefixDocRepository _prefixDocRepository;

        public PrefixAPIService(
            IDatabaseConnectionFactory db,
            ISecurityCommon securityCommon,
            ILogger logger,
            IHttpContextAccessor context,
            UserInfoModel userInfo,
            PrefixDocRepository prefixDocRepository) : base(db, securityCommon)
        {
            _db = db;
            _securityCommon = securityCommon;
            _logger = logger;
            _context = context;
            _userInfo = _getUserInfo(_context.HttpContext.Session.GetString(Constants.Session.User)).Result;
            _prefixDocRepository = prefixDocRepository;
        }

        public Task<ReturnObject<string>> GetPrefixDoc(string docType, string compCode, string plantCode, string plantShortCode)
        {
            var result = new ReturnObject<string>();
            try
            {
                result.data = _prefixDocRepository.Select_RunningCode(docType, compCode, plantCode, plantShortCode).Result;
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.PrefixDoc.Service.GetPrefixDoc}");
                _logger.WriteError(errorCode: ErrorCodes.PrefixDoc.Service.GetPrefixDoc, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }

            return Task.FromResult(result);
        }
    }
}
