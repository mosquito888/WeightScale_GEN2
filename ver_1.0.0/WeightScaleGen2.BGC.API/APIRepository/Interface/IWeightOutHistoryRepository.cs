using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.WeightOutHistory;

namespace WeightScaleGen2.BGC.API.APIRepository.Interface
{
    public interface IWeightOutHistoryRepository
    {
        public Task<List<WeightOutHistoryData>> Select_SearchWeightOutHistoryListData(UserInfoModel userInfo);
        public Task<List<WeightOutHistoryData>> Select_SearchWeightOutHistoryListData_By(ParamSearchWeightOutHistoryViewModel param, UserInfoModel userInfo);
    }
}
