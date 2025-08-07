using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.Employee;

namespace WeightScaleGen2.BGC.API.APIRepository.Interface
{
    public interface IEmployeeRepository
    {
        public Task<MessageReport> Insert_EmployeeInfo(ResultGetEmpInfoViewModel param, UserInfoModel userInfo);
        public Task<MessageReport> Update_EmployeeInfo(ResultGetEmpInfoViewModel param, UserInfoModel userInfo);
        public Task<EmployeeData> Select_EmployeeInfo(ParamEmpInfo param, UserInfoModel userInfo);
        public Task<List<EmployeeData>> Select_EmployeeListData_All(UserInfoModel userInfo);
        public Task<List<EmployeeData>> Select_SearchEmployeeListData_By(ParamSearchEmpViewModel param, UserInfoModel userInfo);
    }
}
