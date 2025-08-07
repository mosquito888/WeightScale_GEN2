using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.IdentNumber;

namespace WeightScaleGen2.BGC.API.APIRepository.Interface
{
    public interface IIdentNumberRepository
    {
        public Task<string> Select_IdentNumber(ParamGetIdentNumberViewModel param, UserInfoModel userInfo);
    }
}
