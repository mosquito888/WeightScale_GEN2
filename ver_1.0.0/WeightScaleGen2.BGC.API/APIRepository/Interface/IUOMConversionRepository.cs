using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.UOMConversion;

namespace WeightScaleGen2.BGC.API.APIRepository.Interface
{
    public interface IUOMConversionRepository
    {
        public Task<List<UOMConversionData>> Select_SearchUOMConversionListData_All(UserInfoModel userInfo);
        public Task<List<UOMConversionData>> Select_SearchUOMConversionListData_By(ParamSearchUOMConversionViewModel param, UserInfoModel userInfo);
        public Task<List<UOMConversionData>> Select_SearchUOMConversionListData_By_MaterialCode(string materialCode, UserInfoModel userInfo);
        public Task<MessageReport> Insert_UOMConversionInfo(ResultSearchUOMConversionViewModel param, UserInfoModel userInfo);
        public Task<MessageReport> Update_UOMConversionInfo(ResultSearchUOMConversionViewModel param, UserInfoModel userInfo);
    }
}
