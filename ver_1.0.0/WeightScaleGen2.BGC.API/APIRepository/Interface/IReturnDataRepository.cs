using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.SAPModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.ReturnData;

namespace WeightScaleGen2.BGC.API.APIRepository.Interface
{
    public interface IReturnDataRepository
    {
        public Task<List<ReturnData>> Select_SearchReturnDataListData_By(ParamSearchReturnDataViewModel param, UserInfoModel userInfo);
        public Task<MessageReport> Insert_ReturnData_By_Weight(string companyCode, UserInfoModel userInfo);
        public Task<MessageReport> Update_ReturnDataInfo_By_SAP(SapNcoResultsModel param, UserInfoModel userInfo);
    }
}
