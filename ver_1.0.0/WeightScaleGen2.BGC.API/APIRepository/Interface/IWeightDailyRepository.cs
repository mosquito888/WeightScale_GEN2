using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.WeightDaily;

namespace WeightScaleGen2.BGC.API.APIRepository.Interface
{
    public interface IWeightDailyRepository
    {
        public Task<List<WeightDailyData>> Select_SearchWeightDailyListData_By(ParamSearchWeightDailyViewModel param, UserInfoModel userInfo);
    }
}
