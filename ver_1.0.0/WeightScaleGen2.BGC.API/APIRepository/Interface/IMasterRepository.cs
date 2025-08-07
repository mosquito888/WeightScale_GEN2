using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.Master;

namespace WeightScaleGen2.BGC.API.APIRepository.Interface
{
    public interface IMasterRepository
    {
        public Task<MessageReport> Insert_Info(ResultGetMasterInfoViewModel param, UserInfoModel userInfo);
        public Task<MessageReport> Update_Info(ResultGetMasterInfoViewModel param, UserInfoModel userInfo);
        public Task<MessageReport> Delete_Info(ResultGetMasterInfoViewModel param, UserInfoModel userInfo);
        public Task<List<MasterData>> Select_MasterData(ParamSearchMasterViewModel param, UserInfoModel userInfo);
    }
}
