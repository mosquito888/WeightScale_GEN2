using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models.ServicesModels;

namespace WeightScaleGen2.BGC.API.APIRepository.Interface
{
    public interface IWeightMasterRepository
    {
        public Task<MessageReport> Copy_Delete_WeightMaster(string companyCode, UserInfoModel userInfo);
    }
}
