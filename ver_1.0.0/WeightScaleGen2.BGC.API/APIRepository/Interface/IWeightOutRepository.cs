using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.WeightHistory;
using WeightScaleGen2.BGC.Models.ViewModels.WeightOut;

namespace WeightScaleGen2.BGC.API.APIRepository.Interface
{
    public interface IWeightOutRepository
    {
        public Task<List<WeightOutData>> Select_SearchWeightOutListData_By(ParamSearchWeightOutViewModel param, UserInfoModel userInfo);
        public Task<WeightOutData> Select_WeightOutInfo(ParamWeightOutInfo param, UserInfoModel userInfo);
        public Task<WeightOutData> Select_WeightOutInfoByCarLicense(ParamWeightOutInfo param, UserInfoModel userInfo);
        public Task<MessageReport> Insert_WeightOutInfo(ResultGetWeightOutInfoViewModel param, UserInfoModel userInfo);
        public Task<decimal> Select_SumQtyWeightOut_By(ParamGetSumQtyWeightHistoryViewModel param, UserInfoModel userInfo);
        public Task<MessageReport> Update_WeightOutInfo(ResultGetWeightOutInfoViewModel param, UserInfoModel userInfo);
        public Task<MessageReport> Update_WeightOutStatus(ResultGetWeightOutInfoViewModel param, UserInfoModel userInfo);
        public Task<MessageReport> Update_WeightOutCarLicense(ResultGetWeightOutInfoViewModel param, UserInfoModel userInfo);
    }
}
