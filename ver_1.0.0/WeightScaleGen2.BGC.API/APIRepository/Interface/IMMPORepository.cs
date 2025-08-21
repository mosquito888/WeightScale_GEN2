using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.MMPO;

namespace WeightScaleGen2.BGC.API.APIRepository.Interface
{
    public interface IMMPORepository
    {
        public Task<List<MMPOData>> Select_SearchMMPOListData_By(ParamSearchMMPOViewModel param, UserInfoModel userInfo);
        public Task<List<MMPOData>> Select_SearchMMPOListData_By_CompanyCode(string companyCode, UserInfoModel userInfo);
        public Task<MMPOData> Select_SearchMMPOQtyPendingData(ParamSearchMMPOQtyPendingViewModel param, UserInfoModel userInfo);
    }
}
