using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.Plant;

namespace WeightScaleGen2.BGC.API.APIRepository.Interface
{
    public interface IPlantRepository
    {
        public Task<MessageReport> Insert_Info(ResultGetPlantInfoViewModel param, UserInfoModel userInfo);
        public Task<MessageReport> Update_Info(ResultGetPlantInfoViewModel param, UserInfoModel userInfo);
        public Task<MessageReport> Delete_Info(ResultGetPlantInfoViewModel param, UserInfoModel userInfo);
        public Task<List<PlantData>> Select_Plant_All(UserInfoModel userInfo);
        public Task<List<PlantData>> Select_Plant_By(ParamSearchPlantViewModel param, UserInfoModel userInfo);
    }
}
