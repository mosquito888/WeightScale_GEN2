using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.Log;

namespace WeightScaleGen2.BGC.API.APIRepository.Interface
{
    public interface ILogRepository
    {
        public Task<List<LogData>> Select_ListLogDataDll_By(ParamSearchLogViewModel param, UserInfoModel userInfo);
        public Task<List<LogLevelData>> Select_ListLogLevelDll_All(UserInfoModel userInfo);
    }
}
