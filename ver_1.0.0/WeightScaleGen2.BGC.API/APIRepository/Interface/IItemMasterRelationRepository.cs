using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.ItemMasterRelation;

namespace WeightScaleGen2.BGC.API.APIRepository.Interface
{
    public interface IItemMasterRelationRepository
    {
        public Task<MessageReport> Insert_ItemMasterRelationInfo(ResultGetItemMasterRelationInfoViewModel param, UserInfoModel userInfo);
        public Task<List<ItemMasterRelationData>> Select_SearchItemMasterRelationListData_By(ParamSearchItemMasterRelationViewModel param, UserInfoModel userInfo);
        public Task<List<ItemMasterRelationData>> Select_ItemMasterRelationListData_All(UserInfoModel userInfo);
        public Task<ItemMasterRelationData> Select_ItemMasterRelationInfo(ParamItemMasterRelationInfo param, UserInfoModel userInfo);
        public Task<MessageReport> Update_ItemMasterRelationInfo(ResultGetItemMasterRelationInfoViewModel param, UserInfoModel userInfo);
        public Task<MessageReport> Delete_Info(ResultGetItemMasterRelationInfoViewModel param, UserInfoModel userInfo);
    }
}
