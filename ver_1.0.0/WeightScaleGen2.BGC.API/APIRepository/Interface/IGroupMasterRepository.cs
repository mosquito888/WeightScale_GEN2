using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;

namespace WeightScaleGen2.BGC.API.APIRepository.Interface
{
    public interface IGroupMasterRepository
    {
        public Task<List<GroupMasterData>> Select_GroupMasterListData_All(UserInfoModel userInfo);
    }
}
