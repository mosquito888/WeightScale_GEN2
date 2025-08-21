using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;

namespace WeightScaleGen2.BGC.API.APIRepository.Interface
{
    public interface IUOMConversionSapRepository
    {
        public Task<List<UOMConversionSapData>> Select_SearchUOMConversionSapListData_All(UserInfoModel userInfo);
    }
}
