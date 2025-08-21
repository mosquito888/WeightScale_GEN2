using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.ReturnData;
using WeightScaleGen2.BGC.Web.Middleware;

namespace WeightScaleGen2.BGC.Web.Services
{
    public class ReturnDataService
    {
        private IConfiguration _config;
        private Uri _baseUri;
        private ApiKeyMiddleware _apiKey;

        public ReturnDataService(IConfiguration config)
        {
            _config = config;
            _baseUri = new Uri(config.GetSection("Api").GetSection("BaseUrl").Value);
            _apiKey = new ApiKeyMiddleware(this._config);
        }

        public ReturnList<ResultSearchReturnDataViewModel> GetSearchListReturnData(string username, ParamSearchReturnDataViewModel param)
        {
            var result = new ReturnList<ResultSearchReturnDataViewModel>();
            try
            {
                var options = new RestClientOptions(this._baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/ReturnData/GetSearchListReturnData", Method.Get);

                request.AddHeader("SecretKey", this._apiKey.EncryptKey());
                request.AddHeader("User", username);
                request.AddParameter("application/json", JsonConvert.SerializeObject(param), ParameterType.RequestBody);
                //request.Timeout = 0;
                var response = client.ExecuteGet(request);
                if (response.ErrorException != null)
                {
                    result.isCompleted = false;
                    result.message.Add(response.ErrorException.Message);
                    return result;
                }
                ReturnList<ResultSearchReturnDataViewModel> respObj = JsonConvert.DeserializeObject<ReturnList<ResultSearchReturnDataViewModel>>(response.Content);
                result = respObj;
                return result;
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add(ex.Message);
                return result;
            }
        }

        public ReturnObject<bool> PostDataToSAP(string username, ParamSearchReturnDataViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var options = new RestClientOptions(this._baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/ReturnData/PostDataToSAP", Method.Post);

                request.AddHeader("SecretKey", this._apiKey.EncryptKey());
                request.AddHeader("User", username);
                request.AddParameter("application/json", JsonConvert.SerializeObject(param), ParameterType.RequestBody);
                //request.Timeout = 0;
                var response = client.ExecutePost(request);
                if (response.ErrorException != null)
                {
                    result.isCompleted = false;
                    result.message.Add(response.ErrorException.Message);
                    return result;
                }
                ReturnObject<bool> respObj = JsonConvert.DeserializeObject<ReturnObject<bool>>(response.Content);
                result = respObj;
                return result;
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add(ex.Message);
                return result;
            }
        }
    }
}
