using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.ViewModels.Dashboard
{
    public class ResultSearchDashboardSummaryHistoryViewModel
    {
        public List<ResultSearchDashboardSummaryHistoryInfoViewModel> weight_in { get; set; } = new List<ResultSearchDashboardSummaryHistoryInfoViewModel>();
        public List<ResultSearchDashboardSummaryHistoryInfoViewModel> weight_out { get; set; } = new List<ResultSearchDashboardSummaryHistoryInfoViewModel>();
    }

    public class ResultSearchDashboardSummaryHistoryInfoViewModel
    {
        public string date { get; set; }
        public int weight_count { get; set; }
    }
}
