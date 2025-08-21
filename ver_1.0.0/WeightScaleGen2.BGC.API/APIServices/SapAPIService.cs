using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.API.APIRepository;
using WeightScaleGen2.BGC.API.Common;
using WeightScaleGen2.BGC.Models;
using WeightScaleGen2.BGC.Models.SAPModels;
using WeightScaleGen2.BGC.Models.ServicesModels;

namespace WeightScaleGen2.BGC.API.APIServices
{
    public class SapAPIService : BaseAPIService
    {
        private readonly UserInfoModel _userInfo;
        private ReturnDataRepository _returnDataRespository;
        private IConfiguration _config;
        private Uri _baseUri;

        public SapAPIService(IDatabaseConnectionFactory db, ISecurityCommon securityCommon, UserInfoModel userInfo, ReturnDataRepository returnDataRespository, IConfiguration config) : base(db, securityCommon)
        {
            _userInfo = userInfo;
            _returnDataRespository = returnDataRespository;
            _config = config;
            _baseUri = new Uri(config.GetSection("Api").GetSection("SapUrl").Value);
        }

        public Task<ReturnObject<bool>> SubmissionData(List<SapNcoModel> sapDatas)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var sapRes = new ReturnList<SapNcoResultsModel>();
                var client = new RestClient(this._baseUri);
                var request = new RestRequest("/sap/bc/zmm_bridgeweigh?", Method.Post);

                request.AddParameter("application/json", JsonConvert.SerializeObject(sapDatas), ParameterType.RequestBody);
                var response = client.ExecutePost(request);
                if (response.ErrorException != null)
                {
                    result.data = false;
                    result.isCompleted = false;
                    result.message.Add(response.ErrorException.Message);
                    return Task.FromResult(result);
                }
                var respObj = JsonConvert.DeserializeObject<Dictionary<string, List<SapNcoResultsModel>>>(response.Content);
                if (respObj != null && respObj.ContainsKey("return"))
                {
                    foreach (var item in respObj["return"])
                    {
                        if (item.msgty != "E")
                        {
                            // update results datas.
                            _ = _returnDataRespository.Update_ReturnDataInfo_By_SAP(item, _userInfo).Result;
                        }
                    }
                }

                result.data = true;
                result.isCompleted = true;
                result.message.Add(Constants.Result.Success);
            }
            catch (Exception ex)
            {
                result.data = false;
                result.isCompleted = false;
                result.message.Add(ex.Message);
            }

            return Task.FromResult(result);
        }
    }
}
