using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.WeightSummaryDay;

namespace WeightScaleGen2.BGC.API.APIRepository.Interface
{
    public interface IWeightSummaryDayRepository
    {
        public Task<List<WeightSummaryDayData>> Select_SearchWeightSummaryDayListData_By(ParamSearchWeightSummaryDayViewModel param, UserInfoModel userInfo);
    }
}
