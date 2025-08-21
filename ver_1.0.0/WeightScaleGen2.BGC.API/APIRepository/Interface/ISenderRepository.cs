using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.Sender;

namespace WeightScaleGen2.BGC.API.APIRepository.Interface
{
    public interface ISenderRepository
    {
        public Task<MessageReport> Insert_SenderInfo(ResultGetSenderInfoViewModel param, UserInfoModel userInfo);
        public Task<List<SenderData>> Select_SenderListData_All(UserInfoModel userInfo);
        public Task<List<SenderData>> Select_SearchSenderListData_By(ParamSearchSenderViewModel param, UserInfoModel userInfo);
        public Task<SenderData> Select_SenderInfo(ParamSenderInfo param, UserInfoModel userInfo);
        public Task<MessageReport> Update_SenderInfo(ResultGetSenderInfoViewModel param, UserInfoModel userInfo);
        public Task<MessageReport> Delete_Info(ResultGetSenderInfoViewModel param, UserInfoModel userInfo);
    }
}
