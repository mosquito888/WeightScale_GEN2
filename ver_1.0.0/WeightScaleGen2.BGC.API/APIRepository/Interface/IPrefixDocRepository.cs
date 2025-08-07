using System.Threading.Tasks;

namespace WeightScaleGen2.BGC.API.APIRepository.Interface
{
    public interface IPrefixDocRepository
    {
        public Task<string> Select_RunningCode(string docType, string compCode, string plantCode, string plantShortCode);
    }
}
