using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.WeightHistory;
using WeightScaleGen2.BGC.Web.Middleware;

namespace WeightScaleGen2.BGC.Web.Services
{
    public class WeightHistoryService
    {
        private IConfiguration _config;
        private Uri _baseUri;
        private ApiKeyMiddleware _apiKey;

        public WeightHistoryService(IConfiguration config)
        {
            _config = config;
            _baseUri = new Uri(config.GetSection("Api").GetSection("BaseUrl").Value);
            _apiKey = new ApiKeyMiddleware(this._config);
        }

        //public ReturnList<ResultSearchItemMasterViewModel> GetItemMasterListData(string username)
        //{
        //    var result = new ReturnList<ResultSearchItemMasterViewModel>();
        //    try
        //    {
        //        var options = new RestClientOptions(this._baseUri);
        //        options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
        //        var client = new RestClient(options);
        //        var request = new RestRequest("/api/v1/ItemMaster/GetItemMasterListData", Method.Get);

        //        request.AddHeader("SecretKey", this._apiKey.EncryptKey());
        //        request.AddHeader("User", username);
        //        //request.Timeout = 0;
        //        var response = client.ExecuteGet(request);
        //        if (response.ErrorException != null)
        //        {
        //            result.isCompleted = false;
        //            result.message.Add(response.ErrorException.Message);
        //            return result;
        //        }
        //        ReturnList<ResultSearchItemMasterViewModel> respObj = JsonConvert.DeserializeObject<ReturnList<ResultSearchItemMasterViewModel>>(response.Content);
        //        result = respObj;
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        result.isCompleted = false;
        //        result.message.Add(ex.Message);
        //        return result;
        //    }
        //}

        public ReturnList<ResultSearchWeightHistoryViewModel> GetSearchWeightHistoryListData(string username, ParamSearchWeightHistoryViewModel param)
        {
            var result = new ReturnList<ResultSearchWeightHistoryViewModel>();
            try
            {
                var options = new RestClientOptions(this._baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/WeightHistory/GetSearchWeightHistoryListData", Method.Get);

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
                ReturnList<ResultSearchWeightHistoryViewModel> respObj = JsonConvert.DeserializeObject<ReturnList<ResultSearchWeightHistoryViewModel>>(response.Content);
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

        public ReturnObject<ResultGetWeightHistoryInfoViewModel> GetWeightHistoryInfo(string username, ParamWeightHistoryInfo param)
        {
            var result = new ReturnObject<ResultGetWeightHistoryInfoViewModel>();
            try
            {
                var options = new RestClientOptions(this._baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/WeightHistory/GetWeightHistoryInfo", Method.Get);

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
                ReturnObject<ResultGetWeightHistoryInfoViewModel> respObj = JsonConvert.DeserializeObject<ReturnObject<ResultGetWeightHistoryInfoViewModel>>(response.Content);
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
