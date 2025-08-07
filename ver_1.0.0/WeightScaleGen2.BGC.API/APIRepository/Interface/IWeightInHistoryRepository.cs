using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.WeightInHistory;

namespace WeightScaleGen2.BGC.API.APIRepository.Interface
{
    public interface IWeightInHistoryRepository
    {
        public Task<List<WeightInHistoryData>> Select_SearchWeightInHistoryListData(UserInfoModel userInfo);
        public Task<List<WeightInHistoryData>> Select_SearchWeightInHistoryListData_By(ParamSearchWeightInHistoryViewModel param, UserInfoModel userInfo);
    }
}
