using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models.DBModels;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.DocumentPO;

namespace WeightScaleGen2.BGC.API.APIRepository.Interface
{
    public interface IDocumentPORepository
    {
        public Task<MessageReport> Insert_DocumentPOInfo(ResultGetDocumentPOInfoViewModel param, UserInfoModel userInfo);
        public Task<List<DocumentPOData>> Select_SearchDocumentPOListData_All(UserInfoModel userInfo);
        public Task<List<DocumentPOData>> Select_SearchDocumentPOListData_By(ParamSearchDocumentPOViewModel param, UserInfoModel userInfo);
        public Task<DocumentPOData> Select_SearchDocumentPOListData_By_PurchaseNumber(string purchase_number, UserInfoModel userInfo);
        public void Delete_DocumentPO_By_CompanyCode(string CompanyCode, UserInfoModel userInfo);
        public Task<MessageReport> Update_DocumentPOInfo(ResultGetDocumentPOInfoViewModel param, UserInfoModel userInfo);
    }
}
