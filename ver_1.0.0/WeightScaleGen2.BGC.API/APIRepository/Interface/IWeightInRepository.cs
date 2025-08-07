using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.WeightIn;

namespace WeightScaleGen2.BGC.API.APIRepository.Interface
{
    public interface IWeightInRepository
    {
        public Task<List<WeightInData>> Select_SearchWeightInListData_By(ParamSearchWeightInViewModel param, UserInfoModel userInfo);
        public Task<WeightInData> Select_WeightInInfo(ParamWeightInInfo param, UserInfoModel userInfo);
        public Task<WeightInData> Select_WeightInInfoByCarLicense(ParamWeightInInfo param, UserInfoModel userInfo);
        public Task<MessageReport> Insert_WeightInInfo(ResultGetWeightInInfoViewModel param, UserInfoModel userInfo);
        public Task<MessageReport> Update_WeightInInfo(ResultGetWeightInInfoViewModel param, UserInfoModel userInfo);
        public Task<MessageReport> Update_WeightInStatus(ResultGetWeightInInfoViewModel param, UserInfoModel userInfo);
    }
}
