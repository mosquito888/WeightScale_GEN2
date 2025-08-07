using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository;
using WeightScaleGen2.BGC.API.Common;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.Dashboard;
using ILogger = WeightScaleGen2.BGC.API.Common.Logger.ILogger;

namespace WeightScaleGen2.BGC.API.APIServices
{
    public class DashboardAPIService : BaseAPIService
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;
        ILogger _logger;
        private readonly IHttpContextAccessor _context;
        private readonly UserInfoModel _userInfo;
        private DashboardRepository _dashboardRepository;

        public DashboardAPIService(
            IDatabaseConnectionFactory db,
            ISecurityCommon securityCommon,
            ILogger logger,
            IHttpContextAccessor context,
            UserInfoModel userInfo,
            DashboardRepository dashboardRepository) : base(db, securityCommon)
        {
            _db = db;
            _securityCommon = securityCommon;
            _logger = logger;
            _context = context;
            _userInfo = _getUserInfo(_context.HttpContext.Session.GetString(Constants.Session.User)).Result;
            _dashboardRepository = dashboardRepository;
        }

        public Task<ReturnList<ResultSearchDashboardSummaryViewModel>> GetSearchListDashboardSummary()
        {
            var result = new ReturnList<ResultSearchDashboardSummaryViewModel>();
            try
            {
                var lsData = _dashboardRepository.Select_SearchDashboardSummaryData_By(_userInfo).Result;

                result.data = _initSearchDashboardSummaryData(lsData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Dashboard.Service.GetSearchListDashboardSummaryData}");
                _logger.WriteError(errorCode: ErrorCodes.Dashboard.Service.GetSearchListDashboardSummaryData, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        public Task<ReturnObject<ResultSearchDashboardSummaryHistoryViewModel>> GetSearchListDashboardHistorySummary()
        {
            var result = new ReturnObject<ResultSearchDashboardSummaryHistoryViewModel>();
            try
            {
                var lsIData = _dashboardRepository.Select_SearchDashboardSummaryHistoryWeightInData_By(_userInfo).Result;
                var lsOData = _dashboardRepository.Select_SearchDashboardSummaryHistoryWeightOutData_By(_userInfo).Result;

                result.data = _initSearchDashboardSummaryHistoryData(lsIData, lsOData);
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add($"Error Code: {ErrorCodes.Dashboard.Service.GetSearchListDashboardSummaryHistoryData}");
                _logger.WriteError(errorCode: ErrorCodes.Dashboard.Service.GetSearchListDashboardSummaryHistoryData, errorMessage: ex.Message, additionalInfo: result.message[0], exception: ex, user: _userInfo.username);
            }
            return Task.FromResult(result);
        }

        private List<ResultSearchDashboardSummaryViewModel> _initSearchDashboardSummaryData(List<DashboardSummaryData> lsDS)
        {
            List<ResultSearchDashboardSummaryViewModel> result = new List<ResultSearchDashboardSummaryViewModel>();

            foreach (DashboardSummaryData objDS in lsDS)
            {
                result.Add(new ResultSearchDashboardSummaryViewModel
                {
                    header = objDS.header,
                    total_record = objDS.total_record
                });
            }

            return result;
        }

        private ResultSearchDashboardSummaryHistoryViewModel _initSearchDashboardSummaryHistoryData(List<DashboardSummaryHistoryData> lsWHI, List<DashboardSummaryHistoryData> lsWHO)
        {
            var result = new ResultSearchDashboardSummaryHistoryViewModel();

            foreach (DashboardSummaryHistoryData objWHI in lsWHI)
            {
                result.weight_in.Add(new ResultSearchDashboardSummaryHistoryInfoViewModel
                {
                    date = objWHI.date.ToString("dd/MM/yyyy"),
                    weight_count = objWHI.weight_count
                });
            }

            foreach (DashboardSummaryHistoryData objWHO in lsWHO)
            {
                result.weight_out.Add(new ResultSearchDashboardSummaryHistoryInfoViewModel
                {
                    date = objWHO.date.ToString("dd/MM/yyyy"),
                    weight_count = objWHO.weight_count
                });
            }

            return result;
        }
    }
}
