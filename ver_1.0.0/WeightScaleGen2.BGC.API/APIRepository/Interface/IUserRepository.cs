using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.User;

namespace WeightScaleGen2.BGC.API.APIRepository.Interface
{
    public interface IUserRepository
    {
        public void Insert_User(ResultGetUserInfo param);
        public void Upload_Image(ParamUploadImage param);
        public Task<MessageReport> Update_User(ParamUpdateUser param, UserInfoModel userInfo);
        public Task<UserData> Select_User_ById(int param);
        public Task<UserData> Select_User_ByUsername(string username);
        public Task<UserData> Select_User_ByName(string name);
        public Task<List<UserData>> Select_User(ParamSearchUser param);
        public Task<List<RoleData>> Select_RoleDll();
        public Task<List<FileData>> Select_ImageAll();
        public Task<UserData> Select_User_ByUsernamePassword(ParamLoginUser param);
    }
}
