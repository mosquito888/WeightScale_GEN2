using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.Supplier;
using WeightScaleGen2.BGC.Web.Middleware;

namespace WeightScaleGen2.BGC.Web.Services
{
    public class SupplierService
    {
        private IConfiguration _config;
        private Uri _baseUri;
        private ApiKeyMiddleware _apiKey;

        public SupplierService(IConfiguration config)
        {
            _config = config;
            _baseUri = new Uri(config.GetSection("Api").GetSection("BaseUrl").Value);
            _apiKey = new ApiKeyMiddleware(this._config);
        }

        public ReturnList<ResultSearchSupplierViewModel> GetSupplierListData(string username)
        {
            var result = new ReturnList<ResultSearchSupplierViewModel>();
            try
            {
                var options = new RestClientOptions(this._baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/Supplier/GetSupplierListData", Method.Get);

                request.AddHeader("SecretKey", this._apiKey.EncryptKey());
                request.AddHeader("User", username);
                //request.Timeout = 0;
                var response = client.ExecuteGet(request);
                if (response.ErrorException != null)
                {
                    result.isCompleted = false;
                    result.message.Add(response.ErrorException.Message);
                    return result;
                }
                ReturnList<ResultSearchSupplierViewModel> respObj = JsonConvert.DeserializeObject<ReturnList<ResultSearchSupplierViewModel>>(response.Content);
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

        public ReturnList<ResultSearchSupplierViewModel> GetSearchSupplierListData(string username, ParamSearchSupplierViewModel param)
        {
            var result = new ReturnList<ResultSearchSupplierViewModel>();
            try
            {
                var options = new RestClientOptions(this._baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/Supplier/GetSearchSupplierListData", Method.Get);

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
                ReturnList<ResultSearchSupplierViewModel> respObj = JsonConvert.DeserializeObject<ReturnList<ResultSearchSupplierViewModel>>(response.Content);
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

        public ReturnObject<ResultGetSupplierInfoViewModel> GetSupplierInfo(string username, ParamSupplierInfo param)
        {
            var result = new ReturnObject<ResultGetSupplierInfoViewModel>();
            try
            {
                var options = new RestClientOptions(this._baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/Supplier/GetSupplierInfo", Method.Get);

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
                ReturnObject<ResultGetSupplierInfoViewModel> respObj = JsonConvert.DeserializeObject<ReturnObject<ResultGetSupplierInfoViewModel>>(response.Content);
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

        public ReturnObject<bool> CreateSupplierInfo(string username, ResultGetSupplierInfoViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var options = new RestClientOptions(this._baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/Supplier/PostSupplierInfo", Method.Post);

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

        public ReturnObject<bool> UpdateSupplierInfo(string username, ResultGetSupplierInfoViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var options = new RestClientOptions(this._baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/Supplier/PutSupplierInfo", Method.Put);

                request.AddHeader("SecretKey", this._apiKey.EncryptKey());
                request.AddHeader("User", username);
                request.AddParameter("application/json", JsonConvert.SerializeObject(param), ParameterType.RequestBody);
                //request.Timeout = 0;
                var response = client.ExecutePut(request);
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

        public ReturnObject<bool> Delete(string username, ResultGetSupplierInfoViewModel param)
        {
            var result = new ReturnObject<bool>();
            try
            {
                var options = new RestClientOptions(this._baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/Supplier/DeleteItem", Method.Delete);

                request.AddHeader("SecretKey", this._apiKey.EncryptKey());
                request.AddHeader("User", username);
                request.AddParameter("application/json", JsonConvert.SerializeObject(param), ParameterType.RequestBody);
                //request.Timeout = 0;
                var response = client.Delete(request);
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
