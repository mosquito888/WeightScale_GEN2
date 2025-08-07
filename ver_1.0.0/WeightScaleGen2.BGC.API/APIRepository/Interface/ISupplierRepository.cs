using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.Supplier;

namespace WeightScaleGen2.BGC.API.APIRepository.Interface
{
    public interface ISupplierRepository
    {
        public Task<MessageReport> Insert_SupplierInfo(ResultGetSupplierInfoViewModel param, UserInfoModel userInfo);
        public Task<List<SupplierData>> Select_SupplierListData_All(UserInfoModel userInfo);
        public Task<List<SupplierData>> Select_SearchSupplierListData_By(ParamSearchSupplierViewModel param, UserInfoModel userInfo);
        public Task<SupplierData> Select_SupplierInfo(ParamSupplierInfo param, UserInfoModel userInfo);
        public Task<MessageReport> Update_SupplierInfo(ResultGetSupplierInfoViewModel param, UserInfoModel userInfo);
        public Task<MessageReport> Delete_Info(ResultGetSupplierInfoViewModel param, UserInfoModel userInfo);
    }
}
