using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;

namespace WeightScaleGen2.BGC.API.APIRepository.Interface
{
    public interface IDashboardRepository
    {
        public Task<List<DashboardSummaryData>> Select_SearchDashboardSummaryData_By(UserInfoModel userInfo);
        public Task<List<DashboardSummaryHistoryData>> Select_SearchDashboardSummaryHistoryWeightInData_By(UserInfoModel userInfo);
        public Task<List<DashboardSummaryHistoryData>> Select_SearchDashboardSummaryHistoryWeightOutData_By(UserInfoModel userInfo);
    }
}
