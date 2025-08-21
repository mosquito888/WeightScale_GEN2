using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.Department;

namespace WeightScaleGen2.BGC.API.APIRepository.Interface
{
    public interface IDepartmentRepository
    {
        public Task<MessageReport> Insert_Info(ResultGetDeptInfoViewModel param, UserInfoModel userInfo);
        public Task<MessageReport> Update_Info(ResultGetDeptInfoViewModel param, UserInfoModel userInfo);
        public Task<MessageReport> Delete_Info(ResultGetDeptInfoViewModel param, UserInfoModel userInfo);
        public Task<List<DepartmentData>> Select_DepartmentData_All();
        public Task<List<DepartmentData>> Select_DepartmentData_By(ParamSearchDeptViewModel param, UserInfoModel userInfo);
    }
}
