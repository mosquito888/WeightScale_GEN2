using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;

namespace WeightScaleGen2.BGC.API.APIRepository.Interface
{
    public interface ISystemRepository
    {
        public Task<List<SystemData>> Select_SystemData(UserInfoModel userInfo);
    }
}
