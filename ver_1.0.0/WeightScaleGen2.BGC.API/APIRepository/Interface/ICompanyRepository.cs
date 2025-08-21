using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.Company;

namespace WeightScaleGen2.BGC.API.APIRepository.Interface
{
    public interface ICompanyRepository
    {
        public Task<MessageReport> Insert_Info(ResultGetCompInfoViewModel param, UserInfoModel userInfo);
        public Task<MessageReport> Update_Info(ResultGetCompInfoViewModel param, UserInfoModel userInfo);
        public Task<MessageReport> Delete_Info(ResultGetCompInfoViewModel param, UserInfoModel userInfo);
        public Task<List<CompanyData>> Select_Company_All(UserInfoModel userInfo);
        public Task<List<CompanyData>> Select_Company_By(ParamSearchCompViewModel param, UserInfoModel userInfo);
    }
}
