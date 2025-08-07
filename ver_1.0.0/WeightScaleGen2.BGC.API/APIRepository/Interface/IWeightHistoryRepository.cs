using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.WeightHistory;

namespace WeightScaleGen2.BGC.API.APIRepository.Interface
{
    public interface IWeightHistoryRepository
    {
        public Task<List<WeightHistoryData>> Select_SearchWeightHistoryListData_By(ParamSearchWeightHistoryViewModel param, UserInfoModel userInfo);
        public Task<WeightHistoryData> Select_WeightHistoryInfo(ParamWeightHistoryInfo param, UserInfoModel userInfo);
        public Task<decimal> Select_SumQtyWeightHistory_By(ParamGetSumQtyWeightHistoryViewModel param, UserInfoModel userInfo);
    }
}
