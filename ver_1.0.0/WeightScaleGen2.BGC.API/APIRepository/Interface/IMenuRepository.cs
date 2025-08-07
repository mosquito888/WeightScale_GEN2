using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.Menu;
using WeightScaleGen2.BGC.Models.ViewModels.Role;

namespace WeightScaleGen2.BGC.API.APIRepository.Interface
{
    public interface IMenuRepository
    {
        public void Update_RoleItem(List<UpdateRoleItemSection> param, UserInfoModel userInfo);
        public void Update_RoleSelectItem(UpdateRoleItemSection param, UserInfoModel userInfo);
        public void Insert_User(PramGetMenuViewModel param, UserInfoModel userInfo);
        public void Insert_MenuSection(int roleId, UserInfoModel userInfo);
        public void Update_Role(ResultRoleInfo param, UserInfoModel userInfo);
        public void Delete_Role(ResultRoleInfo param, UserInfoModel userInfo);
        public Task<int> Insert_Role(ResultRoleInfo param, UserInfoModel userInfo);
        public Task<UserData> Select_User_ByUsername(PramGetMenuViewModel param, UserInfoModel userInfo);
        public Task<List<MenuData>> Select_MenuUser(PramGetMenuViewModel param, UserInfoModel userInfo);
        public Task<List<MenuData>> Select_MenuSectionUser(PramGetMenuViewModel param, UserInfoModel userInfo);
        public Task<List<MenuData>> Select_MenuRole(int param, UserInfoModel userInfo);
        public Task<List<MenuData>> Select_MenuSectionRole(int param, UserInfoModel userInfo);
        public Task<RoleData> Select_Role(ResultRoleInfo param, UserInfoModel userInfo);
        public Task<int> Select_RoleUsing(ResultRoleInfo param, UserInfoModel userInfo);
    }
}
