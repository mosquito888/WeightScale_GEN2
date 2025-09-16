using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.WeighingScale;
using WeightScaleGen2.BGC.Web.Middleware;

namespace WeightScaleGen2.BGC.Web.Services
{
    public class WeighingScaleService
    {
        private IConfiguration _config;
        private Uri _baseUri;
        private ApiKeyMiddleware _apiKey;

        public WeighingScaleService(IConfiguration config)
        {
            _config = config;
            _baseUri = new Uri(config.GetSection("WeighingApi").GetSection("BaseUrl").Value);
            _apiKey = new ApiKeyMiddleware(this._config);
        }

        public ReturnObject<string> GetConnectWeighingScale(string username)
        {
            var result = new ReturnObject<string>();
            try
            {
                var options = new RestClientOptions(this._baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("api/runservice", Method.Get);

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
                result.isCompleted = true;
                result.data = response.Content.Trim('"');
                return result;
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add(ex.Message);
                return result;
            }
        }

        public ReturnObject<decimal> GetWeightByWeighingScale(string username)
        {
            var result = new ReturnObject<decimal>();
            try
            {
                var options = new RestClientOptions(this._baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("api/weight" , Method.Get);

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
                result.isCompleted = true;
                result.data = decimal.Parse(response.Content.Trim('"'));
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
