using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.WeightCompare;

namespace WeightScaleGen2.BGC.API.APIRepository.Interface
{
    public interface IWeightCompareRepository
    {
        public Task<List<WeightCompareData>> Select_SearchWeightCompareListData_By(ParamSearchWeightCompareViewModel param, UserInfoModel userInfo);
    }
}
