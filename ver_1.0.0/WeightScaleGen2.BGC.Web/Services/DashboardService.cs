using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.Dashboard;
using WeightScaleGen2.BGC.Web.Middleware;

namespace WeightScaleGen2.BGC.Web.Services
{
    public class DashboardService
    {
        private IConfiguration _config;
        private Uri _baseUri;
        private ApiKeyMiddleware _apiKey;

        public DashboardService(IConfiguration config)
        {
            _config = config;
            _baseUri = new Uri(config.GetSection("Api").GetSection("BaseUrl").Value);
            _apiKey = new ApiKeyMiddleware(this._config);
        }

        public ReturnList<ResultSearchDashboardSummaryViewModel> GetSearchListDashboard(string username)
        {
            var result = new ReturnList<ResultSearchDashboardSummaryViewModel>();
            try
            {
                var options = new RestClientOptions(this._baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/Dashboard/GetSearchListDashboardSummary", Method.Get);

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
                ReturnList<ResultSearchDashboardSummaryViewModel> respObj = JsonConvert.DeserializeObject<ReturnList<ResultSearchDashboardSummaryViewModel>>(response.Content);
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

        public ReturnObject<ResultSearchDashboardSummaryHistoryViewModel> GetSearchListDashboardHistory(string username)
        {
            var result = new ReturnObject<ResultSearchDashboardSummaryHistoryViewModel>();
            try
            {
                var options = new RestClientOptions(this._baseUri);
                options.Authenticator = new HttpBasicAuthenticator(_apiKey.BasicAuthen().username, _apiKey.BasicAuthen().password);
                var client = new RestClient(options);
                var request = new RestRequest("/api/v1/Dashboard/GetSearchListDashboardHistorySummary", Method.Get);

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
                ReturnObject<ResultSearchDashboardSummaryHistoryViewModel> respObj = JsonConvert.DeserializeObject<ReturnObject<ResultSearchDashboardSummaryHistoryViewModel>>(response.Content);
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
