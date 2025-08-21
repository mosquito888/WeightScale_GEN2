using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.ItemMaster;

namespace WeightScaleGen2.BGC.API.APIRepository.Interface
{
    public interface IItemMasterRepository
    {
        public Task<List<ItemMasterData>> Select_SearchItemMasterListData_By(ParamSearchItemMasterViewModel param, UserInfoModel userInfo);
        public Task<List<ItemMasterData>> Select_ItemMasterListData_All(UserInfoModel userInfo);
        public Task<ItemMasterData> Select_ItemMasterInfo(ParamItemMasterInfo param, UserInfoModel userInfo);
        public Task<MessageReport> Insert_ItemMasterInfo(ResultGetItemMasterInfoViewModel param, UserInfoModel userInfo);
        public Task<MessageReport> Update_ItemMasterInfo(ResultGetItemMasterInfoViewModel param, UserInfoModel userInfo);
        public Task<MessageReport> Delete_Info(ResultGetItemMasterInfoViewModel param, UserInfoModel userInfo);
    }
}
