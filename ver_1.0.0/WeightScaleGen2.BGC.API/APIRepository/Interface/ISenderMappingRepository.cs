using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.SenderMapping;

namespace WeightScaleGen2.BGC.API.APIRepository.Interface
{
    public interface ISenderMappingRepository
    {
        public Task<MessageReport> Insert_SenderMappingInfo(ResultGetSenderMappingInfoViewModel param, UserInfoModel userInfo);
        public Task<MessageReport> Update_SenderMappingSenderId(ResultGetSenderMappingInfoViewModel param, UserInfoModel userInfo);
    }
}
